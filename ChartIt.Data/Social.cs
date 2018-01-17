using System;
using System.Collections.Generic;
using System.Compat.Web;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;

namespace ChartIt.Data
{
    public class TwitterHashtagStats
    {
        public TwitterHashtagStats()
        {
            Tweets = new List<Tweet>();
        }

       public string Count { get; set; }
       public string FavoriteCount { get; set; }
       public string RetweetCount { get; set; }
       public List<Tweet> Tweets { get; set; }
       
              
    }
    public class Tweet
    {
        public string Id { get; set; }
        public string Text { get; set; }  
        public string RetweetCount { get; set; }  
        public string FavouritesCount { get; set; }  
        public string ProfileImageUrl { get; set; }  
        public DateTime CreatedDate { get; set; }  
        public string ScreenName { get; set; }  
        public string Name { get; set; }  
    }


    public class Social
    {
        private const string TWITTER_CONSUMER_KEY = "MOGWpXhpuPDAlqHbrfDL3EUCB";
        private const string TWITTER_CONSUMER_SECRET = "oLY46hSbPJr1gjBhERGOt0oG7zXEeLTUjZajVQbbpW0xS17USm";
        private const string TWITTER_ACCESS_TOKEN = "892418749621387264-g828f5hSiBHQeu8p0BQvrs7Ct8UTjVP";
        private const string TWITTER_ACCESS_TOKEN_SECRET = "J0rmhCqXVrLEzs3THcZpnWVh5YmcKPMtvwcttu5AfesVc";
        //https://www.csc2.ncsu.edu/faculty/healey/tweet_viz/tweet_app/
        //{"limit":450, "remain":429, "reset":1515799417, "http_code":200, "max_id":951947695773704191, "tw":
        //[{"created_at":"Fri Jan 12 23:15:13 +0000 2018",
        //"id":951955754625286144,
        //"id_str":"951955754625286144",
        //"full_text":"0.05BTC --&gt; 20BTC We need Just a Week nJoin : https://t.co/azARNloQdz n$MTL $DGB $XEM $TRIG $STRAT $LSK $IOP $PIVX $PAY $ARK $DASH $MCO $ARDR $CVC $ETH $NEO $XLM $XRP $ADA $EMC2 $OMG $BCC $VTC $AEON $ZEC $ADX $XVG $BAT $LTC",
        //"truncated":false,
        //"display_text_range":[0, 226],
        //"entities":{
        //    "hashtags":[],
        //    "symbols":
        //        [{"text":"MTL", "indices":[74, 78]
        //    }, 
        //        {"text":"DGB", "indices":[79, 83]
        //}, 
        //        {"text":"XEM", "indices":[84, 88]}, 
        //        {"text":"TRIG", "indices":[89, 94]}, 
        //        {"text":"STRAT", "indices":[95, 101]}, 
        //        {"text":"LSK", "indices":[102, 106]}, 
        //        {"text":"IOP", "indices":[107, 111]}, 
        //        {"text":"PIVX", "indices":[112, 117]}, 
        //        {"text":"PAY", "indices":[118, 122]}, 
        //        {"text":"ARK", "indices":[123, 127]}, 
        //        {"text":"DASH", "indices":[128, 133]}, 
        //        {"text":"MCO", "indices":[134, 138]}, 
        //        {"text":"ARDR", "indices":[139, 144]}, 
        //        {"text":"CVC", "indices":[145, 149]}, 
        //        {"text":"ETH", "indices":[150, 154]}, 
        //        {"text":"NEO", "indices":[155, 159]}, 
        //        {"text":"XLM", "indices":[160, 164]}, 
        //        {"text":"XRP", "indices":[165, 169]}, 
        //        {"text":"ADA", "indices":[170, 174]}, 
        //        {"text":"OMG", "indices":[181, 185]}, 
        //        {"text":"BCC", "indices":[186, 190]}, 
        //        {"text":"VTC", "indices":[191, 195]}, 
        //        {"text":"AEON", "indices":[196, 201]}, 
        //        {"text":"ZEC", "indices":[202, 206]}, 
        //        {"text":"ADX", "indices":[207, 211]}, 
        //        {"text":"XVG", "indices":[212, 216]}, 
        //        {"text":"BAT", "indices":[217, 221]}, 
        //        {"text":"LTC", "indices":[222, 226]}],
        //    "user_mentions":[],
        //    "urls": [{
        //        "url":"https://t.co/azARNloQdz",
        //        "expanded_url":"http://t.me/Monsterpumper",
        //        "display_url":"t.me/Monsterpumper",
        //        "indices":[49, 72]}]}, 
        //        "metadata":{
        //            "iso_language_code":"en", 
        //            "result_type":"recent"
        //        }, 
        //        "source": "<a href=\"http://twittbot.net/\" rel=\"nofollow\">twittbot.net</a>", 
        //        "in_reply_to_status_id":null, 
        //        "in_reply_to_status_id_str":null, 
        //        "in_reply_to_user_id":null, 
        //        "in_reply_to_user_id_str":null, 
        //        "in_reply_to_screen_name":null, 
        //        "user":{
        //            "id":923155096619311109, 
        //            "id_str":"923155096619311109", 
        //            "name":"cryptopower", 
        //            "screen_name":"cryptopower5", 
        //            "location":"", 
        //            "description":"", 
        //            "url":null, 
        //            "entities": {
        //                "description":
        //                    { 
        //                        "urls":[]
        //                    }
        //             }, 
        //             "protected":false, 
        //             "followers_count":76, 
        //             "friends_count":48, 
        //             "listed_count":0, 
        //             "created_at":"Wed Oct 25 11:51:41 +0000 2017", 
        //             "favourites_count":1, 
        //             "utc_offset":null, 
        //             "time_zone":null, 
        //             "geo_enabled":false, 
        //             "verified":false, 
        //             "statuses_count":4307, 
        //             "lang":"en", 
        //             "contributors_enabled":false, 
        //             "is_translator":false, 
        //             "is_translation_enabled":false, 
        //             "profile_background_color":"F5F8FA", 
        //             "profile_background_image_url":null, 
        //             "profile_background_image_url_https":null, 
        //             "profile_background_tile":false, 
        //             "profile_image_url":"http://pbs.twimg.com/profile_images/929753440510558209/1Y7lvBsR_normal.jpg", 
        //             "profile_image_url_https":"https://pbs.twimg.com/profile_images/929753440510558209/1Y7lvBsR_normal.jpg", 
        //             "profile_link_color":"1DA1F2", 
        //             "profile_sidebar_border_color":"C0DEED", 
        //             "profile_sidebar_fill_color":"DDEEF6", 
        //             "profile_text_color":"333333", 
        //             "profile_use_background_image":true, 
        //             "has_extended_profile":false, 
        //             "default_profile":true, 
        //             "default_profile_image":false, 
        //             "following":null, 
        //             "follow_request_sent":null, 
        //             "notifications":null, 
        //             "translator_type":"none"
        //        }, 
        //        "geo":null, 
        //        "coordinates":null, 
        //        "place":null, 
        //        "contributors":null, 
        //        "is_quote_status":false, 
        //        "retweet_count":0, 
        //        "favorite_count":0, 
        //        "favorited":false, 
        //        "retweeted":false, 
        //        "possibly_sensitive":false, 
        //        "lang":"en"
        //    }]
        //}
        public static string GetSentiment(string q){
            var url = "https://www.csc2.ncsu.edu/faculty/healey/tweet_viz/tweet_app/php/recent.php?q=" + System.Uri.EscapeDataString(q) + "&pg=3";
            var request = WebRequest.Create(url);

            WebResponse response = request.GetResponse();
            var dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            return responseFromServer;
        }


