using Books.Models;

namespace Books.Services
{
    public interface IReserveRepository
    {
        IEnumerable<Reserve> GetAllReserveBooks();
        IEnumerable<Reserve> GetAllUserReserveBooks(string id);
        Reserve GetReserveById(int id);
        Reserve GetReserveByBookId(int id);

        void AddReserve(Reserve reserve);
        void UpdateReserve(Reserve reserve);
        void DeleteReserve(int id);
    }
}
