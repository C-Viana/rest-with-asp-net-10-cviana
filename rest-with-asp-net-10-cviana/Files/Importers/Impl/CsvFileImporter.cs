using CsvHelper;
using CsvHelper.Configuration;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Files.Importers.Contract;
using System.Globalization;

namespace rest_with_asp_net_10_cviana.Files.Importers.Impl
{
    public class CsvFileImporter : IFileImporter
    {
        public async Task<List<PersonDTO>> ImportFileAsync(Stream fileStream)
        {
            using var reader = new StreamReader(fileStream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                IgnoreBlankLines = true
            });

            List<PersonDTO> people = [];
            await foreach (var record in csv.GetRecordsAsync<dynamic>())
            {
                PersonDTO person = new()
                {
                    FirstName = record.first_name,
                    LastName = record.last_name,
                    Address = record.address,
                    Gender = record.gender,
                    Enabled = true
                };
                people.Add(person);
            }

            return people;
        }

    }
}
