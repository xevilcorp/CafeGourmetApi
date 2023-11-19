using System.Text.Json.Serialization;

public class OrderItem
{
    public int Id { get; set; }
    public Product? Product { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    [JsonIgnore]
    public ProductOrder? ProductOrder { get; set; }
    public int? ProductOrderId { get; set; }
}