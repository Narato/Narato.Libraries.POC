using AutoMapper;
#if (EnableExample)
using Narato.Libraries.POC.APIContracts.DTO;
using Narato.Libraries.POC.Domain.Models;
#endif

namespace Narato.Libraries.POC.API.Mappers
{
    public class DTOAutoMapperProfileConfiguration : Profile
    {
        public DTOAutoMapperProfileConfiguration()
        {
#if (EnableExample)
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
#endif
        }
    }
}
