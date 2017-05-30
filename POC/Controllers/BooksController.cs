using Microsoft.AspNetCore.Mvc;
using Narato.ResponseMiddleware.Models.Models;
using POC.APIContracts.DTO;
using POC.Domain.Managers.Interfaces;
using System;
using System.Threading.Tasks;

namespace POC.Controllers
{
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly IBookManager _bookManager;

        public BooksController(IBookManager bookManager)
        {
            _bookManager = bookManager;
        }

        /// <summary>
        /// Gets all books (paged)
        /// </summary>
        /// <param name="page">which page to get</param>
        /// <param name="pagesize">how many items per page</param>
        /// <returns>a paged list of books</returns>
        [ProducesResponseType(typeof(Paged<BookDto>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorContent), (int)System.Net.HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorContent), (int)System.Net.HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pagesize = 10)
        {
            return Ok(await _bookManager.GetAllBooksAsync(page, pagesize));
        }

        /// <summary>
        /// Gets a book by its Id
        /// </summary>
        /// <param name="id">the Id of the book</param>
        /// <returns>the Book with the given Id</returns>
        [ProducesResponseType(typeof(BookDto), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorContent), (int)System.Net.HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorContent), (int)System.Net.HttpStatusCode.InternalServerError)]
        [HttpGet("{id}", Name = "GetBookById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _bookManager.GetBookByIdAsync(id));
        }

        /// <summary>
        /// Creates a book
        /// </summary>
        /// <param name="book">the book to create</param>
        /// <returns>the newly created book</returns>
        [ProducesResponseType(typeof(BookDto), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationErrorContent<string>), (int)System.Net.HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorContent), (int)System.Net.HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookDto book)
        {
            var createdBook = await _bookManager.CreateBookAsync(book);
            return CreatedAtRoute("GetBookById", new { Id = createdBook.Id }, createdBook);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BookDto book)
        {
            return Ok(await _bookManager.UpdateBookAsync(id, book));
        }
    }
}
