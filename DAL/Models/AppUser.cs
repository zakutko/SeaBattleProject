using Microsoft.AspNetCore.Identity;
namespace DAL.Models
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public bool? IsHit { get; set; }
    }
}
