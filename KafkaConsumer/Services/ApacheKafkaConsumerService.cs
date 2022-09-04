using Confluent.Kafka;
using KafkaConsumer.Models;
using System.Text.Json;

namespace KafkaConsumer.Services
{
    public class ApacheKafkaConsumerService : BackgroundService
    {
        private readonly string _topic = "mytest";
        private readonly string _bootstrapServers = "kafka:9092";
        private readonly string _groupId = "test_group";
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                GroupId = _groupId,
                SocketConnectionSetupTimeoutMs = 5000,
                SocketTimeoutMs = 3000,
                SocketMaxFails = 0,
                AllowAutoCreateTopics = true
            };

            try
            {
                new Thread(() => { StartConsumerLoop(stoppingToken, config); }).Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return Task.CompletedTask;
        }


        private void StartConsumerLoop(CancellationToken cancellationToken, ConsumerConfig config)
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_topic);
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(cancellationToken);

                    var orderRequest = JsonSerializer.Deserialize
                        <OrderProcessingRequest>
                        (consumeResult.Message.Value);
                    if (orderRequest != null) Console.WriteLine($"Processing Order Id: {orderRequest}");


                }

                consumer.Close();
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }

        }

    }
}
