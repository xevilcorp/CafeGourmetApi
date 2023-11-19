public class Transaction
{
  public int Id { get; set; }
  public string Tid { get; set; }
  public string PaymentId { get; set; }
  public decimal Amount { get; set; }
  public string Currency { get; set; }
  public TransactionStatus Status { get; set; }
  public PaymentType PaymentType { get; set; }
  public DateTime TransactionDate { get; set; }
  public string Description { get; set; }
}