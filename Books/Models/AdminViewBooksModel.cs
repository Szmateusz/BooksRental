namespace Books.Models
{
    public class AdminViewBooksModel
    {
        
        public List<Book>? Books { get; set; }

        public string? SearchQuery { get; set; }

        public Genres Genres { get; set; }

    }
}
