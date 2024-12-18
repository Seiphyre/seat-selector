using Microsoft.IdentityModel.Tokens;
using SeatsSelector.Shared.Models.Seats;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace SeatsSelector.Shared.Models.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public Seat Seat { get; set; }

        //public List<string> Roles { get; set; } = new();



        // ---------------------------------------------

        public ClaimsPrincipal ToClaimsPrincipal()
        {
            var claims = new Claim[]
            {
                new Claim (nameof(Id), Id.ToString()),
                new Claim (nameof(DisplayName), DisplayName)
            };

            //.Concat(Roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray());

            // --

            var claimsIdentity = new ClaimsIdentity(claims);

            // --

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // --

            return claimsPrincipal;
        }

        public static User FromClaimsPrincipal(ClaimsPrincipal principal)
        {
            return new User()
            {
                Id = int.Parse(principal.FindFirst(nameof(Id))?.Value ?? "0"),
                DisplayName = principal.FindFirst(nameof(DisplayName))?.Value ?? ""
            };
        }

        public static User FromToken(string token)
        {
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = false;

            validationParameters.ValidAudience = "seat-selector-app";
            validationParameters.ValidIssuer = "seat-selector-webapi";
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("rzN7BmKiBZVJu/ymJcbUSs3xdUAY4wRkcJasW+EH0Fc="));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out validatedToken);

            foreach(var claims in principal.Claims)
                Console.WriteLine(claims.Type);

            return User.FromClaimsPrincipal(principal);
        }
    }
}
