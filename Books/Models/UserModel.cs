using Microsoft.AspNetCore.Identity;

namespace Books.Models
{
    public class UserModel:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<Rental> Rentals { get; set; }

    }
}
