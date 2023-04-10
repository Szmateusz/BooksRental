namespace Books.Models
{
    public class AdminViewRentalBooks
    {
        public List<Rental>? Rentals { get; set; }
        public string? SearchQuery { get; set; }
    }
}
