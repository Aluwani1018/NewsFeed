using System;
using System.Collections.Generic;
using System.Text;

namespace NewsFeed.Core.Models
{
    public class Headlines
    {
        public string Status { get; set; }
        public int TotalResults { get; set; }

        public IEnumerable<Article> Articles { get; set; }
    }
}
