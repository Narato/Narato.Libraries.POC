using System;

namespace Narato.Libraries.POC.APIContracts.DTO
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public AuthorDto Author { get; set; }
    }
}
