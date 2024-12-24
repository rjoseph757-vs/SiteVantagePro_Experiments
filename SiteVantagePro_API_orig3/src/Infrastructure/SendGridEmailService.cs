using Application.Common.Interfaces;
using Application.Notifications.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using SendGrid;
using SendGrid.Helpers.Mail;
using SiteVantagePro_API.Domain.Extensions;

namespace Infrastructure
{
    public partial class SendGridEmailService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SendGridEmailService> _logger;

        public SendGridEmailService(IConfiguration configuration, ILogger<SendGridEmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public Task SendAsync(MessageDto message)
        {
            if (!string.IsNullOrEmpty(message.Subject) && !string.IsNullOrEmpty(message.Body) && !string.IsNullOrEmpty(message.To))
            {
                if (string.IsNullOrEmpty(message.From))
                {
                    message.From = _configuration.GetSection("SendGridEmailFrom").Value!;
                }

                return Execute(message.Subject, message.Body, message.To, message.From);
                //return Task.CompletedTask; //debugging only (comment line above and use this line)
            }
            return Task.CompletedTask; //default
        }

        public async Task<Response> Execute(string subject, string body, string toEmail, string fromEmail)
        {
            // Get SendGrid client ready
            var apiKey = _configuration.GetSection("SENDGRID_API_KEY").Value!;
            string apiUrl = _configuration.GetSection("SendGridAPIURL").Value!;
            string apiVersion = _configuration.GetSection("SendGridAPIVersion").Value!;
            var from = new EmailAddress(fromEmail);

            _logger.LogInformation("toEmail: {toEmail}", toEmail);
            // Split out emails if they are a semi-colon separated list
            var toEmailList = new List<EmailAddress>();
            foreach (var toEm in toEmail.ToLower().Split(";", StringSplitOptions.None))
            {
                if (StringExtensions.IsValidEmail(toEm.Trim()))
                {
                    toEmailList.Add(new EmailAddress(toEm.Trim()));
                }
                else
                {
                    // Try anyway, but log it for comparison
                    toEmailList.Add(new EmailAddress(toEm.Trim()));
                    _logger.LogWarning("Invalid Email?: {toEm.Trim()}", toEm.Trim());
                }
            }
            toEmailList = toEmailList.Distinct().ToList();
            var client = new SendGridClient(apiKey, apiUrl, null, apiVersion);
            string contentPlainText = GetPlainTextFromHtml(body);
            string contentHtml = body;

            //--https://sendgrid.com/docs/for-developers/sending-email/v3-csharp-code-example/
            // The "sender" cannot have multiple email addresses!!!
            //Issue: --https://github.com/sendgrid/sendgrid-csharp/pull/1080/commits/66f0ca6999ef69218252e29b258cc42e4c1ad609
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, contentPlainText, contentHtml); //Plain Text MUST be first according to RFC 1341, section 7.2
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, toEmailList, subject, contentPlainText, contentHtml); //Plain Text MUST be first according to RFC 1341, section 7.2
            // Disable click tracking. - See --https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            // Send email and track any errors
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                _logger.LogDebug("SendGrid problem: {response.StatusCode}", response.StatusCode);
                _logger.LogError("response.StatusCode: {response.StatusCode}", response.StatusCode);
                _logger.LogError("response.Body.ReadAsStringAsync().Result: {+ response.Body.ReadAsStringAsync().Result} ", response.Body.ReadAsStringAsync().Result);
                _logger.LogError("response.Headers.ToString(): {response.Headers.ToString()}", response.Headers.ToString());
                throw new ExternalException("Error sending message");
            }

            _logger.LogInformation("SendGrid Execute Subject:[{subject}] ToEmail:[{toEmail}]", subject, toEmail);
            return response;
        }

        private static string GetPlainTextFromHtml(string htmlString)
        {
            // First make concession for linebreaks
            htmlString = htmlString.Replace("<br />", Environment.NewLine).Replace("<br/>", Environment.NewLine).Replace("<br>", Environment.NewLine);
            string htmlTagPattern = "<.*?>";
            var regexCss = MyRegex();
            htmlString = regexCss.Replace(htmlString, string.Empty);
            htmlString = Regex.Replace(htmlString, htmlTagPattern, string.Empty);
            htmlString = MyRegex1().Replace(htmlString, "");
            htmlString = htmlString.Replace(" ", string.Empty);

            return htmlString;
        }

        [GeneratedRegex(@"(\\<script(.+?)\\)|(\\<style(.+?)\\)", RegexOptions.IgnoreCase | RegexOptions.Singleline, "en-US")]
        private static partial Regex MyRegex();

        [GeneratedRegex(@"^\s+$[\r\n]*", RegexOptions.Multiline)]
        private static partial Regex MyRegex1();
    }
}
