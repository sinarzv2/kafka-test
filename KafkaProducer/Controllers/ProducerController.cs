using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using KafkaProducer.Models;
using Microsoft.AspNetCore.Mvc;

namespace KafkaProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private const string BootstrapServers = "kafka:9092";

        private const string Topic = "mytest3";

        private const string SchemaRegistryUrl = "schema-registry:8081";

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequest orderRequest)
        {
            var message = JsonSerializer.Serialize(orderRequest);
            return Ok(await SendOrderRequest(Topic, orderRequest));
        }

        private async Task<bool> SendOrderRequest(string topic, OrderRequest message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = BootstrapServers,
                ClientId = Dns.GetHostName(),
                RequestTimeoutMs = 2000,
                MessageTimeoutMs = 5000,
                SocketTimeoutMs = 3000,
                SocketMaxFails = 0
            };
            var schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = SchemaRegistryUrl
            };

            try
            {
                using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
                using var producer = new ProducerBuilder<Null, OrderRequest>(config)
                    .SetValueSerializer(new AvroSerializer<OrderRequest>(schemaRegistry))
                    .Build();

                var result = await producer
                    .ProduceAsync(topic, new Message<Null, OrderRequest> { Value = message });

               Debug.WriteLine($"Delivery Timestamp:{result.Timestamp.UtcDateTime}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
            }

            return false;
        }
    }
}
