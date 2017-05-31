using Narato.Libraries.POC.Domain.Managers.Interfaces;
using System.Collections.Generic;
using Narato.ResponseMiddleware.Models.Models;
using AutoMapper;
using System.Threading.Tasks;
using Narato.Libraries.POC.Domain.Contracts.DataProviders;
using Narato.Libraries.POC.APIContracts.DTO;
using System;
using Narato.Libraries.POC.Domain.Models;
using Narato.ResponseMiddleware.Models.Exceptions;

namespace Narato.Libraries.POC.Domain.Managers
{
    public class BookManager : IBookManager
    {
        private readonly IBookDataProvider _bookDataProvider;
        private readonly IMapper _mapper;

        public BookManager(IBookDataProvider bookDataProvider, IMapper mapper)
        {
            _bookDataProvider = bookDataProvider;
            _mapper = mapper;
        }

        public async Task<Paged<BookDto>> GetAllBooksAsync(int page = 1, int pagesize = 10)
        {
            var count = await _bookDataProvider.CountAllAsync();
            var books = await _bookDataProvider.GetAllAsync(page, pagesize);
            var mappedBooks = _mapper.Map<IEnumerable<BookDto>>(books);
            return new Paged<BookDto>(mappedBooks, page, pagesize, count);
        }

        public async Task<BookDto> GetBookByIdAsync(Guid id)
        {
            var book = await _bookDataProvider.GetByIdAsync(id);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> CreateBookAsync(BookDto book)
        {
            var mappedBook = _mapper.Map<Book>(book);
            var createdBook = await _bookDataProvider.CreateAsync(mappedBook);
            return _mapper.Map<BookDto>(createdBook);
        }

        public async Task<BookDto> UpdateBookAsync(Guid id, BookDto book)
        {
            var validationMessages = new ModelValidationDictionary<string>();
            if (id != book.Id)
                validationMessages.Add("", "Id in book is not the same as the id in url.");

            // do some other validations

            if (validationMessages.Count > 0)
                throw new ValidationException<string>(validationMessages);

            var mappedBook = _mapper.Map<Book>(book);
            var updatedBook = await _bookDataProvider.UpdateAsync(mappedBook);
            return _mapper.Map<BookDto>(updatedBook);
        }

        public async Task DeleteBookAsync(Guid id)
        {
            var book = await _bookDataProvider.GetByIdAsync(id);
            await _bookDataProvider.DeleteAsync(book);
        }
    }
}
