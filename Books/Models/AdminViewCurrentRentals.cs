namespace Books.Models
{
    public class AdminViewCurrentRentals
    {
        public List<Rental>? Rentals { get; set; }
        public string? SearchQuery { get; set; }
    }
}
