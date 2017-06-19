using Microsoft.AspNetCore.Mvc;
using Narato.ResponseMiddleware.Models.Models;
using Narato.Libraries.POC.APIContracts.DTO;
using Narato.Libraries.POC.Domain.Managers.Interfaces;
using System;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;
using Narato.Libraries.POC.Domain.Models;

namespace Narato.Libraries.POC.API.Controllers
{
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly IBookManager _bookManager;
        private readonly IMapper _mapper;

        public BooksController(IBookManager bookManager, IMapper mapper)
        {
            _bookManager = bookManager;
            _mapper = mapper;
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
            var pagedBooks = await _bookManager.GetAllBooksAsync(page, pagesize);
            var mappedPagedBooks = _mapper.Map<IEnumerable<BookDto>>(pagedBooks.Items);
            return Ok(new Paged<BookDto>(mappedPagedBooks, page, pagesize, pagedBooks.Total));
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
            var book = await _bookManager.GetBookByIdAsync(id);
            return Ok(_mapper.Map<BookDto>(book));
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
            var businessBook = _mapper.Map<Book>(book);
            var createdBook = await _bookManager.CreateBookAsync(businessBook);
            return CreatedAtRoute("GetBookById", new { Id = createdBook.Id }, _mapper.Map<BookDto>(createdBook));
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
            var businessBook = _mapper.Map<Book>(book);
            var updatedBook = await _bookManager.UpdateBookAsync(id, businessBook);
            return Ok(_mapper.Map<BookDto>(updatedBook));
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
