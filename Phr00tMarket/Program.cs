using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace Phr00tMarket
{
    public class Program
    {
        static GitHubClient client;

        // Notice that, instead of `void`, I use `async Task` for my main function: This works only with C# 7.1 and up!
        public static async Task Main(string[] args)
        {
            // Testing Octokit.
            client = new GitHubClient(new ProductHeaderValue("phr00t-market"));
            var basicAuth = new Credentials("fdrobidoux", Properties.Resources.SECRET); // NOTE: not real credentials
            client.Credentials = basicAuth;

            var procedures = await GetRateLimits()
                .ContinueWith((x) => { Console.WriteLine("Press any key to start..."); Console.ReadKey(); })
                .ContinueWith((x) => ObtainCommitsByXen2());

            procedures.Wait();
        }

        static async Task<List<Commit>> ObtainCommitsByXen2()
        {
            List<Commit> commits = new List<Commit>();
            IReadOnlyList<GitHubCommit> repoXen2;
            IReadOnlyList<GitHubCommit> repoPhr00t;

            repoXen2 = await client.Repository.Commit.GetAll("xenko3d", "xenko");
            await GetRateLimits();
            repoPhr00t = await client.Repository.Commit.GetAll("phr00t", "xenko");
            await GetRateLimits();

            foreach (GitHubCommit ghc in repoXen2)
            {
                // TODO

            }

            return new List<Commit>();
        }

        static async Task GetRateLimits()
        {
            StringBuilder builder = new StringBuilder( "--- CURRENT LIMITS ---");
            int cursorX = Console.CursorLeft,
                cursorY = Console.CursorTop;

            var miscellaneousRateLimit = await client.Miscellaneous.GetRateLimits();

            //  The "core" object provides your rate limit status except for the Search API.
            var coreRateLimit = miscellaneousRateLimit.Resources.Core;
            var howManyCoreRequestsCanIMakePerHour = coreRateLimit.Limit;
            var howManyCoreRequestsDoIHaveLeft = coreRateLimit.Remaining;
            var whenDoesTheCoreLimitReset = coreRateLimit.Reset.ToLocalTime(); // UTC time

            // the "search" object provides your rate limit status for the Search API.
            var searchRateLimit = miscellaneousRateLimit.Resources.Search;
            var howManySearchRequestsCanIMakePerMinute = searchRateLimit.Limit;
            var howManySearchRequestsDoIHaveLeft = searchRateLimit.Remaining;
            var whenDoesTheSearchLimitReset = searchRateLimit.Reset.ToLocalTime(); // UTC time

            builder.AppendLine("### Core rate limit")
                .AppendLine($"Requests available per hour = {howManyCoreRequestsCanIMakePerHour}")
                .AppendLine($"Requests remaining = {howManyCoreRequestsDoIHaveLeft}")
                .AppendLine($"When it will reset = {whenDoesTheCoreLimitReset}")
                .AppendLine()
                .AppendLine("### Search rate limit")
                .AppendLine($"Search requests available per minute = {howManySearchRequestsCanIMakePerMinute}")
                .AppendLine($"Search requests left = {howManySearchRequestsDoIHaveLeft}")
                .AppendLine($"When it will reset = {whenDoesTheSearchLimitReset}")
                .AppendLine()
                .AppendLine("---")
                .AppendLine("");

            Console.SetCursorPosition(0, 0);
            Console.Write(builder.ToString());
            Console.SetCursorPosition(cursorX, cursorY);
        }
    }
}
