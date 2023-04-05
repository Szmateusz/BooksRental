using System.ComponentModel.DataAnnotations;

namespace Books.Models
{
    public class LoginModel
    {

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
