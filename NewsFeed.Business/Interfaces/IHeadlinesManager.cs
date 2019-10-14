using NewsFeed.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsFeed.Business.Interfaces
{
    public interface IHeadlinesManager
    {
        Task<Headlines> GetNewsHeadlineAsync();
    }
}
