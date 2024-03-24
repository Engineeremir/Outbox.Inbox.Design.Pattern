namespace Outbox.Inbox.Design.Pattern.Order.API.Dtos
{
    public class CreateOrderDto
    {
        public int BuyerId { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; }
    }
}
