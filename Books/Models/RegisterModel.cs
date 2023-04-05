using System.ComponentModel.DataAnnotations;

namespace Books.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
