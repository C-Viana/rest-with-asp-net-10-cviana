using rest_with_asp_net_10_cviana.Data.DTO.V1;

namespace rest_with_asp_net_10_cviana.Files.Importers.Contract
{
    public interface IFileImporter
    {
        Task<List<PersonDTO>> ImportFileAsync(Stream fileStream);
    }
}
