﻿using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaConsumer.Models;
using System.Text.Json;

namespace KafkaConsumer.Services
{
    public class ApacheKafkaConsumerService : BackgroundService
    {
        private readonly string _topic = "mytest3";
        private readonly string _bootstrapServers = "kafka:9092";
        private readonly string _groupId = "test_group";
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

            try
            {
                await CreateTopics(config);
                new Thread(() =>
                {
                    
                    StartConsumerLoop(stoppingToken, config);
                }).Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
           
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
