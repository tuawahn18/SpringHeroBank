namespace SpringHeroBank.Entity;

public class Transaction
{
    public int Id { get; set; }
    public string AccountNumber { get; set; }
    
    public string TransactionType { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransacAmounttionDate { get; set; }
}