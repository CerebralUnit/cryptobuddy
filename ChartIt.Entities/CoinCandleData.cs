using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartIt.Entities
{
    public class CoinCandleCharts
    {
        public List<CoinCandleData> Day { get; set; }
        public List<CoinCandleData> Hour { get; set; }
        public List<CoinCandleData> Minute { get; set; }
    }
    public class CoinCandleData
    {
        public decimal close { get; set; }

        public decimal high { get; set; }

        public decimal low { get; set; }

        public decimal open { get; set; }
         
        public DateTimeOffset date { get; set; }

        public decimal volumeFrom { get; set; }

        public decimal volume { get; set; }
    }
}
