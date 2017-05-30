using System;

namespace POC.Domain.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public Author Author { get; set; }
    }
}
