using Shared.Instrumentation;
using System.Diagnostics.Metrics;

namespace EmailApi.Instrumentation;

public class OtelMeters : IOtelMeter
{	
	private Counter<int> EmailSentCounter { get; }
	
	public string MeterName { get; }

	public OtelMeters(string meterName = "Shop")
	{
		var meter = new Meter(meterName);
		MeterName = meterName;

		EmailSentCounter = meter.CreateCounter<int>("domain.shop.email.sent", "Email");		
	}
	
	public void EmailSent() => EmailSentCounter.Add(1);	

}
