using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using rest_with_asp_net_10_cviana.Mail.Settings;

namespace rest_with_asp_net_10_cviana.Mail
{
    public class EmailSender(EmailSettings settings, ILogger<EmailSender> logger)
    {
        private readonly ILogger<EmailSender> _logger = logger;
        private readonly EmailSettings _settings = settings;

        private string _to;
        private string _subject;
        private string _body;
        private string? _attachment;
        private readonly List<MailboxAddress> _recipients = [];

        public SecureSocketOptions SecuritySocketOptions { get; private set; }

        public EmailSender To(string to)
        {
            _to = to;
            _recipients.Clear();
            _recipients.AddRange(ParseRecipient(to));
            return this;
        }

        public EmailSender WithSubject(string subject)
        {
            _subject = subject;
            return this;
        }

        public EmailSender WithMessage(string message)
        {
            _body = message;
            return this;
        }

        public EmailSender Attachment(string filePath)
        {
            if(File.Exists(filePath))
            {
                _attachment = filePath;
            }
            else
            {
                _logger.LogWarning("Attachment file not found");
            }
            return this;
        }

        public void Send()
        {
            MimeMessage message = new();
            message.From.Add(new MailboxAddress(_settings.From, _settings.Username));
            message.To.AddRange(_recipients);
            message.Subject = _subject ?? _settings.Subject ?? "Automated Generated Subject";
            BodyBuilder builder = new()
            {
                TextBody = _body ?? _settings.Message ?? ""
            };
            if(!string.IsNullOrWhiteSpace(_attachment))
            {
                var fileName = Path.GetFileName(_attachment);
                builder.Attachments.Add(fileName, File.ReadAllBytes(_attachment));
            }
            message.Body = builder.ToMessageBody();
            try
            {
                using var client = new SmtpClient();
                client.Connect(_settings.Host, _settings.Port, _settings.Ssl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                client.Authenticate(_settings.Username, _settings.Password);
                client.Send(message);
                client.Disconnect(true);
                _logger.LogInformation("E-mail successfully sent to {Recipients}", string.Join(";", _recipients));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send e-mail to {Recipients}", string.Join(";", _recipients));
                throw;
            }
            finally
            {
                Reset();
            }
        }

        private void Reset()
        {
            _to = null;
            _subject = null;
            _body = null;
            _attachment = null;
            _recipients.Clear();
            _logger.LogInformation("Connection reseted successfully");
        }

        private static IEnumerable<MailboxAddress> ParseRecipient(string to)
        {
            string recipientsWithoutSpaces = to.Replace(" ", string.Empty);
            string[] recipients = recipientsWithoutSpaces.Split(";", StringSplitOptions.RemoveEmptyEntries);
            List<MailboxAddress> emailAddressList = [];
            foreach(string email in recipients)
            {
                try
                {
                    emailAddressList.Add(MailboxAddress.Parse(email));
                }
                catch
                {
                    //_logger.LogWarning(ex, "Invalid e-mail address: {email}", email);
                    throw;
                }
            }
            return emailAddressList;
        }

    }
}
