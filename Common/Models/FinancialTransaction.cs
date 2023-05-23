using Avro;
using Avro.Specific;

namespace Common.Models
{
    public class FinancialTransaction : ISpecificRecord
    {
        public long TransactionId { get; set; }
        public long TransactionDateTime { get; set; }
        public long EffectiveDateTime { get; set; }
        public List<string> OperationCode { get; set; } = new();
        public decimal Amount { get; set; }
        public long PartyId { get; set; }
        public long PamCode { get; set; }
        public int EffectiveDateValue { get; set; }
        public Document Document { get; set; } = new();
        public IList<Account> Accounts { get; set; } = new List<Account>();

        public object Get(int fieldPos)
        {
            return fieldPos switch
            {
                0 => TransactionId,
                1 => TransactionDateTime,
                2 => EffectiveDateTime,
                3 => OperationCode,
                4 => new AvroDecimal(Amount),
                5 => PartyId,
                6 => PamCode,
                7 => EffectiveDateValue,
                8 => Document,
                9 => Accounts,
                _ => throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()")
            };
        }

        public void Put(int fieldPos, object fieldValue)
        {
            switch (fieldPos)
            {
                case 0: TransactionId = (long)fieldValue; break;
                case 1: TransactionDateTime = (long)fieldValue; break;
                case 2: EffectiveDateTime = (long)fieldValue; break;
                case 3: OperationCode = (List<string>)fieldValue; break;
                case 4: Amount = (decimal)(AvroDecimal)fieldValue; break;
                case 5: PartyId = (long)fieldValue; break;
                case 6: PamCode = (long)fieldValue; break;
                case 7: EffectiveDateValue = (int)fieldValue; break;
                case 8: Document = (Document)fieldValue; break;
                case 9: Accounts = (IList<Account>)fieldValue; break;
                default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
            }
        }

        public Schema Schema => Schema.Parse(@"
{
  ""namespace"": ""Common.Models"",
  ""name"": ""FinancialTransaction"",
  ""type"": ""record"",
  ""fields"": [
    {
      ""name"": ""TransactionId"",
      ""type"": ""long""
    },
    {
      ""name"": ""TransactionDateTime"",
      ""type"": ""long""
    },
    {
      ""name"": ""EffectiveDateTime"",
      ""type"": ""long""
    },
    {
      ""name"": ""OperationCode"",
      ""type"": {
        ""type"": ""array"",
        ""items"": ""string""
      }
    },
    {
      ""name"": ""Amount"",
      ""type"": {
        ""type"": ""bytes"",
        ""logicalType"": ""decimal"",
        ""precision"": 18,
        ""scale"": 0
      }
    },
    {
      ""name"": ""PartyId"",
      ""type"": ""long""
    },
    {
      ""name"": ""PamCode"",
      ""type"": ""long""
    },
    {
      ""name"": ""EffectiveDateValue"",
      ""type"": ""int""
    },
    {
      ""name"": ""Document"",
      ""namespace"": ""Common.Models"",
      ""type"": {
        ""type"": ""record"",
        ""name"": ""Document"",
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
    },
    {
      ""name"": ""Accounts"",
      ""namespace"": ""Common.Models"",
      ""type"": {
        ""type"": ""array"",
        ""items"": {
          ""type"": ""record"",
          ""name"": ""Account"",
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
      }
    }
  ]
}
");
    }
}