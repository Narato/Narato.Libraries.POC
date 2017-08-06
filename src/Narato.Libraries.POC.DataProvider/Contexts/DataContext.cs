using Microsoft.EntityFrameworkCore;

namespace Narato.Libraries.POC.DataProvider.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
    }
}
