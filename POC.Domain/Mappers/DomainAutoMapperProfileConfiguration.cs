using AutoMapper;
using POC.APIContracts.DTO;
using POC.Domain.Models;

namespace POC.Domain.Mappers
{
    public class DomainAutoMapperProfileConfiguration : Profile
    {
        public DomainAutoMapperProfileConfiguration()
        {
            CreateMap<Book, BookDto>()
                .PreserveReferences();

            CreateMap<Author, AuthorDto>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(d => d.BooksWritten, opt => opt.MapFrom(src => src.Books.Count))
                .PreserveReferences();
        }
    }
}
