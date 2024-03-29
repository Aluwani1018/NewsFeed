﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NewsFeed.Core.Models
{
    public class Article
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string UrlToImage { get; set; }
        public string PublishedAt { get; set; }
        public string Content { get; set; }
        public Source Source { get; set; }
    }
}
