using System.Xml.Serialization;

namespace rest_with_asp_net_10_cviana.Data.DTO.V1
{
    [XmlRoot("FileDetails")]
    public class FileDetailDTO
    {
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public string DocumentUrl { get; set; }

        public FileDetailDTO() { }

        public FileDetailDTO(string documentName, string documentType, string documentUrl)
        {
            DocumentName = documentName;
            DocumentType = documentType;
            DocumentUrl = documentUrl;
        }
    }
}
