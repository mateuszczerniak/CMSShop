using System.Data.Entity;

namespace CMSShop.Models.Data
{
    public class Db : DbContext
    {
        public DbSet<PageDTO> Pages { get; set; }
    }
}