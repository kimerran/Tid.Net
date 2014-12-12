using Newtonsoft.Json;

namespace Likja.Tid
{
    public class TidEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("details")]
        public string Details { get; set; }
        
        [JsonProperty("status")]
        public EntryStatus Status { get; set; }
    }
}
