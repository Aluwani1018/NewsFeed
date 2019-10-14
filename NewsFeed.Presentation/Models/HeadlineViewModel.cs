using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Presentation.Models
{
    public class HeadlineViewModel
    {
        public string Status { get; set; }
        public int TotalResults { get; set; }

        public IEnumerable<ArticleViewModel> Articles { get; set; }
    }
}
