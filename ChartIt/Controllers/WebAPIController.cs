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


        [Route("api/getATH")]
        public async Task<Dictionary<string, CoinAllTimeHigh>> getATH()
        {
            var ATH = await CoinAPI.GetATHList();
            return ATH;
        }

        [Route("api/getexchangedata")]
        public async Task<List<CoinCandleData>> getexchangedata(string symbol, string id, string exchanges)
        {
            var Exchanges = exchanges.Split(new char[1] { ';' }).ToList();
            var ATH = await CoinAPI.GetExchangeData(symbol, id, Exchanges);
            return ATH;
        }

        [Route("api/getcoinath")]
        public async Task<KeyValuePair<long, decimal>> GetCoinATH(string name)
        {
           
            var ATH = await CoinAPI.GetATH(name);
            return ATH;
        }

        [Route("api/getcoinscores")]
        public async Task<Dictionary<string, CoinScore>> GetCoinScores()
        {
            var Scores = await CoinAPI.GetCoinScores();

            return Scores;
        }

        [Route("api/getcoinscoredetails")]
        public async Task<Dictionary<string, Dictionary<string,int>>> GetCoinScoreDetails(string name)
        {
            var Scores = await CoinAPI.GetCoinScoreDetails(name);

            return Scores;
        }
    
    }

}