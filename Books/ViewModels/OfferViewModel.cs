using Books.Models;

namespace Books.ViewModels
{
    public class OfferViewModel
    {
        public List<Book> Books { get; set; }
        public BookGenre Genres { get; set; }
        public string? SearchQuery { get; set; }
    }
}
