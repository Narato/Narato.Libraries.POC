using Narato.Libraries.POC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Narato.Libraries.POC.Domain.Contracts.DataProviders
{
    public interface IBookDataProvider
    {
        Task<IEnumerable<Book>> GetAllAsync(int page = 1, int pagesize = 10);
        Task<int> CountAllAsync();
        Task<Book> GetByIdAsync(Guid id);
        Task<Book> CreateAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task DeleteAsync(Book book);
    }
}
