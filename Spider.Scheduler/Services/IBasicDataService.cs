using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider.Scheduler.Services
{
    public interface IBasicDataService
    {
        Task<List<string>> FetchLanguageListAsync();
    }
}
