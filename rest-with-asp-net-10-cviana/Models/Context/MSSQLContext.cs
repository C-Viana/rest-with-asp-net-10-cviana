using Microsoft.EntityFrameworkCore;

namespace rest_with_asp_net_10_cviana.Models.Context
{
    public class MSSQLContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
