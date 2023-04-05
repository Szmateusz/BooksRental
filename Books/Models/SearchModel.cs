namespace Books.Models
{
    public class SearchModel
    {
        public string? SearchQuery { get; set; }

        public Genres Genres { get; set; }
    }
}
