using Microsoft.EntityFrameworkCore;
using TheProject.Domain.Entities;

namespace TheProject.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        //criando tabelas referenciando Categories Products e Users:)
        public DbSet<Categories> Categories { get; set; }

        public DbSet<Products> Products { get; set; }
        
        public DbSet<Users> Users { get; set; }
    }
}