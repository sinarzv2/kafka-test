using Avro.Specific;
using Avro;

namespace Common.Models
{
    public class Document : ISpecificRecord
    {
        public string Exchangetype { get; set; } = string.Empty;
        public string Volume { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string BranchCode { get; set; } = string.Empty;
        public string SymbolName { get; set; } = string.Empty;
        public string SymbolFullName { get; set; } = string.Empty;
        public string SymbolIsin { get; set; } = string.Empty;
        public decimal BrokrageWageAmount { get; set; }
        public decimal SupervisorWageAmount { get; set; }
        public decimal ITCWageAmount { get; set; }
        public decimal ExchangeWageAmount { get; set; }
        public decimal CSDIWageAmount { get; set; }
        public decimal DiscountWageAmount { get; set; }
        public decimal AccessabilityExchangeWageAmount { get; set; }
        public decimal ShareProfitAmount { get; set; }
        public object Get(int fieldPos)
        {
            return fieldPos switch
            {
                0 => Exchangetype,
                1 => Volume,
                2 => Price,
                3 => BranchCode,
                4 => SymbolName,
                5 => SymbolFullName,
                6 => SymbolIsin,
                7 => new AvroDecimal(BrokrageWageAmount),
                8 => new AvroDecimal(SupervisorWageAmount),
                9 => new AvroDecimal(ITCWageAmount),
                10 => new AvroDecimal(ExchangeWageAmount),
                11 => new AvroDecimal(CSDIWageAmount),
                12 => new AvroDecimal(DiscountWageAmount),
                13 => new AvroDecimal(AccessabilityExchangeWageAmount),
                14 => new AvroDecimal(ShareProfitAmount),

                _ => throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()")
            };
        }

        public void Put(int fieldPos, object fieldValue)
        {
            switch (fieldPos)
            {
                case 0: Exchangetype = (string)fieldValue; break;
                case 1: Volume = (string)fieldValue; break;
                case 2: Price = (string)fieldValue; break;
                case 3: BranchCode = (string)fieldValue; break;
                case 4: SymbolName = (string)fieldValue; break;
                case 5: SymbolFullName = (string)fieldValue; break;
                case 6: SymbolIsin = (string)fieldValue; break;
                case 7: BrokrageWageAmount = (decimal)(AvroDecimal)fieldValue; break;
                case 8: SupervisorWageAmount = (decimal)(AvroDecimal)fieldValue; break;
                case 9: ITCWageAmount = (decimal)(AvroDecimal)fieldValue; break;
                case 10: ExchangeWageAmount = (decimal)(AvroDecimal)fieldValue; break;
                case 11: CSDIWageAmount = (decimal)(AvroDecimal)fieldValue; break;
                case 12: DiscountWageAmount = (decimal)(AvroDecimal)fieldValue; break;
                case 13: AccessabilityExchangeWageAmount = (decimal)(AvroDecimal)fieldValue; break;
                case 14: ShareProfitAmount = (decimal)(AvroDecimal)fieldValue; break;
                default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
            }
        }

        public Schema Schema => Schema.Parse(@"
{
  ""namespace"": ""Common.Models"",
  ""name"": ""Document"",
  ""type"": ""record"",
  ""fields"": [
    {
      ""name"": ""Exchangetype"",
      ""type"": ""string""
    },
    {
      ""name"": ""Volume"",
      ""type"": ""string""
    },
    {
      ""name"": ""Price"",
      ""type"": ""string""
    },
    {
      ""name"": ""BranchCode"",
      ""type"": ""string""
    },
    {
      ""name"": ""SymbolName"",
      ""type"": ""string""
    },
    {
      ""name"": ""SymbolFullName"",
      ""type"": ""string""
    },
    {
      ""name"": ""SymbolIsin"",
      ""type"": ""string""
    },
    {
      ""name"": ""BrokrageWageAmount"",
      ""type"": {
        ""type"": ""bytes"",
        ""logicalType"": ""decimal"",
        ""precision"": 18,
        ""scale"": 0
      }
    },
    {
      ""name"": ""SupervisorWageAmount"",
      ""type"": {
        ""type"": ""bytes"",
        ""logicalType"": ""decimal"",
        ""precision"": 18,
        ""scale"": 0
      }
    },
    {
      ""name"": ""ITCWageAmount"",
      ""type"": {
        ""type"": ""bytes"",
        ""logicalType"": ""decimal"",
        ""precision"": 18,
        ""scale"": 0
      }
    },
    {
      ""name"": ""ExchangeWageAmount"",
      ""type"": {
        ""type"": ""bytes"",
        ""logicalType"": ""decimal"",
        ""precision"": 18,
        ""scale"": 0
      }
    },
    {
      ""name"": ""CSDIWageAmount"",
      ""type"": {
        ""type"": ""bytes"",
        ""logicalType"": ""decimal"",
        ""precision"": 18,
        ""scale"": 0
      }
    },
    {
      ""name"": ""DiscountWageAmount"",
      ""type"": {
        ""type"": ""bytes"",
        ""logicalType"": ""decimal"",
        ""precision"": 18,
        ""scale"": 0
      }
    },
    {
      ""name"": ""AccessabilityExchangeWageAmount"",
      ""type"": {
        ""type"": ""bytes"",
        ""logicalType"": ""decimal"",
        ""precision"": 18,
        ""scale"": 0
      }
    },
    {
      ""name"": ""ShareProfitAmount"",
      ""type"": {
        ""type"": ""bytes"",
        ""logicalType"": ""decimal"",
        ""precision"": 18,
        ""scale"": 0
      }
    }

  ]
}
");
    }

}
