using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Binance;
using Binance.Account.Orders;
using Binance.Api; 
using Binance.Cache;
using Binance.Market;
using Binance.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CoinMarketCap;
using CoinMarketCap.Entities;
using ChartIt.Entities;

using CryptoCompare;
using System.Diagnostics;
using System.Web;
using ScrapySharp.Network;
using ScrapySharp.Extensions;
using System.Collections.ObjectModel;

namespace ChartIt.Data
{
    public class CoinAPI
    {
        private static IReadOnlyDictionary<string, CoinInfo> _coinList;
        private static List<string> TopLevelCoins = new List<string>() { "BTC", "ETH", "LIT", "USDT" };

        private static Dictionary<string, DateTime> CoinAges = new JSONFile<Dictionary<string, DateTime>>().Object;
        private static Dictionary<string, CoinAllTimeHigh> AllTimeHighs = null;
        private static IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<string>>> Exchanges = null;
        public static int? GetCoinAgeInDays(string symbol, string id)
        {
            DateTime? StartDate = null;
            int? Age = null;
            var Key = symbol.ToLower();

           
            if (id == "blockCat")
                Key += "1";
            else if (id == "bitclave")
                Key += "2";
             

            if (CoinAges == null)
            {
              
                var FileName = @"CoinAge.json";
            
                var JSFile = new JSONFile<Dictionary<string, DateTime>>(FileName, true);

                CoinAges = JSFile.Object;
            }

            if (CoinAges.ContainsKey(symbol.ToLower())) 
                StartDate = CoinAges[symbol.ToLower()];

            if (StartDate.HasValue)
                Age = (int)Math.Floor((DateTime.Now - StartDate.Value).TotalDays);

            return Age;
        }

