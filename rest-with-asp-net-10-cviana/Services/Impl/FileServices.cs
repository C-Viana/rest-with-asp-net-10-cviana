using Microsoft.IdentityModel.Tokens;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using System.Text.Encodings.Web;
using System.Xml.Linq;

namespace rest_with_asp_net_10_cviana.Services.Impl
{
    public class FileServices : IFileServices
    {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _context;
        private static readonly HashSet<string> _allowedExtensions = new() { ".mp3", ".docx", ".txt", ".pdf", ".png", ".jpg", ".jpeg", ".gif"};

        public FileServices(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor;
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadDir");
            if(!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public byte[] GetFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException("File name is either null or empty. A file name must be specified");
            string filePath = Path.Combine(_basePath, fileName);
            if (!File.Exists(filePath)) return null;
            return File.ReadAllBytes(filePath);
        }

        public async Task<FileDetailDTO> SaveFileToDisk(IFormFile file)
        {
            if (file == null || file.Length == 0) throw new InvalidDataException("File is either missing or corrupted");
            string currentFileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(currentFileExtension)) throw new BadHttpRequestException($"Extension [{currentFileExtension}] is not supported. Must use one of the following extension {_allowedExtensions.ToString()}");

            string docName = Path.GetFileName(file.FileName);
            string encodedDocName = Uri.EscapeDataString(docName);
            string destinationPath = Path.Combine(_basePath, docName);
            string baseUrl = $"{_context.HttpContext.Request.Scheme}://{_context.HttpContext.Request.Host}";

            FileDetailDTO fileDetailDTO = new()
            {
                DocumentName = docName,
                DocumentType = file.ContentType,
                DocumentUrl = $"{baseUrl}/api/file/v1/download/{encodedDocName}"
            };

            using FileStream stream = new FileStream(destinationPath, FileMode.Create);
            await file.CopyToAsync(stream);
            return fileDetailDTO;
        }

        public async Task<List<FileDetailDTO>> SaveFilesToDisk(List<IFormFile> files)
        {
            List<FileDetailDTO> savedFiles = [];
            foreach (var item in files)
            {
                savedFiles.Add(SaveFileToDisk(item).Result);
            }
            return savedFiles;
        }

    }
}
