using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Devlord.Utilities.MapsApi
{
    [Serializable]
    public class DistanceResults
    {
        public DistanceResults()
        {
            DestinationAddresses = new List<string>();
            OriginAddresses = new List<string>();
            Rows = new List<ElementRow>();
        }

        public DistanceElement GetResult(int index)
        {
            return Rows.ElementAt(0).Elements.ElementAt(index);
        }

        public ICollection<string> DestinationAddresses { get; set; }

        public ICollection<string> OriginAddresses { get; set; }

        public ICollection<ElementRow> Rows { get; set; }

        public string ToJson()
        {
            string json = JsonConvert.SerializeObject(
                this,
                Formatting.Indented,
                Settings);
            return json;
        }

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver =
                new UnderscoreContractResolver()
        };

        public static DistanceResults FromJson(string json)
        {
            return JsonConvert.DeserializeObject<DistanceResults>(json, Settings);
        }
    }
}