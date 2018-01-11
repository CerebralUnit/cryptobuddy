﻿using System;
using System.IO;
using System.Threading.Tasks;
using Binance;
using Binance.Api; 
using Binance.Cache;
using Binance.Market;
using Binance.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChartId.DataAccess
{
    public class Binance
    {
        public static void GetStats()
        {
            try
            {
                // Load configuration.
                var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, false)
                .Build();

                // Configure services.
                var services = new ServiceCollection()
                .AddBinance()
                .AddLogging(builder => builder.SetMinimumLevel(LogLevel.Trace))
                .BuildServiceProvider();

                // Configure logging.
                services.GetService<ILoggerFactory>()
                .AddFile(configuration.GetSection("Logging").GetSection("File"));

                // Get configuration settings.
                var symbol = configuration.GetSection("Statistics")?["Symbol"] ?? Symbol.BTC_USDT;

                var cache = services.GetService<ISymbolStatisticsCache>();

                using (var controller = new RetryTaskController())
                {
                    var api = services.GetService<IBinanceApi>();

                    // Query and display the 24-hour statistics.
                    Display(await api.Get24HourStatisticsAsync(symbol));

                    // Monitor 24-hour statistics and display updates in real-time.
                    controller.Begin(
                    tkn => cache.SubscribeAsync(symbol, evt => Display(evt.Statistics), tkn),
                    err => Console.WriteLine(err.Message));

                    Console.ReadKey(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine();
                Console.WriteLine("  ...press any key to close window.");
                Console.ReadKey(true);
            }
        }
        
        private static void Display(params SymbolStatistics[] statistics)
        {
            Console.SetCursorPosition(0, 0);

            foreach (var stats in statistics)
            {
                Console.WriteLine($"  24-hour statistics for {stats.Symbol}:");
                Console.WriteLine($"    %: {stats.PriceChangePercent:0.00} | O: {stats.OpenPrice:0.00000000} | H: {stats.HighPrice:0.00000000} | L: {stats.LowPrice:0.00000000} | V: {stats.Volume:0.}");
                Console.WriteLine($"    Bid: {stats.BidPrice:0.00000000} | Last: {stats.LastPrice:0.00000000} | Ask: {stats.AskPrice:0.00000000} | Avg: {stats.WeightedAveragePrice:0.00000000}");
                Console.WriteLine();
            }

            Console.WriteLine("...press any key to exit.");
        }
    
    }
}
