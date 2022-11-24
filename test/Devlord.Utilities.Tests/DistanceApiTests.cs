using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Devlord.Utilities.MapsApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace Devlord.Utilities.Tests
{
    public class DevlordTestConfiguration
    {
        public DevlordSettings Settings { get; private set; }
        
        public DevlordTestConfiguration(DevlordSettings settings)
        {
            Settings = settings;
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddUserSecrets<DistanceApiTests>()
                .Build();
            var services = new ServiceCollection().AddOptions();
            services.Configure<DevlordSettings>(config.GetSection("Devlord.Utilities"));
            Settings = services.BuildServiceProvider().GetService<DevlordSettings>();
            //return config;
        }
    }
    
    public class DistanceApiTests : IClassFixture<DevlordTestConfiguration>
    {
        private readonly DevlordSettings _settings;
        
        public DistanceApiTests(DevlordTestConfiguration settings)
        {
            _settings = settings.Settings;
        }
        
        private static double ParseDuration(string distanceString)
        {
            var resultDuration = Regex.Match(distanceString, @"[\d\.]+(?=\smins)")
                .Captures[0]
                .Value;

            return double.Parse(resultDuration);
        }

        [Fact]
        public void ReturnsDeserializedResults()
        {
            var endPoint =
                "https://maps.googleapis.com/maps/api/distancematrix/json?sensor=false&origins=95969&destinations=95928&key="
                + _settings.GoogleMapsApiKey;
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(endPoint).Result;

                if (response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.OK)
                {
                    Assert.True(false, "No response content");
                }

                Assert.NotNull(response.Content);

                var content = response.Content.ReadAsStringAsync().Result;

                var settings = new JsonSerializerSettings { ContractResolver = new UnderscoreContractResolver() };

                var parsedObject = JsonConvert.DeserializeObject<DistanceResults>(content, settings);

                Assert.Equal(typeof(DistanceElement),
                    parsedObject.Rows.ElementAt(0).Elements.ElementAt(0).GetType());

                var resultDuration = ParseDuration(parsedObject.GetResult(0).Duration.Text);
                resultDuration.ShouldBeInRange(20, 30);
            }
        }

        [Fact]
        public void ReturnsExpectedResultWithCustomApi()
        {
            const string endPoint =
                "https://maps.googleapis.com/maps/api/distancematrix/json?sensor=false&origins=95969&destinations=95928";

            using (IApiCall client = new ApiCall(endPoint,
                new JsonSerializerSettings { ContractResolver = new UnderscoreContractResolver() }))
            {
                var result = client.Execute<DistanceResults>();
                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultDuration = ParseDuration((string) result.DataItem.GetResult(0).Duration.Text);
                resultDuration.ShouldBeInRange(20, 30);
            }
        }

        /// <summary>
        /// Given the following input:
        /// https://maps.googleapis.com/maps/api/distancematrix/json?sensor=false&amp;origins=95969&amp;destinations=95928
        /// ...
        /// </summary>
        [Fact(Skip = "Run explicitly")]
        public void ReturnsJsonResults()
        {
            const string endPoint =
                "https://maps.googleapis.com/maps/api/distancematrix/json?sensor=false&origins=95969&destinations=95928";
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(endPoint).Result;

                if (response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.OK)
                {
                    Assert.True(false, "No response content");
                }

                Assert.NotNull(response.Content);

                var content = response.Content.ReadAsStringAsync().Result;

                // Assume we know how to deserialize the object.
                var expected = @"{
   ""destination_addresses"" : [ ""Chico, CA 95928, USA"" ],
   ""origin_addresses"" : [ ""Paradise, CA 95969, USA"" ],
   ""rows"" : [
      {
         ""elements"" : [
            {
               ""distance"" : {
                  ""text"" : ""27.7 km"",
                  ""value"" : 27722
               },
               ""duration"" : {
                  ""text"" : ""25 mins"",
                  ""value"" : 1518
               },
               ""status"" : ""OK""
            }
         ]
      }
   ],
   ""status"" : ""OK""
}
".Replace("\r\n", "\n");

                //Assert.Equal(expected, content);

                Assert.True(false, @"It's showing s a 27-minute drive instead of 25. We need to update 
this comparison to allow for variations in travel time.");
            }
        }

        [Fact]
        public void ReturnsResultWithCustomApiAndQueryParams()
        {
            const string baseUri = "https://maps.googleapis.com/maps/api/distancematrix/json";

            // sensor=false&origins=95969&destinations=95928
            using (IApiCall client = new ApiCall(baseUri,
                new JsonSerializerSettings { ContractResolver = new UnderscoreContractResolver() }))
            {
                client.QueryParams.Add("sensor", "false");
                client.QueryParams.Add("origins", "95969");
                client.QueryParams.Add("destinations", "95928");
                var result = client.Execute<DistanceResults>();
                Assert.Equal(HttpStatusCode.OK, result.StatusCode);

                var resultDuration = ParseDuration((string) result.DataItem.GetResult(0).Duration.Text);
                resultDuration.ShouldBeInRange(20, 30);
            }
        }
    }
}