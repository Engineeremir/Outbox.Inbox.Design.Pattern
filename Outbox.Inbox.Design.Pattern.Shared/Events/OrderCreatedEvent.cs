using Outbox.Inbox.Design.Pattern.Shared.Datas;

namespace Outbox.Inbox.Design.Pattern.Shared.Events;

public class OrderCreatedEvent
{
    public int OrderId { get; set; }
    public int BuyerId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public Guid IdempotentToken { get; set; }
}