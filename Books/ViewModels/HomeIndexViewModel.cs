using Books.Models;

namespace Books.ViewModels
{
    public class IndexViewModel
    {
        public List<Book> LatestBooks { get; set; }
        public List<Book> RecommendedBooks { get; set; }
        public List<Book> SearchedBooks { get; set; }
    }
}
