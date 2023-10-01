using EmailApi.Instrumentation;
using MassTransit;
using MimeKit;
using Shared.ServiceBus.Commands;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace EmailApi.CommandConsumers;

public class SendMailCommandConsumer : IConsumer<SendMailCommand>
{
	private readonly OtelMeters _meters;
    public SendMailCommandConsumer(OtelMeters meters)
    {
		_meters = meters;
    }

    public async Task Consume(ConsumeContext<SendMailCommand> context)
	{
		var message = context.Message;

		var emailMessage = new MimeMessage();
		emailMessage.From.Add(new MailboxAddress("Martin", "your_email@your_domain.com"));
		emailMessage.To.Add(new MailboxAddress($"{message.Firstname} {message.Lastname}", message.Email));
		emailMessage.Subject = message.Subject;
		emailMessage.Body = new TextPart("plain") { Text = message.Body };

		await SendEmail(emailMessage);
	}
	
	private async Task SendEmail(MimeMessage emailMessage)
	{
		using var client = new SmtpClient();

		await client.ConnectAsync("host.docker.internal", 2525, MailKit.Security.SecureSocketOptions.None);

		// await client.AuthenticateAsync("your_username", "your_password");
		
		await client.SendAsync(emailMessage);
		
		await client.DisconnectAsync(true);

		_meters.EmailSent();
	}
}