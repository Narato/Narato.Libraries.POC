using Narato.Libraries.POC.Domain.Models;
using Narato.ResponseMiddleware.Models.Models;
using System;
using System.Threading.Tasks;

namespace Narato.Libraries.POC.Domain.Managers.Interfaces
{
    public interface IBookManager
    {
        Task<Paged<Book>> GetAllBooksAsync(int page = 1, int pagesize = 10);
        Task<Book> GetBookByIdAsync(Guid id);
        Task<Book> CreateBookAsync(Book book);
        Task<Book> UpdateBookAsync(Guid id, Book book);
        Task DeleteBookAsync(Guid id);
    }
}
