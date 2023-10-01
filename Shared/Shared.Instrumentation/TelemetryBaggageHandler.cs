using System.Collections.Concurrent;
using System.Diagnostics;

namespace Shared.Instrumentation;

public class TelemetryBaggageHandler
{
    private static readonly ConcurrentDictionary<Type, ConcurrentBag<TelemetryBaggageHandler>> BaggageHandlerDictionary = new();

    protected TelemetryBaggageHandler(string propertyName, Type baggageSourceType)
    {
        PropertyName = propertyName;
        BaggageSourceType = baggageSourceType;
    }

    public Type BaggageSourceType { get; }
    public string PropertyName { get; }

    public static void AddBaggage(string propertyName, object propertyValue) => Activity.Current?.AddBaggage(propertyName, propertyValue.ToString());

    public static void AddBaggage<TBaggageSource>(Activity activity, TBaggageSource baggageSource)
    {
        if (!BaggageHandlerDictionary.TryGetValue(typeof(TBaggageSource), out var baggageCollectors))
            return;

        foreach (var baggageHandler in baggageCollectors.OfType<TelemetryBaggageHandler<TBaggageSource>>())
        {
            activity.AddBaggage(baggageHandler.PropertyName, baggageHandler.GetValue(baggageSource));
        }
    }

    public static void AddBaggageFrom<TBaggageSource>(TBaggageSource baggageSource)
    {
        var activity = Activity.Current;
        if (activity is not null)
            AddBaggage(activity, baggageSource);
    }

    public static void Register<TBaggageSource>(string propertyName, Func<TBaggageSource, string?> valueAccessor)
    {
        var baggageCollectors = BaggageHandlerDictionary.GetOrAdd(typeof(TBaggageSource), _ => new ConcurrentBag<TelemetryBaggageHandler>());
        baggageCollectors.Add(new TelemetryBaggageHandler<TBaggageSource>(propertyName, valueAccessor));
    }
}

internal class TelemetryBaggageHandler<TBaggageSource> : TelemetryBaggageHandler
{
    private readonly Func<TBaggageSource, string?> _valueAccessor;

    public TelemetryBaggageHandler(string propertyName, Func<TBaggageSource, string?> valueAccessor)
        : base(propertyName, typeof(TBaggageSource))
    {
        _valueAccessor = valueAccessor;
    }

    public string GetValue(TBaggageSource baggageSource) => _valueAccessor.Invoke(baggageSource) ?? string.Empty;
}