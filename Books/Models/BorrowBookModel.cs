namespace Books.Models
{
    public class BorrowBookModel
    {
        public Rental Rental { get; set; }

        public string UserId { get; set; }
        public List<UserModel> Users { get; set; }
    }
}
