using System.ComponentModel.DataAnnotations;

namespace SeatsSelector.Application.Models
{
    public class Login
    {
        [Required(ErrorMessage = "This field is required.")]
        public string Username { get; set; }

		[Required(ErrorMessage = "This field is required.")]
        public string Password { get; set; }
        public bool IsPersistant { get; set; }
    }
}
