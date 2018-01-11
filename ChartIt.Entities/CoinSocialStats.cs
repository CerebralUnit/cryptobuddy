using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
namespace ChartIt.Entities
{
    public class CoinSocialStats
    {
        public long FacebookLikes { get; set; }

        public string FacebookLink { get; set; }

        public string FacebookIsClosed { get; set; }

        public string FacebookTalkingAbout { get; set; }

        public string FacebookName { get; set; }

        public long FacebookPoints { get; set; }

        public string Name { get; set; }

        public string CoinName { get; set; }

        public string Type { get; set; }

        public string RedditPostsPerHour { get; set; }

        public string RedditCommentsPerHour { get; set; }

        public string RedditPostsPerDay { get; set; }

        public double RedditCommentsPerDay { get; set; }

        public string RedditName { get; set; }

        public string RedditLink { get; set; }

        public long RedditActiveUsers { get; set; }

        public string RedditCommunityCreation { get; set; }

        public long RedditSubscribers { get; set; }

        public long RedditPoints { get; set; } 

        public string TwitterFollowing { get; set; }

        public string TwitterAccountCreation { get; set; }

        public string TwitterName { get; set; }

        public long TwitterLists { get; set; }

        public long TwitterStatuses { get; set; }

        public string TwitterFavorites { get; set; }

        public long TwitterFollowers { get; set; }

        public string TwitterLink { get; set; }

        public long TwitterPoints { get; set; }

        public List<CodeRepo> CodeRepos { get; set; }

        public long CodeRepoPoints { get; set; }
    }
 
    public partial class CodeRepo
    {
        public string CreatedAt { get; set; }

        public string OpenTotalIssues { get; set; }
 
        public string Size { get; set; }

        public string ClosedTotalIssues { get; set; }

        public long Stars { get; set; }

        public string LastUpdate { get; set; }

        public long Forks { get; set; }

        public string Url { get; set; }

        public string ClosedIssues { get; set; }

        public string ClosedPullIssues { get; set; }

        public string Fork { get; set; }

        public string LastPush { get; set; } 

        public string OpenPullIssues { get; set; }

        public string Language { get; set; }

        public long Subscribers { get; set; }

        public string OpenIssues { get; set; }

        public string CodeRepoParentName { get; set; }

        public string CodeRepoParentUrl { get; set; }

        public long CodeRepoParentInternalId { get; set; }
    } 
}
