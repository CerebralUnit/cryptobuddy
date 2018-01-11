using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoCompare
{
    public class CoinSocialStatsResponse : BaseApiResponse
    {
        [JsonProperty("Data")]
        public CoinSocialData Data { get; set; }
    }
}
