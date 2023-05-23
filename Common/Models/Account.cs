using Avro;
using Avro.Specific;

namespace Common.Models;

public class Account : ISpecificRecord
{
    public long AccountId { get; set; }
    public string Label { get; set; } = string.Empty;
    public object Get(int fieldPos)
    {
        return fieldPos switch
        {
            0 => AccountId,
            1 => Label,
            _ => throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()")
        };
    }

    public void Put(int fieldPos, object fieldValue)
    {
        switch (fieldPos)
        {
            case 0: AccountId = (long)fieldValue; break;
            case 1: Label = (string)fieldValue; break;
            default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
        }
    }

    public Schema Schema => Schema.Parse(@"
{
  ""namespace"": ""Common.Models"",
  ""name"": ""Account"",
  ""type"": ""record"",
  ""fields"": [
    {
      ""name"": ""AccountId"",
      ""type"": ""long""
    },
    {
      ""name"": ""Label"",
      ""type"": ""string""
    }
  ]
}
");
}