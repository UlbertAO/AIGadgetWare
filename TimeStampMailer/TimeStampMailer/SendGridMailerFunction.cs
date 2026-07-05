using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace TimeStampMailer
{
    public class SendGridMailerFunction
    {
        [FunctionName("SendGridMailerFunction")]
        public async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function SendGridMailerFunction executed at: {DateTime.Now}");

            // Retrieve SendGrid API key from environment variables
            string apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                log.LogError("SendGrid API key is missing. Please set the SENDGRID_API_KEY environment variable.");
                return;
            }

            // Set sender and recipient email addresses
            string senderEmail = Environment.GetEnvironmentVariable("SMTP_ADDRESS");
            string recipientEmails = Environment.GetEnvironmentVariable("RECIPIENT_EMAILS");

            if (string.IsNullOrEmpty(senderEmail))
            {
                throw new InvalidOperationException("Sender email address is missing. Please set the SMTP_ADDRESS environment variable.");
            }

            if (string.IsNullOrEmpty(recipientEmails))
            {
                throw new InvalidOperationException("Recipient email addresses are missing. Please set the RECIPIENT_EMAILS environment variable.");
            }

            string recipientEmail = recipientEmails.Split(',').Select(e => e.Trim()).FirstOrDefault();
            if (string.IsNullOrEmpty(recipientEmail))
            {
                throw new InvalidOperationException("Recipient email address is invalid or empty.");
            }

            // Create a SendGrid client
            var client = new SendGridClient(apiKey);

            // Create the email message
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(senderEmail),
                Subject = "Your subject goes here",
                HtmlContent = "<strong>Testing</strong>"
            };
            msg.AddTo(new EmailAddress(recipientEmail));

            try
            {
                // Send the email
                var response = await client.SendEmailAsync(msg);
                log.LogInformation($"Email sent successfully via SendGrid. Response Status Code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                log.LogError($"Error sending email via SendGrid: {ex.Message}");
            }
        }
    }
}
