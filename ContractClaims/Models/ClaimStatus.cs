// Models/ClaimStatus.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractClaims.Models
{
    public enum ClaimStatus
    {
        Submitted,
        Verified,
        Approved,
        Rejected,
        Paid
    }
}
