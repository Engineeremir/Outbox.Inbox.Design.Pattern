
using MassTransit;
using Outbox.Inbox.Design.Pattern.Shared.Events;
using Outbox.Publisher.Service.Entities;
using Quartz;
using System.Text.Json;

namespace Outbox.Publisher.Service;

public class OrderOutboxPublisherJob(IPublishEndpoint publishEndpoint) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        if (OrderOutboxSingletonDatabase.DataReaderState)
        {
            OrderOutboxSingletonDatabase.DataReaderBusy();
            List<OrderOutbox> orderOutboxes = (await OrderOutboxSingletonDatabase.QueryAsync<OrderOutbox>($@"SELECT * from OrderOutBoxes where ProcessedDate is null order by occuredon asc")).ToList();

            foreach (var orderOutbox in orderOutboxes)
            {
                if (orderOutbox.Type == nameof(OrderCreatedEvent))
                {
                    OrderCreatedEvent orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(orderOutbox.Payload);
                    if (orderCreatedEvent != null)
                    {
                        await publishEndpoint.Publish(orderCreatedEvent);
                        OrderOutboxSingletonDatabase.ExecuteAsync($@"update orderoutboxes set ProcessedDate = getdate() where ID = '{orderOutbox.IdempotentToken}'");
                    }
                }
            }

            OrderOutboxSingletonDatabase.DataReaderReady();
        }
    }
}
