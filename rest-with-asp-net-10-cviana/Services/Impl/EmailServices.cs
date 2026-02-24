using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http.HttpResults;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Mail;

namespace rest_with_asp_net_10_cviana.Services.Impl
{
    public class EmailServices(EmailSender emailSender) : IEmailServices
    {
        private readonly EmailSender _emailSender = emailSender;

        public void SendBasicEmail(EmailRequestDTO emailRequestDTO)
        {
            _emailSender
                .To(emailRequestDTO.To)
                .WithSubject(emailRequestDTO.Subject)
                .WithMessage(emailRequestDTO.Body)
                .Send();
        }

        public async Task SendEmailWithAttachment(EmailRequestDTO emailRequestDTO, IFormFile attachment)
        {
            if(attachment  == null || attachment.Length == 0)
            {
                throw new BadHttpRequestException("Error when setting attachment. File is either null or missing");
            }
            if (emailRequestDTO == null)
            {
                throw new BadHttpRequestException("E-mail request has no metadata");
            }
            string tempFilePath = Path.Combine(Path.GetTempPath(), attachment.FileName);
            try
            {
                await using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await attachment.CopyToAsync(stream);
                }
                _emailSender
                    .To(emailRequestDTO.To)
                    .WithSubject(emailRequestDTO.Subject)
                    .WithMessage(emailRequestDTO.Body)
                    .Attachment(tempFilePath)
                    .Send();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if(File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
            }
        }

    }
}
