using ChartIt.Entities;
using CoinMarketCap.Entities;
using CoinMarketCap.Enums;
using CryptoCompare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartIt.Data
{
    public static class Convert
    {
        public static Dictionary<Currency, double?> ToEntity(this Dictionary<ConvertEnum, double?> dict)
        {
            var Converted = new Dictionary<Currency, double?>();

            foreach(var Entry in dict)
            {
                var CurrencyValue = (Currency)Enum.Parse(typeof(Currency), Entry.Key.ToString());

                Converted.Add(CurrencyValue, Entry.Value.GetValueOrDefault());
            }

            return Converted;
        }

        public static MarketInfo ToEntity(this GlobalDataEntity globalData)
        {
            var Converted = new MarketInfo() {
                ActiveAssets             = globalData.ActiveAssets,
                ActiveCurrencies         = globalData.ActiveCurrencies,
                ActiveMarkets            = globalData.ActiveMarkets,
                BTCPercentageOfMarketCap = globalData.BTCPercentageOfMarketCap,
                MarketCapUsd             = globalData.MarketCapUsd,
                Volume24hUsd             = globalData.Volume24hUsd
            };

            return Converted;
        }
        public static List<CoinCandleData> ToEntities (this IReadOnlyList<CandleData> dataList)
        {
            var Entities = new List<CoinCandleData>();
            foreach(var Data in dataList)
            {
                var Converted = new CoinCandleData()
                {
                    close      = Data.Close,
                    high       = Data.High,
                    low        = Data.Close,
                    open       = Data.Open,
                    date       = Data.Time,
                    volumeFrom = Data.VolumeFrom,
                    volume   = Data.VolumeTo,
                };

                Entities.Add(Converted);
            }

            return Entities;
        }
        public static CoinMetadata ToEntity(this CoinSnapshotFullData coin)
        {
            var Converted = new CoinMetadata()
            {
               AffiliateUrl = coin.General.AffiliateUrl,
               Algorithm = coin.General.Algorithm,
               BaseAngularUrl = coin.General.BaseAngularUrl,
               BlockRewardReduction = coin.General.BlockRewardReduction,
               BlockReward = coin.General.BlockReward,
               BlockNumber = coin.General.BlockNumber,
               BlockTime = coin.General.BlockTime,
               DangerTop = coin.General.DangerTop,
               Description = coin.General.Description,
               DifficultyAdjustment = coin.General.DifficultyAdjustment,
               DocumentType = coin.General.DocumentType,
               Features = coin.General.Features,
               H1Text = coin.General.H1Text,
               Id = coin.General.Id,
               ImageUrl = coin.General.ImageUrl,
               InfoTop = coin.General.InfoTop,
               LastBlockExplorerUpdateTS = coin.General.LastBlockExplorerUpdateTS,
               Name = coin.General.Name,
               NetHashesPerSecond = coin.General.NetHashesPerSecond,
               PreviousTotalCoinsMined = coin.General.PreviousTotalCoinsMined,
               ProofType = coin.General.ProofType,
               StartDate = coin.General.StartDate,
               Symbol = coin.General.Symbol,
               Technology = coin.General.Technology,
               TotalCoinsMined = coin.General.TotalCoinsMined,
               TotalCoinSupply = coin.General.TotalCoinSupply,
               Twitter = coin.General.Twitter,
               Url = coin.General.Url,
               WarningTop = coin.General.WarningTop,
               Website = coin.General.Website,
               ICO =  coin.ICO.ToEntity()
            };

            return Converted;
        }

        public static ICODetails ToEntity(this ICO ico)
        {
            var Converted = new ICODetails()
            {
                BlogLink = ico.BlogLink, 
                Date =  ico.Date, 
                Description =  ico.Description, 
                EndDate =  ico.EndDate, 
                Features =  ico.Features, 
                FundingCap =  ico.FundingCap, 
                FundingTarget =  ico.FundingTarget, 
                FundsRaisedList =  ico.FundsRaisedList, 
                FundsRaisedUSD =  ico.FundsRaisedUSD, 
                ICOTokenSupply =  ico.ICOTokenSupply, 
                Jurisdiction =  ico.Jurisdiction, 
                LegalAdvisers =  ico.LegalAdvisers, 
                LegalForm =  ico.LegalForm, 
                PaymentMethod =  ico.PaymentMethod, 
                PublicPortfolioId =  ico.PublicPortfolioId, 
                PublicPortfolioUrl =  ico.PublicPortfolioUrl, 
                SecurityAuditCompany =  ico.SecurityAuditCompany, 
                StartPrice =  ico.StartPrice, 
                StartPriceCurrency =  ico.StartPriceCurrency, 
                Status =  ico.Status, 
                TokenPercentageForInvestors =  ico.TokenPercentageForInvestors, 
                TokenReserveSplit =  ico.TokenReserveSplit, 
                TokenSupplyPostICO =  ico.TokenSupplyPostICO, 
                TokenType =  ico.TokenType, 
                Website =  ico.Website, 
                WebsiteLink =  ico.WebsiteLink, 
                WhitePaper =  ico.WhitePaper, 
                WhitePaperLink =  ico.WhitePaperLink  

            };

            return Converted;
        }

        public static CoinSocialStats ToEntity (this CoinSocialData data)
        {
            var Converted = new CoinSocialStats() {
                FacebookLikes = data.Facebook.Likes,

                FacebookLink =data.Facebook.Link,

                FacebookIsClosed =data.Facebook.IsClosed,

                FacebookTalkingAbout =data.Facebook.TalkingAbout,

                FacebookName =data.Facebook.Name,

                FacebookPoints =data.Facebook.Points,
                 
                Name = data.General.Name,

                CoinName = data.General.CoinName,

                Type = data.General.Type,
                 
                RedditPostsPerHour = data.Reddit.PostsPerHour,

                RedditCommentsPerHour = data.Reddit.CommentsPerHour,

                RedditPostsPerDay = data.Reddit.PostsPerDay,

                RedditCommentsPerDay = data.Reddit.CommentsPerDay,

                RedditName = data.Reddit.Name,

                RedditLink = data.Reddit.Link,

                RedditActiveUsers = data.Reddit.ActiveUsers,

                RedditCommunityCreation = data.Reddit.CommunityCreation,

                RedditSubscribers = data.Reddit.Subscribers,

                RedditPoints = data.Reddit.Points,
                 
                TwitterFollowing = data.Twitter.Following,

                TwitterAccountCreation = data.Twitter.Following,

                TwitterName = data.Twitter.Following,

                TwitterLists = data.Twitter.Lists,

                TwitterStatuses = data.Twitter.Statuses,

                TwitterFavorites = data.Twitter.Favourites,

                TwitterFollowers = data.Twitter.Followers,

                TwitterLink = data.Twitter.Link,

                TwitterPoints = data.Twitter.Points,

                CodeRepoPoints = data.CodeRepository.Points,

                CodeRepos = data.CodeRepository.List.ToEntities()
             };

            return Converted;
        }

        public static List<CodeRepo> ToEntities(this List<CodeRepos> repos)
        {
            var Converted = new List<CodeRepo>();

            foreach(var repo in repos)
            {
                var ConvertedRepo = new CodeRepo() {
                    CreatedAt = repo.CreatedAt,
                    ClosedIssues = repo.ClosedIssues,
                    ClosedPullIssues = repo.ClosedPullIssues,
                    ClosedTotalIssues = repo.ClosedTotalIssues,
                    CodeRepoParentInternalId = repo.Parent == null ? 0 : repo.Parent.InternalId,
                    CodeRepoParentName = repo.Parent == null ? null : repo.Parent.Name,
                    CodeRepoParentUrl = repo.Parent == null ? null : repo.Parent.Url,
                    Fork = repo.Fork,
                    Forks = repo.Forks,
                    Language = repo.Language,
                    LastPush = repo.LastPush,
                    LastUpdate = repo.LastUpdate,
                    OpenIssues = repo.OpenIssues,
                    OpenPullIssues = repo.OpenPullIssues,
                    OpenTotalIssues = repo.OpenTotalIssues,
                    Size = repo.Size,
                    Stars = repo.Stars,
                    Subscribers = repo.Subscribers,
                    Url = repo.Url
                };

                Converted.Add(ConvertedRepo);
            }

            return Converted;
        }
    }
}
