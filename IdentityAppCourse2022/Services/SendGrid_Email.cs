using IdentityAppCourse2022.helpers;
using IdentityAppCourse2022.interfaces;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IdentityAppCourse2022.Services
{
    public class SendGrid_Email: ISendGrid_Email
    {
        private readonly ILogger<SendGrid_Email> _logger;

        public AuthMessageSendOptions Options { get; set; }

        public SendGrid_Email(IOptions<AuthMessageSendOptions> options,ILogger<SendGrid_Email> logger)
        {
            Options = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail,string subject,string message)
        {
            if(string.IsNullOrEmpty(Options.ApiKey))
            {
                throw new Exception("Null SendGrid Key");
                
            }

            await Execute(Options.ApiKey,subject,message,toEmail);
        }

        private async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From=new EmailAddress("aarunkumar334@gmail.com"),
                Subject=subject,
                PlainTextContent=message,
                HtmlContent=message
            };
            msg.AddTo(new EmailAddress(toEmail));

            msg.SetClickTracking(false, false);
            var response=await client.SendEmailAsync(msg);
            var dummy = response.StatusCode;
            var dummy2 = response.Headers;
            _logger.LogInformation(response.IsSuccessStatusCode
                   ? $"Email to {toEmail} queued successfully..!"
                   : $"Failed Email to {toEmail}");
        }
    }
}
