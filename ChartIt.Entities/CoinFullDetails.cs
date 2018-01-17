using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartIt.Entities
{
    public class CoinMetadata
    {
        public string Url { get; set; }
        public string Twitter { get; set; }
        public string TotalCoinSupply { get; set; }
        public string TotalCoinsMined { get; set; }
        public string Technology { get; set; }
        public string Symbol { get; set; } 
        public DateTimeOffset StartDate { get; set; }
        public string ProofType { get; set; }
        public string PreviousTotalCoinsMined { get; set; }
        public string NetHashesPerSecond { get; set; }
        public string Name { get; set; } 
        public DateTimeOffset LastBlockExplorerUpdateTS { get; set; }
        public string InfoTop { get; set; }
        public string WarningTop { get; set; }
        public string ImageUrl { get; set; }
        public string H1Text { get; set; }
        public string Features { get; set; }
        public string DocumentType { get; set; }
        public string DifficultyAdjustment { get; set; }
        public string Description { get; set; }
        public string DangerTop { get; set; }
        public string BlockTime { get; set; }
        public string BlockRewardReduction { get; set; }
        public string BlockReward { get; set; }
        public string BlockNumber { get; set; }
        public string BaseAngularUrl { get; set; }
        public string Algorithm { get; set; }
        public string AffiliateUrl { get; set; }
        public string Id { get; set; }
        public string Website { get; set; }



        public ICODetails ICO { get; set; }

    }
}
