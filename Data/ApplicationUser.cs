using Microsoft.AspNetCore.Identity;

namespace EasyGames.Data;

// Extend Identity user to store more fields later (e.g., FullName)
public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
}
