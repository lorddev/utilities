using System.Text.Json;

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
            string json = JsonSerializer.Serialize(
                this,
                Settings);
            return json;
        }

        private static readonly JsonSerializerOptions Settings = new JsonSerializerOptions
        {   
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            WriteIndented = true,
        };

        public static DistanceResults? FromJson(string json)
        {
            return JsonSerializer.Deserialize<DistanceResults>(json, Settings);
        }
    }
}