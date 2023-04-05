using Books.Models;

namespace Books.Services
{
    public interface IRentalRepository
    {
        IEnumerable<Rental> GetAllRentalBooks();
        IEnumerable<Rental> GetAllUserRentalBooks(string id);
        Rental GetRentalById(int id);
        void AddRental(Rental checkedOut);
        void UpdateRental(Rental checkedOut);
        void DeleteRental(int id);
    }
}
