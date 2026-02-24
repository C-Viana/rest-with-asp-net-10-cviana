using ClosedXML.Excel;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Files.Importers.Contract;

namespace rest_with_asp_net_10_cviana.Files.Importers.Impl
{
    public class XlsxFileImporter : IFileImporter
    {
        public Task<List<PersonDTO>> ImportFileAsync(Stream fileStream)
        {
            List<PersonDTO> people = [];
            using XLWorkbook workbook = new(fileStream);
            var sheet = workbook.Worksheets.First();
            var rows = sheet.RowsUsed().Skip(1);

            foreach (var row in rows)
            {
                if(row.Cell(1).IsEmpty()) continue;
                people.Add(new()
                {
                    FirstName = row.Cell(1).GetString(),
                    LastName = row.Cell(2).GetString(),
                    Address = row.Cell(3).GetString(),
                    Gender = row.Cell(4).GetString(),
                    Enabled = true
                });
            }
            return Task.FromResult(people);
        }

    }
}
