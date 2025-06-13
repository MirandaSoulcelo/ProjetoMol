using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TheProject.Infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Caminho para o appsettings.json 
            var basePath = Directory.GetCurrentDirectory();
            
            // Se estiver rodando de WebApi, volto uma pasta
            if (!File.Exists(Path.Combine(basePath, "appsettings.json")))
            {
                basePath = Path.Combine(basePath, "../TheProject.WebApi");
            }

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite(connectionString); // ou UseSqlServer

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
