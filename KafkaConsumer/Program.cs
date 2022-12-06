using Confluent.Kafka;
using KafkaConsumer.Models;
using KafkaConsumer.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));

    x.AddRider(rider =>
    {
        rider.AddConsumer<ApacheKafkaConsumer>();

        rider.UsingKafka((context, k) =>
        {
            k.Host("10.1.17.4:29092");

            k.TopicEndpoint<OrderRequest>("topic-name", "group_id", c =>
            {
                c.ConfigureConsumer<ApacheKafkaConsumer>(context);
                //c.AutoOffsetReset = AutoOffsetReset.Earliest;
                //c.CreateIfMissing(t => t.NumPartitions = 1);
            });

        });
    });
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();




app.UseAuthorization();

app.MapControllers();

app.Run();
