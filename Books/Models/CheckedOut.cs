namespace Books.Models
{
    public class CheckedOut
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
        public DateTime DateCheckedOut { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? DateReturned { get; set; }
    }
}
