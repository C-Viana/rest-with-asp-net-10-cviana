using rest_with_asp_net_10_cviana.Data.DTO.V1;

namespace rest_with_asp_net_10_cviana.Services
{
    public interface IEmailServices
    {
        void SendBasicEmail(EmailRequestDTO emailRequestDTO);
        Task SendEmailWithAttachment(EmailRequestDTO emailRequestDTO, IFormFile attachment);
    }
}
