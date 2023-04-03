namespace Books.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Publisher { get; set; }
        public int Year { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        public bool IsRecommended { get; set; }
    }
}
