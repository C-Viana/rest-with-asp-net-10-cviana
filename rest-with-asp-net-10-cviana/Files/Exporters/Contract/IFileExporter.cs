using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net_10_cviana.Data.DTO.V1;

namespace rest_with_asp_net_10_cviana.Files.Exporters.Contract
{
    public interface IFileExporter
    {
        FileContentResult ExportFile(List<PersonDTO> listOfPeople);
    }
}
