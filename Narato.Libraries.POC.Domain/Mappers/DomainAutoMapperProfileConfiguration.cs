using AutoMapper;
using Narato.Libraries.POC.APIContracts.DTO;
using Narato.Libraries.POC.Domain.Models;

namespace Narato.Libraries.POC.Domain.Mappers
{
    public class DomainAutoMapperProfileConfiguration : Profile
    {
        public DomainAutoMapperProfileConfiguration()
        {
            CreateMap<Book, BookDto>()
                .PreserveReferences()
                .ReverseMap()
                .PreserveReferences();

            CreateMap<Author, AuthorDto>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(d => d.BooksWritten, opt => opt.MapFrom(src => src.Books.Count))
                .PreserveReferences()
                .ReverseMap()
                .PreserveReferences();
        }
    }
}
