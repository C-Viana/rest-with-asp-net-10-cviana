using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Models;
using rest_with_asp_net_10_cviana.Services;

namespace rest_with_asp_net_10_cviana.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]/v1")]
    [Authorize("Bearer")]
    public class BookController(IBookServices service, ILogger<BookController> logger) : ControllerBase
    {
        private readonly IBookServices _service = service;
        private readonly ILogger<BookController> _logger = logger;

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Create([FromBody] BookDTO NewBook)
        {
            _logger.LogInformation("Saving book '{title}' into the database", NewBook.Title);
            BookDTO SavedBook = _service.Create(NewBook);
            if (SavedBook == null)
            {
                _logger.LogError("Creation of book '{title}' failed", NewBook.Title);
            }
            return CreatedAtAction(nameof(FindBook), new { id = SavedBook.Id }, SavedBook);
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult EditBook([FromBody] BookDTO EditReqBook)
        {
            _logger.LogInformation("Updating book '{title}' in the database", EditReqBook.Title);
            BookDTO UpdatedBook = _service.Update(EditReqBook);
            if (UpdatedBook == null)
            {
                _logger.LogWarning("Book '{title}' could not be updated", EditReqBook.Title);
                return NotFound();
            }
            return Ok(UpdatedBook);
        }

        [HttpGet("{id}", Name = "FindBook")]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult FindBook(long id)
        {
            _logger.LogInformation("Fetching book '{id}' in the database", id);
            BookDTO FoundBook = _service.FindById(id);
            if (FoundBook == null)
            {
                _logger.LogWarning("No book found with '{id}'", id);
                return NotFound();
            }
            return Ok(FoundBook);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<BookDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult FindAllBooks()
        {
            _logger.LogInformation("Retrieving all books from database");
            return Ok(_service.FindAllBooks());
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult DeleteBook(long id)
        {
            _logger.LogInformation("Deleting book '{id}' from database", id);
            _service.Delete(id);
            return NoContent();
        }

        [HttpGet("title/{sortDirection}/{pageSize}/{page}")]
        [ProducesResponseType(200, Type = typeof(PagedSearch<BookDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult FindBooksByTitle([FromQuery] string title, string sortDirection, int pageSize, int page)
        {
            _logger.LogInformation("Fetching all books from database: paged on page={page}, pageSize={pageSize}, sortDirection={sortDirection}, title={title}",
            page, pageSize, sortDirection, title);

            return Ok(_service.GetPagedSearchBookByTitle(title, sortDirection, pageSize, page));
        }

        [HttpGet("author/{sortDirection}/{pageSize}/{page}")]
        [ProducesResponseType(200, Type = typeof(PagedSearch<BookDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult FindBooksByAuthor([FromQuery] string author, string sortDirection, int pageSize, int page)
        {
            _logger.LogInformation("Fetching all books from database: paged on page={page}, pageSize={pageSize}, sortDirection={sortDirection}, author={author}",
            page, pageSize, sortDirection, author);

            return Ok(_service.GetPagedSearchBookByAuthor(author, sortDirection, pageSize, page));
        }

    }
}
