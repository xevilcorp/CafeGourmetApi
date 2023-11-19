public class ProductOrder
{
    public int Id { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public Address Address { get; set; }
    public int AddressId { get; set; }
    public User? User { get; set; }
    public string UserId { get; set; }
    public Transaction? Transaction { get; set; }
    public int? TransactionId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
}