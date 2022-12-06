using KafkaConsumer.Models;
using MassTransit;
using System.Text.Json;

namespace KafkaConsumer.Services
{
    public class ApacheKafkaConsumer : IConsumer<OrderRequest>
    {
        public Task Consume(ConsumeContext<OrderRequest> context)
        {
            var orderRequest = JsonSerializer.Serialize(context.Message);
            Console.WriteLine($"Processing Order Id: {orderRequest}");
            return Task.CompletedTask;
        }
    }
}
