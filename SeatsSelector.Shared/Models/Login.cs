using System.ComponentModel.DataAnnotations;

namespace SeatsSelector.Application.Models
{
    public class Login
    {
        [Required(ErrorMessage = "ID를 입력해 주세요")]
        public string Username { get; set; }

		[Required(ErrorMessage = "비밀번호를 입력해 주세요")]
        public string Password { get; set; }
        public bool IsPersistant { get; set; }
    }
}
