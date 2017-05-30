using System;
using System.Collections.Generic;

namespace POC.Domain.Models
{
    public class Author
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
