using System;
using System.Collections.Generic;
using System.Text;

namespace ChartIt.Entities
{ 
    public class GetCoinMetadataResponse
    {
        public CoinDetails Details { get; set; }
        public ICODetails ICO { get; set; }
        public CoinMetadata Metadata { get; set; }
        public CoinCandleCharts CandleData { get; set; }
        public CoinSocialStats SocialStats { get; set; }
    }
}
