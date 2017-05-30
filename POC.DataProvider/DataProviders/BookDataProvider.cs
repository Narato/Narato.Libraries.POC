using System.Collections.Generic;
using System.Threading.Tasks;
using POC.Domain.Contracts.DataProviders;
using AutoMapper;
using POC.Domain.Models;
using System;

namespace POC.DataProvider.DataProviders
{
    public class BookDataProvider : IBookDataProvider
    {
        private readonly IMapper _mapper;

        public BookDataProvider(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<int> CountAllAsync()
        {
            return await Task.FromResult(100);
        }

        public async Task<IEnumerable<Book>> GetAllAsync(int page = 1, int pagesize = 10)
        {
            var books = new List<POC.DataProvider.Models.Book>();

            for (int i = 0; i < pagesize; i++)
            {
                books.Add(await CreateBookMock());
            }
            return _mapper.Map<IEnumerable<Book>>(books);
        }

        public async Task<Book> GetByIdAsync(Guid id)
        {
            return _mapper.Map<Book>(await CreateBookMock(id));
        }

        public async Task<Book> Create(Book book)
        {
            // actually save it
            if (book.Id == Guid.Empty)
                book.Id = Guid.NewGuid();
            if (book.Author != null && book.Author.Id == Guid.Empty)
                book.Author.Id = Guid.NewGuid();
            return await Task.FromResult(book);
        }

        public async Task<Book> Update(Book book)
        {
            // actually update it
            return await Task.FromResult(book);
        }

        // mocking stuff
        private async Task<POC.DataProvider.Models.Book> CreateBookMock(Guid id = new Guid())
        {
            var book = new POC.DataProvider.Models.Book()
            {
                Id = id == Guid.Empty ? Guid.NewGuid() : id,
                Summary = "There once was ...",
                Title = "TitleTest"
            };

            var author = new POC.DataProvider.Models.Author()
            {
                Id = Guid.NewGuid(),
                FirstName = "TestFirst",
                LastName = "TestLast",
                Books = new List<POC.DataProvider.Models.Book>() { book }
            };
            book.Author = author;

            return await Task.FromResult(book);
        }
    }
}
