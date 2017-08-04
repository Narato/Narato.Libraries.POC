using AutoMapper;

namespace Narato.Libraries.POC.DataProvider.Mappers
{
    public class DataProviderAutoMapperProfileConfiguration : Profile
    {
        public DataProviderAutoMapperProfileConfiguration()
        {
#if (EnableExample)
            CreateMap<Models.Book, Domain.Models.Book>().PreserveReferences();

            CreateMap<Models.Author, Domain.Models.Author>().PreserveReferences();
#endif
        }
    }
}
