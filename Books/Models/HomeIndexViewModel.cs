namespace Books.Models
{
    public class HomeIndexViewModel
    {
        public List<Book>? LatestBooks { get; set; }
        public List<Book>? RecommendedBooks { get; set; }

        public string? SearchQuery { get; set; }

        public Genres Genres { get; set; }

    }
}
