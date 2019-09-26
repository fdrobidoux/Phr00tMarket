using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace Phr00tMarket.Models
{
    [Serializable]
    public class XenkoCommit
    {
        public XenkoCommit()
        { }

        public Octokit.GitHubCommit Xen2Commit { get; set; }
        public Octokit.GitHubCommit Phr00tCommit { get; set; }

        public String Title { get; set; }
        public DateTime Date { get; set; }
    }
}
