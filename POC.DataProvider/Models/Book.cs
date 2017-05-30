using System;

namespace POC.DataProvider.Models
{
    public class Book
    {
        public virtual Guid Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Summary { get; set; }
        public virtual Author Author { get; set; }
    }
}
