// Tests/ClaimTests.cs
using ContractClaims.Data;
using ContractClaims.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace ContractClaims.Tests
{
    public class ClaimTests
    {
        [Fact]
        public void Total_Is_Hours_Times_Rate()
        {
            var c = new Claim { Hours = 10.5, HourlyRate = 200m };
            Assert.Equal(200m * (decimal)10.5, c.Total);
        }

        [Fact]
        public void MeetsAutoVerifyRules_WhenWithinLimits_ReturnsTrue()
        {
            var c = new Claim { Hours = 100, HourlyRate = 500m };
            Assert.True(c.MeetsAutoVerifyRules());
        }

        [Fact]
        public async Task Submit_AutoVerifies_WhenRulesMet()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_AutoVerify")
                .Options;

            await using var db = new ApplicationDbContext(options);
            // seed lecturer and contract
            db.Lecturers.Add(new Lecturer { Id = 1, EmployeeNumber = "EMP1", FullName = "Test User" });
            db.Contracts.Add(new Contract { Id = 1, ContractCode = "C1" });
            await db.SaveChangesAsync();

            var claim = new Claim
            {
                LecturerId = 1,
                ContractId = 1,
                Month = "2025-11",
                Hours = 10,
                HourlyRate = 200m
            };

            db.Claims.Add(claim);
            await db.SaveChangesAsync();

            // check MeetsAutoVerifyRules works
            Assert.True(claim.MeetsAutoVerifyRules());
            // simulate controller auto-verify action
            if (claim.MeetsAutoVerifyRules())
            {
                claim.Status = ClaimStatus.Verified;
                await db.SaveChangesAsync();
            }

            var fromDb = await db.Claims.FindAsync(claim.Id);
            Assert.Equal(ClaimStatus.Verified, fromDb.Status);
        }
    }
}
