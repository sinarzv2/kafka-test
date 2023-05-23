using Confluent.Kafka;
using Confluent.Kafka.Admin;
using System.Text.Json;
using Common.ConstansVariable;
using Common.Models;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace KafkaConsumer.Services
{
    public class ApacheKafkaConsumerFinancialTransactionService : BackgroundService
    {
        private readonly string _topic = Topic.FinancialTransaction;
        private readonly string _bootstrapServers = "kafka:9092";
        private readonly string _groupId = "test_group";
        private readonly string _schemaRegistryUrl = "schema-registry:8081";
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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
            var schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = _schemaRegistryUrl
            };
            try
            {
                await CreateTopics(config);
                new Thread(() =>
                {
                    
                    StartConsumerLoop(stoppingToken, config, schemaRegistryConfig);
                }).Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
           
        }


        private void StartConsumerLoop(CancellationToken cancellationToken, ConsumerConfig config,
            SchemaRegistryConfig schemaRegistryConfig)
        {
            using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
            using var consumer =
                new ConsumerBuilder<Null, FinancialTransaction>(config)
                    .SetValueDeserializer(new AvroDeserializer<FinancialTransaction>(schemaRegistry).AsSyncOverAsync())
                    .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                    .Build();
                consumer.Subscribe(_topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(cancellationToken);

                    var orderRequest = consumeResult.Message.Value;
                    var options = new JsonSerializerOptions
                    {
                        IgnoreReadOnlyProperties = true,
                        WriteIndented = true
                    };
                    var jsonString = JsonSerializer.Serialize(orderRequest, options);
                    if (orderRequest != null) Console.WriteLine($"Processing FinancialTransaction: {jsonString}");
                }
                consumer.Close();
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }

        }

        private  async Task CreateTopics(ConsumerConfig config)
        {
            var adminConfig = new AdminClientConfig(config);

            using var adminClient = new AdminClientBuilder(adminConfig).Build();
            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
            var existTopic = metadata.Topics.Any(a => a.Topic == _topic);
            if(existTopic)
                return;
            try
            {
                await adminClient.CreateTopicsAsync(new[] {
                    new TopicSpecification { Name = _topic, ReplicationFactor = 1, NumPartitions = 1 } });
            }
            catch (CreateTopicsException e)
            {
                Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
            }
        }
    }
}
