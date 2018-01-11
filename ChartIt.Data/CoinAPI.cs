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

namespace ChartIt.Data
{
    public class CoinAPI
    {
        private static IReadOnlyDictionary<string, CoinInfo> _coinList;

        public async static Task<List<CoinDetails>> GetTicker()
        {
            var Coins = new List<CoinDetails>();

            var Client = CoinMarketCapClient.GetInstance();
            var TickerList = await Client.GetTickerListAsync(1000);
            try
            {

          
            foreach (var Ticker in TickerList)
            {
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
                    Volume24hUsd = Ticker.Volume24hUsd.GetValueOrDefault()
                };

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

            var CryptoId      = Int32.Parse(List[symbol].Id); 
            var SnapshotTask  = GetCoinSnapshot(CryptoId);
            var TickerTask    = GetCoinTicker(id);
            var SocialTask    = GetSocialStats(CryptoId);
            var ChartDataTask = GetCandleData(symbol, baseCoinSymbol);

            await Task.WhenAll(SnapshotTask, TickerTask, ChartDataTask, SocialTask);

            Response.Metadata    =   SnapshotTask.Result;
            Response.Details     =   TickerTask.Result;
            Response.CandleData  =   ChartDataTask.Result; 
            Response.SocialStats =   SocialTask.Result;

            return Response; 
        }
        public async static Task<CoinSocialStats> GetSocialStats(int id)
        {
            var client = new CryptoCompareClient();
            var Snapshot = await client.Coins.SocialStats(id);
         
            return Snapshot.Data.ToEntity(); 
        }
        public async static Task<CoinCandleCharts> GetCandleData(string symbol1, string symbol2)
        {
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
    }
}