        public async static Task<List<CoinDetails>> GetTicker()
        {
            var Coins = new List<CoinDetails>();

            var Client = CoinMarketCapClient.GetInstance();
            var TickerList = await Client.GetTickerListAsync(1000);
            try
            {
                if (AllTimeHighs == null)
                {
                    AllTimeHighs = await GetATHList();
                }
          
                foreach (var Ticker in TickerList)
                {
                    int? daysOld = null;

                  
                    try
                    {
                        daysOld = GetCoinAgeInDays(Ticker.Symbol, Ticker.Id);
                    }
                    catch(Exception ex)
                    {

                        var E = ex;
                            Debug.WriteLine(ex.Message);
                            if (ex.InnerException != null)
                                Debug.WriteLine(ex.InnerException.Message);
                    }

                    var Info = new CoinDetails() {
                        AvailableSupply = Ticker.AvailableSupply.GetValueOrDefault(),
                        Id = Ticker.Id,
                        Name = Ticker.Name,
                        PercentChange1h = Ticker.PercentChange1h.GetValueOrDefault(),
                        PercentChange24h = Ticker.PercentChange24h.GetValueOrDefault(),
                        PercentChange7d = Ticker.PercentChange7d.GetValueOrDefault(),
                        PriceBtc = Ticker.PriceBtc,
                        Rank = Ticker.Rank,
                        Symbol = Ticker.Symbol,
                        TotalSupply = Ticker.TotalSupply.GetValueOrDefault(),
                        LastUpdatedUnixTime = Ticker.LastUpdatedUnixTime,
                        LastUpdated = Ticker.LastUpdated,
                        MarketCapUsd = Ticker.MarketCapUsd.GetValueOrDefault(),
                        MarketCapOther = Ticker.MarketCapOther.ToEntity(),
                        PriceOther = Ticker.PriceOther.ToEntity(),
                        PriceUsd = Ticker.PriceUsd,
                        Volume24hOther = Ticker.Volume24hOther.ToEntity(),
                        Volume24hUsd = Ticker.Volume24hUsd.GetValueOrDefault(),
                        DaysOld = daysOld
                    };

                    if (AllTimeHighs.ContainsKey(Ticker.Id))
                    {
                        Info.ATH = AllTimeHighs[Ticker.Id].ATH;
                        Info.LastATH = AllTimeHighs[Ticker.Id].LastATH;
                    }
                    Coins.Add(Info);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (ex.InnerException != null)
                    Debug.WriteLine(ex.InnerException.Message);
            }
            return Coins;
        }

        public async static Task<MarketInfo> GetGlobalStats()
        {
            var Client = CoinMarketCapClient.GetInstance();
            var Stats = await Client.GetGlobalDataAsync();

            return Stats.ToEntity();
        }

        public async static Task<CoinDetails> GetCoinTicker(string id)
        {
            CoinDetails Coin = null;

            var Client = CoinMarketCapClient.GetInstance();
            var Ticker = await Client.GetTickerAsync(id);
           
            Coin = new CoinDetails()
            {
                AvailableSupply = Ticker.AvailableSupply,
                Id = Ticker.Id,
                Name = Ticker.Name,
                PercentChange1h = Ticker.PercentChange1h,
                PercentChange24h = Ticker.PercentChange24h,
                PercentChange7d = Ticker.PercentChange7d,
                PriceBtc = Ticker.PriceBtc,
                Rank = Ticker.Rank,
                Symbol = Ticker.Symbol,
                TotalSupply = Ticker.TotalSupply,
                LastUpdatedUnixTime = Ticker.LastUpdatedUnixTime,
                LastUpdated = Ticker.LastUpdated,
                MarketCapUsd = Ticker.MarketCapUsd,
                MarketCapOther = Ticker.MarketCapOther.ToEntity(),
                PriceOther = Ticker.PriceOther.ToEntity(),
                PriceUsd = Ticker.PriceUsd,
                Volume24hOther = Ticker.Volume24hOther.ToEntity(),
                Volume24hUsd = Ticker.Volume24hUsd
            };

            return Coin;
        }

        public async static Task<GetCoinMetadataResponse> GetCoinMetadata(string symbol, string id, string baseCoinSymbol = "BTC")
        {
            var Response = new GetCoinMetadataResponse(); 
            var List = await GetCoinList();

            if (!List.ContainsKey(symbol))
            {
                var Client = CoinMarketCapClient.GetInstance();
                Response.Details = await GetCoinTicker(id);

                return Response;
            }

            if (Exchanges == null)
                Exchanges = await GetExchanges();

            var CryptoId      = Int32.Parse(List[symbol].Id); 
            var SnapshotTask  = GetCoinSnapshot(CryptoId);
            var TickerTask    = GetCoinTicker(id);
            var SocialTask    = GetSocialStats(CryptoId);
            var ChartDataTask = GetCandleData(symbol, baseCoinSymbol);

            await Task.WhenAll(SnapshotTask, TickerTask, ChartDataTask, SocialTask);

            var ExchangeList = Exchanges.Where(x => x.Value.ContainsKey(symbol)).Select(x => x.Key).ToList();

            Response.Metadata    =   SnapshotTask.Result;
            Response.Details     =   TickerTask.Result;
            Response.CandleData  =   ChartDataTask.Result; 
            Response.SocialStats =   SocialTask.Result;
            Response.Exchanges   =   ExchangeList;
            return Response; 
        }
        public async static Task<List<CoinCandleData>> GetExchangeData(string symbol, string id, List<string> Exchanges)
        {
            var client = new CryptoCompareClient();
            var ExchangeData = await client.History.MinuteAsync(symbol, "BTC", null, Exchanges, null);

            return ExchangeData.Data.ToEntities();
        }
        public async static Task<CoinSocialStats> GetSocialStats(int id)
        {
            var client = new CryptoCompareClient();
            var Snapshot = await client.Coins.SocialStats(id);
         
            return Snapshot.Data.ToEntity(); 
        }
        public async static Task<CoinCandleCharts> GetCandleData(string symbol1, string symbol2)
        {
            if(symbol1 == symbol2 && TopLevelCoins.Contains(symbol1))
            {
                symbol2 = "USDT";
            }
            else if(symbol1 == symbol2)
            {
                symbol2 = "BTC";
            }

            var Response      = new CoinCandleCharts();
            var client        = new CryptoCompareClient();
            var History       =  client.History.DayAsync(symbol1, symbol2, null, null, null);
            var HourHistory   =  client.History.HourAsync(symbol1, symbol2, null, null, null);
            var MinuteHistory =  client.History.MinuteAsync(symbol1, symbol2, null, null, null);
             
            await Task.WhenAll(History, HourHistory, MinuteHistory);

            Response.Day    = History.Result.Data.ToEntities();
            Response.Hour   = HourHistory.Result.Data.ToEntities();
            Response.Minute = MinuteHistory.Result.Data.ToEntities();

            return Response;
        }
        public async static Task<CoinMetadata> GetCoinSnapshot(int id)
        {
            var client = new CryptoCompareClient();
            var Snapshot = await client.Coins.SnapshotFullAsync(id);

            ArchiveCoinAge(Snapshot.Data.General.Symbol.ToLower(), Snapshot.Data.General.StartDate.Date);

            return Snapshot.Data.ToEntity(); 
        } 

        public async static Task<IReadOnlyDictionary<string, CoinInfo>> GetCoinList()
        {
            if(_coinList == null)
            {
                var Client = new CryptoCompareClient();
                var Response = await Client.Coins.ListAsync();

                _coinList = Response.Coins;
            }

            return _coinList;
        }
        public async static Task<Dictionary<string, CoinAllTimeHigh>> GetATHList()
        {
            var Scraper = new ScrapingBrowser();
            Scraper.AllowAutoRedirect = true;
            Scraper.AllowMetaRedirect = true;
            var ATHs = new Dictionary<string, CoinAllTimeHigh>();
            try
            {
                var Page = await Scraper.NavigateToPageAsync(new Uri("https://www.athda.com/Page/all"));
                var Table = Page.Html.CssSelect("#table").First();
               

                foreach (var row in Table.SelectNodes("tbody/tr"))
                {
                    var Cells = row.SelectNodes("td");
                    var ATH = new CoinAllTimeHigh();

                    for (var i = 0; i < Cells.Count; i++)
                    {
                        var Val = Cells[i].InnerText;

                        if (i == 1)
                            ATH.Name = Val;
                        if (i == 2)
                            ATH.ATH = float.Parse(Val.Replace("$",""));
                        if (i == 3)
                            ATH.LastATH = DateTime.Parse(Val.Trim().Substring(0, 10));
                    }

                    ATHs.Add(ATH.Name.ToLower().Replace(" ", "-"), ATH);
                }

            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                if (ex.InnerException != null)
                    Debug.WriteLine(ex.InnerException.Message);
            }

            return ATHs;
        }
        public async static Task<IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<string>>>> GetExchanges()
        {
            var Client = new CryptoCompareClient();
            var Ex = await Client.Exchanges.ListAsync();
            return Ex;
        }
        public static bool ArchiveCoinAge(string symbol, DateTime startDate)
        {
             
            var FileName = @"CoinAge.json"; 
            var JSFile = new JSONFile<Dictionary<string, DateTime>>(FileName, true);

            var Dict = JSFile.Object;

            if (!Dict.ContainsKey(symbol)) 
                Dict.Add(symbol, startDate); 
            else 
                return true; 

            var Overwrite = new JSONFile<Dictionary<string, DateTime>>(Dict, FileName);

            CoinAges = Dict;

            var Saved = Overwrite.Save(FileName);

            return Saved;
        }
    }
}
