namespace rest_with_asp_net_10_cviana.Mail.Settings
{
    public class EmailSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool Ssl { get; set; }

        public MailSettings Properties { get; set; } = new MailSettings();
    }
}
