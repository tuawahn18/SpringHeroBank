using MySql.Data.MySqlClient;
using SpringHeroBank.Entity;

namespace SpringHeroBank.Repository;

public class TransactionRepository: ITransactionRepository
{
    private const string MyConnectionString = "server=127.0.0.1;uid=root;pwd=;database=springherobank";
    public void AddTransaction(Transaction transaction)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(MyConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO transaction (AccountNumber, TransactionType, Amount, TransactionDate) VALUES (@AccountNumber, @TransactionType, @Amount, @TransactionDate)", conn);
                cmd.Parameters.AddWithValue("@AccountNumber", transaction.AccountNumber);
                cmd.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
                cmd.Parameters.AddWithValue("@Amount", transaction.Amount);
                cmd.Parameters.AddWithValue("@TransactionDate", transaction.TransacAmounttionDate);

                cmd.ExecuteNonQuery();
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public List<Transaction> GetTransactionsByAccountNumber(string accountNumber, int pageNumber, int pageSize)
    {
        List<Transaction> transactions = new List<Transaction>();
        try
        {
            using (MySqlConnection conn = new MySqlConnection(MyConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM transaction WHERE AccountNumber = @AccountNumber ORDER BY TransactionDate DESC LIMIT @Offset, @PageSize", conn);
                cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                cmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        transactions.Add(new Transaction
                        {
                            Id = reader.GetInt32("Id"),
                            AccountNumber = reader.GetString("AccountNumber"),
                            TransactionType = reader.GetString("TransactionType"),
                            Amount = reader.GetDecimal("Amount"),
                            TransacAmounttionDate = reader.GetDateTime("TransactionDate")
                        });
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        return transactions;
    }

    public List<Transaction> GetAllTransactions()
    {
        List<Transaction> transactions = new List<Transaction>();
        try
        {
            using (MySqlConnection conn = new MySqlConnection(MyConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM transaction ORDER BY TransactionDate DESC", conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        transactions.Add(new Transaction
                        {
                            Id = reader.GetInt32("Id"),
                            AccountNumber = reader.GetString("AccountNumber"),
                            TransactionType = reader.GetString("TransactionType"),
                            Amount = reader.GetDecimal("Amount"),
                            TransacAmounttionDate= reader.GetDateTime("TransactionDate")
                        });
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        return transactions;
    }

    public List<Transaction> SearchTransactionsByAccountNumber(string accountNumber, int pageNumber, int pageSize)
    {
        {
            return GetTransactionsByAccountNumber(accountNumber, pageNumber, pageSize);
        }
    }
}