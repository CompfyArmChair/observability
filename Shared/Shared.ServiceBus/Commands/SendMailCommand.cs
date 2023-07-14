namespace Shared.ServiceBus.Commands;

public class SendMailCommand
{
	public string Firstname { get; set; } = string.Empty;
	public string Lastname { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Subject { get; set; } = string.Empty;
	public string Body { get; set; } = string.Empty;
}
