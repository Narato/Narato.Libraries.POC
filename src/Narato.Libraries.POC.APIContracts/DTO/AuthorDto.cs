using System;
using System.Collections.Generic;

namespace Narato.Libraries.POC.APIContracts.DTO
{
    public class AuthorDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public ICollection<BookDto> Books { get; set; }
        public int BooksWritten { get; set; }
    }
}
