namespace Books.Models
{
    public class HireModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime ReturnDate { get; set; }

    }
}
