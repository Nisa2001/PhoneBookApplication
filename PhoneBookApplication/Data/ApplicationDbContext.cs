using Microsoft.EntityFrameworkCore;
using PhoneBookApplication.Models;

namespace PhoneBookApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {


        }
        public DbSet<Contact>Contacts  { get; set; }
      
    

    }
}

