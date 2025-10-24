using Microsoft.EntityFrameworkCore;
using ContractClaimSystem.Models;

namespace ContractClaimSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Lecturers
            modelBuilder.Entity<Lecturer>().HasData(
                new Lecturer { LecturerId = 1, FirstName = "Boineelo", LastName = "Mkansie" },
                new Lecturer { LecturerId = 2, FirstName = "Thabo", LastName = "Dlamini" }
            );

            // Seed Contracts
            modelBuilder.Entity<Contract>().HasData(
                new Contract { ContractId = 1, LecturerId = 1, Title = "HCIN6222 Contract", Description = "HCI Module Contract" },
                new Contract { ContractId = 2, LecturerId = 2, Title = "PROG6221 Contract", Description = "Programming Module Contract" }
            );

            // Relationships
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Lecturer)
                .WithMany(l => l.Contracts)
                .HasForeignKey(c => c.LecturerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Claim>()
                .HasOne(cl => cl.Lecturer)
                .WithMany(l => l.Claims)
                .HasForeignKey(cl => cl.LecturerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Claim>()
                .HasOne(cl => cl.Contract)
                .WithMany(c => c.Claims)
                .HasForeignKey(cl => cl.ContractId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.Claim)
                .WithMany(cl => cl.Documents)
                .HasForeignKey(d => d.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);

            // Decimal precision
            modelBuilder.Entity<Claim>()
                .Property(c => c.HourlyRate).HasPrecision(18, 2);
            modelBuilder.Entity<Claim>()
                .Property(c => c.HoursWorked).HasPrecision(18, 2);
        }
    }
}
