// Tests/ClaimTests.cs
using System.Security.Claims;
using ContractClaims.Models;
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
        public void Default_Status_Is_Submitted()
        {
            var c = new Claim { Hours = 1, HourlyRate = 10m, Month = "2025-11", LecturerId = 1, ContractId = 1 };
            Assert.Equal(ClaimStatus.Submitted, c.Status);
        }
    }
}
