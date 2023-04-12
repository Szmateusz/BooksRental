using System.ComponentModel.DataAnnotations;

namespace Books.Models
{
    public class LoginModel
    {
        [Display(Name = "Login")]
        [Required(ErrorMessage = "Pole  'Nazwa Użytkownika' jest wymagane")]
        public string UserName { get; set; }


        [Display(Name = "Hasło")]
        [Required(ErrorMessage = "Pole 'hasło' jest wymagane.")]
        public string Password { get; set; }
    }
}
