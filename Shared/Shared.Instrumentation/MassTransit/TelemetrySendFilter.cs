using MassTransit;

namespace Shared.Instrumentation.MassTransit
{
    public class TelemetrySendFilter<TMessage> : 
        IFilter<PublishContext<TMessage>>,
        IFilter<SendContext<TMessage>>
        where TMessage : class
    {
        public Task Send(SendContext<TMessage> context, IPipe<SendContext<TMessage>> next)
        {
            TelemetryBaggageHandler.AddBaggageFrom(context.Message);

            return next.Send(context);
        }

        public Task Send(PublishContext<TMessage> context, IPipe<PublishContext<TMessage>> next)
        {
            TelemetryBaggageHandler.AddBaggageFrom(context.Message);

            return next.Send(context);
        }

        public void Probe(ProbeContext context)
        {
        }
    }
}
