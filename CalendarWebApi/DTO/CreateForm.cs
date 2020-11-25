using System;
using Newtonsoft.Json;
namespace CalendarWebApi.DTO
{
    public class CreateForm
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("eventOrganizer")]
        public string EventOrganizer { get; set; }

        [JsonProperty("members")]
        public string Members { get; set; }
    }
}
