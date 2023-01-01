using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Authentication
{
    public class Login
    {
        [Required(ErrorMessage = "請輸入帳號")]
        public string? Userid { get; set; }
        [Required(ErrorMessage = "請輸入密碼")]
        public string? Password { get; set; }
    }
}
