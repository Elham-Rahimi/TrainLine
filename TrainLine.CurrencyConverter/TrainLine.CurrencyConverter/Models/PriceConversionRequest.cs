using System.ComponentModel.DataAnnotations;

namespace TrainLine.CurrencyConverter.Models
{
    public class PriceConversionRequest
    {
        [Required]
        public decimal Price { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z][a-zA-Z][a-zA-Z]")]
        public string SourceCurrency { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z][a-zA-Z][a-zA-Z]")]
        public string TargetCurrency { get; set; }
    }
}
