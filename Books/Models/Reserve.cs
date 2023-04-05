namespace Books.Models
{
    public class Reserve
    {
        public int Id { get; set; }
        public DateTime ReserveDate { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public string UserId { get; set; }
        public UserModel User { get; set; }
        
    }
}
