// Data/ApplicationDbContext.cs
using ContractClaims.Models;
using Microsoft.EntityFrameworkCore;

namespace ContractClaims.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Lecturer> Lecturers { get; set; } = null!;
        public DbSet<Contract> Contracts { get; set; } = null!;
        public DbSet<Claim> Claims { get; set; } = null!;
        public DbSet<Document> Documents { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed minimal sample data for demonstration:
            builder.Entity<Lecturer>().HasData(new Lecturer { Id = 1, EmployeeNumber = "EMP001", FullName = "Alice Mphahlele", Email = "alice@example.com" });
            builder.Entity<Contract>().HasData(new Contract { Id = 1, ContractCode = "C-101", Description = "Part-time Lecturing", LecturerId = 1 });
        }
    }
}
