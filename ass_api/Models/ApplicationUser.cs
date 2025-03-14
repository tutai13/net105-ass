using Microsoft.AspNetCore.Identity;

namespace ass_api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
