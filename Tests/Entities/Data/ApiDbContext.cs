using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities.Data
{
    public class ApiDbContext : DbContext
    {
        public DbSet<ModelExample>? ModelExamples { get; set; }

        public DbSet<Categoria>? Categorias { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }
    }
}