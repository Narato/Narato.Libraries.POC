using AutoMapper;

namespace POC.DataProvider.Mappers
{
    public class DataProviderAutoMapperProfileConfiguration : Profile
    {
        public DataProviderAutoMapperProfileConfiguration()
        {
            CreateMap<Models.Book, Domain.Models.Book>().PreserveReferences();

            CreateMap<Models.Author, Domain.Models.Author>().PreserveReferences();
        }
    }
}
