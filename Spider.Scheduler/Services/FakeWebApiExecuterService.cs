using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider.Scheduler.Services
{
    public class FakeWebApiExecuterService
    {
        public async Task CallWebApiAsync(string webApiUrl, string payload)
        {
            Console.WriteLine("CallWebApiAsync");
            await Task.Delay(1000);
        }
    }
}
