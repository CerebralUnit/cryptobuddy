using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartIt.Entities
{
    public class MarketInfo
    {
        public double MarketCapUsd { get; set; }
         
        public double Volume24hUsd { get; set; }
         
        public double BTCPercentageOfMarketCap { get; set; }
       
        public int ActiveCurrencies { get; set; }
       
        public int ActiveAssets { get; set; }
        
        public int ActiveMarkets { get; set; }
    }
}
