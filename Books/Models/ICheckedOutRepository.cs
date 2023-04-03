namespace Books.Models
{
    public interface ICheckedOutRepository
    {
        IEnumerable<CheckedOut> GetAllCheckedOutBooks();
        CheckedOut GetCheckedOutById(int id);
        void AddCheckedOut(CheckedOut checkedOut);
        void UpdateCheckedOut(CheckedOut checkedOut);
        void DeleteCheckedOut(int id);
    }
}
