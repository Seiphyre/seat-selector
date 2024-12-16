using System.Security.Claims;

namespace SeatsSelector.Shared.Models.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }

        //public List<string> Roles { get; set; } = new();



        // ---------------------------------------------

        public ClaimsPrincipal ToClaimsPrincipal()
        {
            var claims = new Claim[]
            {
                new (ClaimTypes.NameIdentifier, Username),
                new (ClaimTypes.Name, DisplayName)
            };
            //.Concat(Roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray());

            // --

            var claimsIdentity = new ClaimsIdentity(claims);

            // --

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // --

            return claimsPrincipal;
        }

        //public static User FromClaimsPrincipal(ClaimsPrincipal principal)
        //{
        //    return new User()
        //    {
        //        Username = principal.FindFirst(ClaimTypes.Name)?.Value ?? "",
        //        Password = principal.FindFirst(ClaimTypes.Hash)?.Value ?? "",
        //        Roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList()
        //    };
        //}
    }
}
