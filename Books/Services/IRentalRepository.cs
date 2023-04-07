using Books.Models;

namespace Books.Services
{
    public interface IRentalRepository
    {
        IEnumerable<Rental> GetAllRentalBooks();
        IEnumerable<Rental> GetAllOverdueRentalBooks();

        IEnumerable<Rental> GetAllUserRentalBooks(string id);

        IEnumerable<Rental> GetAllUserCurrentRentalBooks(string id);
        IEnumerable<Rental> GetAllUserOverdueRentalBooks(string userId);
        Rental GetRentalById(int id);
        void AddRental(Rental checkedOut);
        void UpdateRental(Rental checkedOut);
        void DeleteRental(int id);
    }
}
