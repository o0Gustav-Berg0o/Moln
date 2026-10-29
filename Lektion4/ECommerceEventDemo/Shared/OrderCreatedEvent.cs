namespace Shared;

public class OrderCreatedEvent
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public List<string> ProductNames { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
