using SpringHeroBank.Entity;

namespace SpringHeroBank.Repository;

public interface ITransactionRepository
{
    void AddTransaction(Transaction transaction);
    List<Transaction> GetTransactionsByAccountNumber(string accountNumber, int pageNumber, int pageSize);
    List<Transaction> GetAllTransactions();
    List<Transaction> SearchTransactionsByAccountNumber(string accountNumber, int pageNumber, int pageSize);
}