using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Likja.Tid
{
    public class TidConfig
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("entries")]
        public List<TidEntry> Entries { get; set; }

        public TidEntry GetEntryByIdNumber(string id)
        {
            var codeId = string.Format("{0}-{1}", Code, id);
            return Entries.FirstOrDefault(x => x.Id == codeId);
        }

    }

   
}
