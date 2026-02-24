using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace rest_with_asp_net_10_cviana.Data.DTO.V1
{
    [XmlRoot("File")]
    public class FileUploadDTO
    {
        public FileUploadDTO()
        {
        }

        public FileUploadDTO(IFormFile file)
        {
            File = file;
        }

        [Required]
        [XmlIgnore]
        public IFormFile File  { get; set; }

    }
}
