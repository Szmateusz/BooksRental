using System.ComponentModel.DataAnnotations;

namespace Books.Models
{
    public class RegisterModel
    {
        [Display(Name = "Login")]
        [Required(ErrorMessage = "Pole  'Nazwa Użytkownika' jest wymagane")]
        [StringLength(30, ErrorMessage = "Pole 'Nazwa uzytkownika' może mieć maksymalnie 30 znaków.")]
        public string UserName { get; set; }

        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Pole 'Imię' jest wymagane")]
        [StringLength(30, ErrorMessage = "Pole 'Imię' może mieć maksymalnie 30 znaków.")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Pole 'Nazwisko'jest wymagane.")]
        [StringLength(30, ErrorMessage = "Pole 'Nazwisko' może mieć maksymalnie 30 znaków.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Pole 'Adres e-mail' jest wymagane.")]
        [EmailAddress(ErrorMessage = "Pole 'Adres e-mail' musi być prawidłowym adresem e-mail.")]
        public string Email { get; set; }

        [Display(Name = "Hasło")]
        [Required(ErrorMessage = "Pole 'hasło' jest wymagane.")]
        [StringLength(30, ErrorMessage = "Pole 'hasło' może mieć maksymalnie 30 znaków.")]
        public string Password { get; set; }

        [Display(Name = "Data urodzenia")]
        [Required(ErrorMessage = "Pole 'Data urodzenia' jest wymagane.")]
        public DateTime DateOfBirth { get; set; }
    }
}
