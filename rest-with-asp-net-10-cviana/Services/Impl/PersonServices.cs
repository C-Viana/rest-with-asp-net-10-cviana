using Mapster;
using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net_10_cviana.Data.Converter.Impl;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Files.Exporters.Factory;
using rest_with_asp_net_10_cviana.Files.Importers.Factory;
using rest_with_asp_net_10_cviana.Hypermedia.Utils;
using rest_with_asp_net_10_cviana.Models;
using rest_with_asp_net_10_cviana.Repositories.PersonRepository;

namespace rest_with_asp_net_10_cviana.Services.Impl
{
    public class PersonServices : IPersonServices
    {
        private readonly IPersonRepository _repository;
        private readonly PersonConverter _converter;
        private readonly FileImporterFactory _fileImporterFactory;
        private readonly FileExporterFactory _fileExporterFactory;
        private readonly ILogger<PersonServices> _logger;

        public PersonServices(IPersonRepository repository, FileImporterFactory fileImporterFactory, FileExporterFactory fileExporterFactory, ILogger<PersonServices> logger)
        {
            _repository = repository;
            _converter = new PersonConverter();
            _fileImporterFactory = fileImporterFactory;
            _fileExporterFactory = fileExporterFactory;
            _logger = logger;
        }

        PersonDTO IPersonServices.Create(PersonDTO person)
        {
            Person personEntity = _converter.Parse(person);
            return _converter.Parse(_repository.Create(personEntity));
        }

        List<PersonDTO> IPersonServices.FindAll()
        {
            return _converter.ParseList(_repository.FindAll());
        }

        PersonDTO IPersonServices.FindById(long id)
        {
            return _converter.Parse(_repository.FindById(id));
        }

        PersonDTO IPersonServices.Update(PersonDTO person)
        {
            Person personEntity = _converter.Parse(person);
            return _converter.Parse(_repository.Update(personEntity));
        }

        void IPersonServices.Delete(long id)
        {
            _repository.Delete(id);
        }

        public PersonDTO Disable(long id)
        {
            return _converter.Parse(_repository.Disable(id));
        }

        public PersonDTO Enable(long id)
        {
            return _converter.Parse(_repository.Enable(id));
        }

        public List<PersonDTO> FindByFirstName(string firstName, string lastName)
        {
            return _converter.ParseList(_repository.FindByFirstName(firstName, lastName));
        }

        public PagedSearchDto<PersonDTO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var pagedResult = _repository.FindWithPagedSearch(name, sortDirection, pageSize, page);
            return pagedResult.Adapt<PagedSearchDto<PersonDTO>>();
        }

        public async Task<List<PersonDTO>> BulkCreationAsync(IFormFile file)
        {
            _logger.LogInformation("Requiring import for file {file}", file.FileName);
            if (file == null || file.Length == 0) throw new ArgumentException("File is either null or empty");

            using var stream = file.OpenReadStream();
            var fileName = file.FileName;

            var importer = _fileImporterFactory.GetImporter(fileName);
            var people = await importer.ImportFileAsync(stream);

            var entities = people.Select(dto => _repository.Create(dto.Adapt<Person>())).ToList();

            _logger.LogInformation("Import finished successfully for file {file}", file.FileName);

            return entities.Adapt<List<PersonDTO>>();
        }

        public FileContentResult ExportPage(string name, string sortDirection, int pageSize, int page, string acceptHeader)
        {
            _logger.LogInformation("Requiring export of a {acceptHeader} file ", acceptHeader);
            var pagedResult = FindWithPagedSearch(name, sortDirection, pageSize, page);
            try
            {
                var people = pagedResult.Items.Adapt<List<PersonDTO>>();
                var exporter = _fileExporterFactory.GetExporter(acceptHeader);
                _logger.LogInformation("File successfully created for export");
                return exporter.ExportFile(people);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when creating export file");
                throw;
            }
        }
    }
}
