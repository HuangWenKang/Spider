using System.Collections.Generic;
using Newtonsoft.Json;
using Scheduler.API.Models;
using System.Linq;

namespace Scheduler.API.Infrastructure.Converters
{
    public class PayloadEnricher : IPayloadEnricher
    {
        public string AppendLanguages(string jsonPayload, IList<string> languages)
        {
            GithubForSync githubForSync = JsonConvert.DeserializeObject<GithubForSync>(jsonPayload);
            githubForSync.Languages.AddRange(languages);
            return JsonConvert.SerializeObject(githubForSync);
        }

        public string AppendTags(string jsonPayload, IList<string> tags)
        {
            MSDNForSync githubForSync = JsonConvert.DeserializeObject<MSDNForSync>(jsonPayload);
            githubForSync.Tags.AddRange(tags);
            return JsonConvert.SerializeObject(githubForSync);
        }

        public T Convert<T>(string jsonPayload)
        {
            return JsonConvert.DeserializeObject<T>(jsonPayload);
        }        
    }
}
