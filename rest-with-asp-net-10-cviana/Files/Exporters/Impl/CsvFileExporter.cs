using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Files.Exporters.Contract;
using rest_with_asp_net_10_cviana.Files.Importers.Factory;
using System.Globalization;
using System.Text;

namespace rest_with_asp_net_10_cviana.Files.Exporters.Impl
{
    public class CsvFileExporter : IFileExporter
    {
        public FileContentResult ExportFile(List<PersonDTO> listOfPeople)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen:true);
            using var csv = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            });
            csv.WriteRecords(listOfPeople);
            streamWriter.Flush();

            var fileBytes = memoryStream.ToArray();

            return new FileContentResult(fileBytes, FileMediaTypes.CSV)
            {
                FileDownloadName = $"people_csv_export_{DateTime.UtcNow:yyyyMMddHHmmss}.csv"
            };
        }
    }
}
