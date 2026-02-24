namespace rest_with_asp_net_10_cviana.Data.DTO.V1
{
    public class EmailRequestDTO
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
