using Narato.Libraries.POC.Domain.Managers.Interfaces;
using System.Collections.Generic;
using Narato.ResponseMiddleware.Models.Models;
using System.Threading.Tasks;
using Narato.Libraries.POC.Domain.Contracts.DataProviders;
using System;
using Narato.Libraries.POC.Domain.Models;
using Narato.ResponseMiddleware.Models.Exceptions;

namespace Narato.Libraries.POC.Domain.Managers
{
    public class BookManager : IBookManager
    {
        private readonly IBookDataProvider _bookDataProvider;

        public BookManager(IBookDataProvider bookDataProvider)
        {
            _bookDataProvider = bookDataProvider;
        }

        public async Task<Paged<Book>> GetAllBooksAsync(int page = 1, int pagesize = 10)
        {
            var count = await _bookDataProvider.CountAllAsync();
            var books = await _bookDataProvider.GetAllAsync(page, pagesize);
            return new Paged<Book>(books, page, pagesize, count);
        }

        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            return await _bookDataProvider.GetByIdAsync(id);
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            return await _bookDataProvider.CreateAsync(book);
        }

        public async Task<Book> UpdateBookAsync(Guid id, Book book)
        {
            var validationMessages = new ModelValidationDictionary<string>();
            if (id != book.Id)
                validationMessages.Add("", "Id in book is not the same as the id in url.");

            // do some other validations

            if (validationMessages.Count > 0)
                throw new ValidationException<string>(validationMessages);

            return await _bookDataProvider.UpdateAsync(book);
        }

        public async Task DeleteBookAsync(Guid id)
        {
            var book = await _bookDataProvider.GetByIdAsync(id);
            await _bookDataProvider.DeleteAsync(book);
        }
    }
}
