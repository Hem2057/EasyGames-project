using Microsoft.AspNetCore.Identity;

namespace EasyGames.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Full name used on registration and profile
        public string? FullName { get; set; }

        // Additional fields for PDF requirements
        public string Tier { get; set; } = "Bronze";
        public decimal LifetimeProfitContribution { get; set; }
    }
}
