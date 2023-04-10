namespace Books.Models
{
    public class AdminViewOverdueBooks
    {
        public List<Rental>? Rentals { get; set; }
        public string? SearchQuery { get; set; }
    }
}
