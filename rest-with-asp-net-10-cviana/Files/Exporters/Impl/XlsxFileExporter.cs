using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Files.Exporters.Contract;
using rest_with_asp_net_10_cviana.Files.Importers.Factory;

namespace rest_with_asp_net_10_cviana.Files.Exporters.Impl
{
    public class XlsxFileExporter : IFileExporter
    {
        public FileContentResult ExportFile(List<PersonDTO> listOfPeople)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("PEOPLE");

            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "First name";
            worksheet.Cell(1, 3).Value = "Last name";
            worksheet.Cell(1, 4).Value = "Address";
            worksheet.Cell(1, 5).Value = "Gender";
            worksheet.Cell(1, 6).Value = "Enabled";

            var headerRange = worksheet.Range(1, 1, 1, 6);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;
            foreach (PersonDTO person in listOfPeople)
            {
                worksheet.Cell(row, 1).Value = person.Id;
                worksheet.Cell(row, 2).Value = person.FirstName;
                worksheet.Cell(row, 3).Value = person.LastName;
                worksheet.Cell(row, 4).Value = person.Address;
                worksheet.Cell(row, 5).Value = person.Gender;
                worksheet.Cell(row, 6).Value = person.Enabled == true ? "yes" : "no";
                row++;
            }

            worksheet.Columns().AdjustToContents();
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            var fileBytes = stream.ToArray();

            return new FileContentResult(fileBytes, FileMediaTypes.XLSX)
            {
                FileDownloadName = $"people_excel_export_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx"
            };
        }

    }
}
