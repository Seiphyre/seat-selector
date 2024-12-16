using Microsoft.AspNetCore.Identity;

namespace SeatsSelector.WebAPI.Models
{
    public class UserEntity : IdentityUser<int>
    {
        public string DisplayName { get; set; }
    }
}
