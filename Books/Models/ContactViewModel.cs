using System.ComponentModel.DataAnnotations;

namespace Books.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Pole 'Imię' jest wymagane.")]
        [StringLength(100, ErrorMessage = "Pole 'Imię' może mieć maksymalnie 100 znaków.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pole 'Adres e-mail' jest wymagane.")]
        [EmailAddress(ErrorMessage = "Pole 'Adres e-mail' musi być prawidłowym adresem e-mail.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole 'Temat' jest wymagane.")]
        [StringLength(100, ErrorMessage = "Pole 'Temat' może mieć maksymalnie 100 znaków.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Pole 'Wiadomość' jest wymagane.")]
        [StringLength(500, ErrorMessage = "Pole 'Wiadomość' może mieć maksymalnie 500 znaków.")]
        public string Message { get; set; }
    }
}
