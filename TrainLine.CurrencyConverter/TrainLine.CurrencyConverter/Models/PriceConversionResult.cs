using System.Text.Json.Serialization;

namespace TrainLine.CurrencyConverter.Models
{
    public class PriceConversionResult
    {
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}
