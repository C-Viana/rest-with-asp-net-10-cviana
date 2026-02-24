using rest_with_asp_net_10_cviana.Data.DTO.V1;

namespace rest_with_asp_net_10_cviana.Services
{
    public interface IFileServices
    {
        byte[] GetFile(string fileName);
        Task<FileDetailDTO> SaveFileToDisk(IFormFile file);
        Task<List<FileDetailDTO>> SaveFilesToDisk(List<IFormFile> files);
    }
}