        public static TwitterHashtagStats GetHashtagData(string hashtag)
        {
            var Twitter = new TwitterService(TWITTER_CONSUMER_KEY, TWITTER_CONSUMER_SECRET);
            Twitter.AuthenticateWith(TWITTER_ACCESS_TOKEN, TWITTER_ACCESS_TOKEN_SECRET);

            var Stats = new TwitterHashtagStats();

            var Count = 1;
            var FavoriteCount = 0;
            var RetweetCount = 0;

            var tweets_search = Twitter.Search( new SearchOptions {
                Q = "$" + hashtag,
                Resulttype = TwitterSearchResultType.Recent,
                Count = 200
            });
             
            List<TwitterStatus> resultList = new List<TwitterStatus>(tweets_search.Statuses);
            foreach (var tweet in tweets_search.Statuses)
            {
                try
                {
                    var Status = new Tweet()
                    {
                        CreatedDate = tweet.CreatedDate,
                        FavouritesCount = tweet.User.FavouritesCount.ToString(),
                        Id = tweet.Id.ToString(),
                        Name = tweet.User.Name,
                        ProfileImageUrl = tweet.User.ProfileImageUrl,
                        RetweetCount = tweet.RetweetCount.ToString(),
                        ScreenName = tweet.User.ScreenName,
                        Text = tweet.Text,
                    }; 

                    Stats.Tweets.Add(Status);
                    RetweetCount += tweet.RetweetCount;
                    FavoriteCount += tweet.User.FavouritesCount;
                    Count++;
                }
                catch { }
            }
            Stats.RetweetCount = RetweetCount.ToString();
            Stats.Count = Count.ToString();
            Stats.FavoriteCount = FavoriteCount.ToString();

            return Stats;
        }
    }
}
