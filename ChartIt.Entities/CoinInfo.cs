using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartIt.Entities
{
    public class CoinDetails
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }

        public int Rank { get; set; }
        
        public double PriceBtc { get; set; }
        
        public double? AvailableSupply { get; set; }
        
        public double? TotalSupply { get; set; }
        
        public double? PercentChange1h { get; set; }
        
        public double? PercentChange24h { get; set; }
        
        public double? PercentChange7d { get; set; }
         
        public double PriceUsd { get; set; }

        public Dictionary<Currency, double?> MarketCapOther { get; set; }

        public Dictionary<Currency, double?> Volume24hOther { get; set; }

        public Dictionary<Currency, double?> PriceOther { get; set; }

        public double? MarketCapUsd { get; set; }

        public DateTime LastUpdated { get; set; }
         
        public double LastUpdatedUnixTime { get; set; }
         
        public double? Volume24hUsd { get; set; }

    }
}
