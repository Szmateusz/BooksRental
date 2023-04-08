using System.ComponentModel.DataAnnotations;

namespace Books.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public string Publisher { get; set; }
        public Genres Genre { get; set; }
        public int Year { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        public bool IsRecommended { get; set; }
    }
}
