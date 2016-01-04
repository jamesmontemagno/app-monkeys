using System.Collections.Generic;

using MonkeysApp.Adapters;
using MonkeysApp.Helpers;
using Newtonsoft.Json;

namespace MonkeysApp.Models
{
    public class Monkey
    {

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Location")]
        public string Location { get; set; }

        [JsonProperty("Details")]
        public string Details { get; set; }

        [JsonProperty("Image")]
        public string Image { get; set; }

        [JsonProperty("Population")]
        public int Population { get; set; }
    }

    public class MonkeyCollection : List<Monkey>
    {
    }
}