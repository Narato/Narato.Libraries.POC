using System;
using System.Collections.Generic;

namespace POC.DataProvider.Models
{
    public class Author
    {
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
