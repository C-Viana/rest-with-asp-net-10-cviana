using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace rest_with_asp_net_10_cviana.Data.DTO.V1
{
    [XmlRoot("Files")]
    public class MultipleFilesUploadDTO
    {
        public MultipleFilesUploadDTO() { }

        public MultipleFilesUploadDTO(List<IFormFile> files)
        {
            Files = files;
        }

        [Required]
        public List<IFormFile> Files { get; set; }
    }
}
