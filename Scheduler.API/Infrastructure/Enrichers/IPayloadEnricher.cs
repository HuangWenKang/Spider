using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.API.Infrastructure.Converters
{
    public interface IPayloadEnricher
    {
        T Convert<T>(string jsonPayload);

        string AppendLanguages(string jsonPayload, IList<string> languages);

        string AppendTags(string jsonPayload, IList<string> tags);
    }
}
