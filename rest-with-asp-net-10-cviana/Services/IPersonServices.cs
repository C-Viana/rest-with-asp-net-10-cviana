using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Hypermedia.Utils;

namespace rest_with_asp_net_10_cviana.Services
{
    public interface IPersonServices
    {
        PersonDTO Create(PersonDTO person);
        PersonDTO Update(PersonDTO person);
        void Delete(long id);
        List<PersonDTO> FindAll();
        PersonDTO FindById(long id);
        PersonDTO Disable(long id);
        PersonDTO Enable(long id);
        List<PersonDTO> FindByFirstName(string firstName, string lastName);
        PagedSearchDto<PersonDTO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);
        Task<List<PersonDTO>> BulkCreationAsync(IFormFile file);
        FileContentResult ExportPage(string name, string sortDirection, int pageSize, int page, string acceptHeader);
    }
}
