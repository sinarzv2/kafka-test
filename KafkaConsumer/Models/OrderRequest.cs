using Avro;
using Avro.Specific;

namespace KafkaConsumer.Models
{
    public record OrderRequest : ISpecificRecord
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public object Get(int fieldPos)
        {
            return fieldPos switch
            {
                0 => OrderId,
                1 => ProductId,
                2 => CustomerId,
                3 => Quantity,
                4 => Status,
                _ => throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()")
            };
        }

        public void Put(int fieldPos, object fieldValue)
        {
            switch (fieldPos)
            {
                case 0: OrderId = (int)fieldValue; break;
                case 1: ProductId = (int)fieldValue; break;
                case 2: CustomerId = (int)fieldValue; break;
                case 3: Quantity = (int)fieldValue; break;
                case 4: Status = (string)fieldValue; break;
                default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
            }
        }

        public Schema Schema => Schema.Parse(@"
{
  ""namespace"": ""confluent.io.examples.serialization.avro"",
  ""name"": ""OrderRequest"",
  ""type"": ""record"",
  ""fields"": [
    {
      ""name"": ""OrderId"",
      ""type"": ""int""
    },
    {
      ""name"": ""ProductId"",
      ""type"": ""int""
    },
    {
      ""name"": ""CustomerId"",
      ""type"": ""int""
    },
    {
      ""name"": ""Quantity"",
      ""type"": ""int""
    },
    {
      ""name"": ""Status"",
      ""type"": ""string""
    }
  ]
}");
    }
}
