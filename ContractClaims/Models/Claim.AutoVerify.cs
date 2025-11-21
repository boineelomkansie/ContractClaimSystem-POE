// Models/Claim.AutoVerify.cs
namespace ContractClaims.Models
{
    public partial class Claim
    {
        // Centralized automation rule — adjust thresholds as required.
        public bool MeetsAutoVerifyRules()
        {
            // Example rules: max 160 hours per month; hourly rate <= 1000
            return Hours <= 160 && HourlyRate <= 1000m;
        }
    }
}
