using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using System.Linq;

namespace TimeStampMailer
{
    public class TimeStampMailerFunction
    {
        [FunctionName("TimeStampMailer")]
        public async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            // Retrieve SMTP configuration from local.settings.json
            var smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST");
            var smtpPort = Environment.GetEnvironmentVariable("SMTP_PORT"); // Gmail SMTP port
            var smtpAddress = Environment.GetEnvironmentVariable("SMTP_ADDRESS");
            var smtpName = Environment.GetEnvironmentVariable("SMTP_USER_NAME");
            var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");// Not you regular gmail password but password generated from "APP PASSWORD"

            // List of email addresses to send emails to
            var recipients = Environment.GetEnvironmentVariable("RECIPIENT_EMAILS")?.Split(',').Select(e => e.Trim()).ToArray();

            if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpPort) || string.IsNullOrEmpty(smtpAddress) || string.IsNullOrEmpty(smtpName) || string.IsNullOrEmpty(smtpPassword) || recipients == null || recipients.Length == 0)
            {
                log.LogError("SMTP credentials or recipient emails are missing.");
                return;
            }

            // Compose the email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtpName, smtpAddress)); // Sender name and address
            foreach (var recipient in recipients)
            {
                message.To.Add(MailboxAddress.Parse(recipient)); // Add recipients
            }

            message.Subject = $"Test Email from Azure Function executed at: {DateTime.Now}"; // Email subject
            message.Body = new TextPart("plain")
            {
                Text = "This is a test email sent from Azure Function using Gmail SMTP!" // Email body
            };

            try
            {
                // Connect to the SMTP server and send the email
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtpHost, Int32.Parse(smtpPort), false);
                    await client.AuthenticateAsync(smtpAddress, smtpPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                log.LogInformation("Email sent successfully!");
            }
            catch (Exception ex)
            {
                log.LogError($"Error sending email: {ex.Message}");
            }

        }
    }
}
