using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrainLine.CurrencyConverter.Services.ExchangeRateService
{
    public class ExchangeResponce
    {
        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("time_last_updated")]
        public long TimeLastUpdated { get; set; }

        [JsonProperty("rates")]
        public IDictionary<string, decimal> Rates { get; } = new Dictionary<string, decimal>();
    }
}
