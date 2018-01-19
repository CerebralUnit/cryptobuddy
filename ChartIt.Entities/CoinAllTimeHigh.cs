using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartIt.Entities
{
    public class CoinAllTimeHigh
    {   
        public string Name { get; set; }
        public float ATH { get; set; }
        public DateTime LastATH { get; set; }
    }
}
