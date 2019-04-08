using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider.Scheduler.Services
{
    public class FakeBasicDataService : IBasicDataService
    {
        public async Task<List<string>> FetchLanguageListAsync()
        {
            var languages = new List<string>() { "Java", "Python" };
            await Task.Delay(1000);
            return languages; ;
        }
    }
}
