namespace Books.Models
{
    public class UserRentalsViewModel
    {
        public List<Rental>? CurrentRentals { get; set; }
        public List<Rental>? RentalsHistory { get; set; }
        public List<Rental>? RentalsOverdue { get; set; }


    }
}
