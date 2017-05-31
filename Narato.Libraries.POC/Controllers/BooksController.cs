using Microsoft.AspNetCore.Mvc;
using Narato.ResponseMiddleware.Models.Models;
using Narato.Libraries.POC.APIContracts.DTO;
using Narato.Libraries.POC.Domain.Managers.Interfaces;
using System;
using System.Threading.Tasks;

namespace Narato.Libraries.POC.Controllers
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
        /// <returns>the newly created book, or a list of validation errors</returns>
        [ProducesResponseType(typeof(BookDto), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationErrorContent<string>), (int)System.Net.HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorContent), (int)System.Net.HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookDto book)
        {
            var createdBook = await _bookManager.CreateBookAsync(book);
            return CreatedAtRoute("GetBookById", new { Id = createdBook.Id }, createdBook);
        }

        /// <summary>
        /// Updates a book
        /// </summary>
        /// <param name="id">the Id of the book to update</param>
        /// <param name="book">the book to update</param>
        /// <returns>the updated book, or a list of validation errors</returns>
        [ProducesResponseType(typeof(BookDto), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationErrorContent<string>), (int)System.Net.HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorContent), (int)System.Net.HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorContent), (int)System.Net.HttpStatusCode.InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BookDto book)
        {
            return Ok(await _bookManager.UpdateBookAsync(id, book));
        }

        /// <summary>
        /// Deletes the book by given id
        /// </summary>
        /// <param name="id">the id of the book that will get deleted</param>
        /// <returns>204 no content</returns>
        [ProducesResponseType(typeof(void), (int)System.Net.HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorContent), (int)System.Net.HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorContent), (int)System.Net.HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorContent), (int)System.Net.HttpStatusCode.InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _bookManager.DeleteBookAsync(id);
            return NoContent();
        }
    }
}
