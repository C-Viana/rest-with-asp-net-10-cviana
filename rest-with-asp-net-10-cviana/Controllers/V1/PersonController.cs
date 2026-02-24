using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Files.Importers.Factory;
using rest_with_asp_net_10_cviana.Models;
using rest_with_asp_net_10_cviana.Services;
using System.Net;

namespace rest_with_asp_net_10_cviana.Controllers.V1;

[ApiController]
[Route("api/[controller]/v1")]
[Authorize("Bearer")]
//[EnableCors("DefaultPolicy")]
public class PersonController : ControllerBase
{
    private readonly IPersonServices _personServices;
    private readonly ILogger<PersonController> _logger;

    public PersonController(IPersonServices personServices, ILogger<PersonController> logger)
    {
        _personServices = personServices;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<PersonDTO>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult FindAllPeople()
    {
        //https://localhost:7244/api/Person/v1
        _logger.LogInformation("Fetching all people from database PERSON");
        return Ok(_personServices.FindAll());
    }

    [HttpGet("{page}/{pageSize}/{sortDirection}")]
    [ProducesResponseType(200, Type = typeof(PagedSearch<PersonDTO>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult FindAllPeople(int page, int pageSize, string sortDirection, [FromQuery] string name)
    {
        //https://localhost:7244/api/Person/v1/1/5/desc?name=mar
        _logger.LogInformation("Fetching all people from database: paged on page={page}, pageSize={pageSize}, sortDirection={sortDirection}, name={name}",
            page, pageSize, sortDirection, name);
        return Ok(_personServices.FindWithPagedSearch(name, sortDirection, pageSize, page));
    }

    [HttpGet("{id}", Name = "FindById")]
    [ProducesResponseType(200, Type = typeof(PersonDTO))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult FindById(long id)
    {
        //https://localhost:7244/api/Person/v1/8
        _logger.LogInformation("Fetching person with ID {id}", id);
        PersonDTO person = _personServices.FindById(id);
        if (person == null) {
            _logger.LogWarning("Person with ID {id} not found", id);
        }
        return Ok(person);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PersonDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult CreatePerson([FromBody] PersonDTO person) {
        //https://localhost:7244/api/Person/v1
        _logger.LogInformation("Creating person with name {firstName}", person.FirstName);
        PersonDTO SavedPerson = _personServices.Create(person);
        if (SavedPerson == null)
        {
            _logger.LogError("Person {name} could not be created", person.FirstName);
        }
        
        return CreatedAtAction(nameof(FindById), new {id = SavedPerson.Id }, SavedPerson);
    }

    [HttpPut]
    [ProducesResponseType(200, Type = typeof(PersonDTO))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult UpdatePerson([FromBody] PersonDTO person)
    {
        //https://localhost:7244/api/Person/v1
        _logger.LogInformation("Updating person with ID {id}", person.Id);
        PersonDTO UpdatedPerson = _personServices.Update(person);
        if (UpdatedPerson == null)
        {
            _logger.LogError("Person {id} could not be updated", person.Id);
            return NotFound();
        }
        return Ok(UpdatedPerson);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult DeletePerson(long id)
    {
        //https://localhost:7244/api/Person/v1
        _logger.LogInformation("Deleting person with ID {id}", id);
        _personServices.Delete(id);
        return NoContent();
    }

    [HttpPatch("disable/{id}")]
    [ProducesResponseType(200, Type = typeof(PersonDTO))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult DisablePerson(long id)
    {
        _logger.LogInformation("Setting person with ID {id} to disabled", id);
        var response = _personServices.Disable(id);
        if(response == null)
        {
            _logger.LogError("No Person with ID {id} was found", id);
            return NotFound();
        }
        return Ok(response);
    }

    [HttpPatch("enable/{id}")]
    [ProducesResponseType(200, Type = typeof(PersonDTO))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult EnablePerson(long id)
    {
        _logger.LogInformation("Setting person with ID {id} to enabled", id);
        var response = _personServices.Enable(id);
        if (response == null)
        {
            _logger.LogError("No Person with ID {id} was found", id);
            return NotFound();
        }
        return Ok(response);
    }

    [HttpGet("fetchname", Name = "FindByName")]
    [ProducesResponseType(200, Type = typeof(List<PersonDTO>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public IActionResult FindByName([FromQuery] string firstName, [FromQuery] string lastName)
    {
        //https://localhost:7244/api/Person/v1?name=reginaldo
        _logger.LogInformation("Fetching person with names \"{firstName}\" and \"{lastName}\"", firstName, lastName);
        List<PersonDTO> filteredPeople = _personServices.FindByFirstName(firstName, lastName);
        if (filteredPeople == null || filteredPeople.Count == 0)
        {
            _logger.LogWarning("Person with names {firstName} and {lastName} not found", firstName, lastName);
        }
        return Ok(filteredPeople);
    }

    [HttpPost("import")]
    [ProducesResponseType(200, Type = typeof(List<PersonDTO>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> BulkCreation([FromForm] FileUploadDTO file)
    {
        if(file.File == null || file.File.Length == 0)
        {
            _logger.LogError("File is either missing or empty");
            return BadRequest("File is either missing or empty");
        }
        var people = await _personServices.BulkCreationAsync(file.File);
        _logger.LogInformation("Creating registers into person table using file {fileName}", file.File.FileName);
        if (people == null)
        {
            _logger.LogError("Bulk creation from file {fileName} returned unsuccessfully", file.File.FileName);
            return UnprocessableEntity("Bulk creation failed. Please confirm that the content meets the requirement or try again later.");
        }
        _logger.LogInformation("Bulk creating ended successfully for file {fileName}. Total of {count} people were created", file.File.FileName, people.Count);

        return Ok(people);
    }

    [HttpPost("export/{page}/{pageSize}/{sortDirection}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(415)]
    [ProducesResponseType(500)]
    [Produces(FileMediaTypes.CSV, FileMediaTypes.XLSX)]
    public IActionResult BulkExtraction(int page, int pageSize, string sortDirection, [FromQuery] string name = "")
    {
        //https://localhost:7244/api/Person/v1/export/1/5/desc?name=mar
        var acceptHeader = Request.Headers.Accept;
        if (string.IsNullOrEmpty(acceptHeader))
        {
            return BadRequest("Accept header is required");
        }
        _logger.LogInformation("Creating file for exportation with required type {acceptHeader}", acceptHeader);
        try { 
            return _personServices.ExportPage(name, sortDirection, pageSize, page, acceptHeader); 
        }
        catch (NotSupportedException ex)
        {
            _logger.LogError(ex, "Unsupported file format: {acceptHeader}", acceptHeader);
            return StatusCode(StatusCodes.Status415UnsupportedMediaType, ex.Message);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating export file");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

}
