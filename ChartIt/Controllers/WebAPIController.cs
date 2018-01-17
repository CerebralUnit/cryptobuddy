using ChartIt.Data;
using ChartIt.Entities;
using CryptoCompare;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ChartIt.Controllers
{
    public class WebAPIController : ApiController
    {
        [Route("api/getticker")]
        public async Task<List<CoinDetails>> GetTicker()
        { 
            var Stats = await CoinAPI.GetTicker();
            return Stats;
        }

        [Route("api/getcoininfo")]
        public async Task<CoinDetails> GetCoinInfo([FromUri] string id)
        {
            var Stats = await CoinAPI.GetCoinTicker(id);
            return Stats;
        }

        [Route("api/getglobalstats")]
        public async Task<MarketInfo> GetGlobalStats()
        {
            var Stats = await CoinAPI.GetGlobalStats();
            return Stats;
        }

        [Route("api/getcoinmeta")]
        public async Task<GetCoinMetadataResponse> GetCoinMeta([FromUri] string symbol, [FromUri] string id)
        {
            var Response = await CoinAPI.GetCoinMetadata(symbol, id);
            return Response;
        }
        [Route("api/getcandledata")]
        public async Task<CoinCandleCharts> GetCandleData([FromUri] string symbol, [FromUri] string symbol2)
        {
            var Details = await CoinAPI.GetCandleData(symbol, symbol2);
            return Details;
        }

        [Route("api/gethashtagstats")]
        public TwitterHashtagStats GetHashtagStats([FromUri] string hashtag)
        {
            var Stats = Social.GetHashtagData(hashtag);
            return Stats;
        }

        [Route("api/getsentiment")]
        public dynamic GetSentiment([FromUri] string q)
        {
            var Stats = Social.GetSentiment(q).Trim(new char[2] { ')', '(' });
            return JsonConvert.DeserializeObject( Stats );
        }
    }

}