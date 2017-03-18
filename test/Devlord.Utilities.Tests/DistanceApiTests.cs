using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Devlord.Utilities.MapsApi;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Devlord.Utilities.Tests
{
    [TestFixture]
    public class DistanceApiTests
    {
        private static double ParseDuration(string distanceString)
        {
            var resultDuration = Regex.Match(distanceString, @"[\d\.]+(?=\smins)")
                .Captures[0]
                .Value;

            return double.Parse(resultDuration);
        }

        [Test]
        public void ReturnsDeserializedResults()
        {
            const string EndPoint =
                "https://maps.googleapis.com/maps/api/distancematrix/json?sensor=false&origins=95969&destinations=95928";
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(EndPoint).Result;

                if (response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.OK)
                {
                    Assert.Fail("No response content");
                }

                Assert.IsNotNull(response.Content);

                var content = response.Content.ReadAsStringAsync().Result;

                var settings = new JsonSerializerSettings { ContractResolver = new UnderscoreContractResolver() };

                var parsedObject = JsonConvert.DeserializeObject<DistanceResults>(content, settings);

                Assert.AreEqual(typeof(DistanceElement),
                    parsedObject.Rows.ElementAt(0).Elements.ElementAt(0).GetType());

                var resultDuration = ParseDuration(parsedObject.GetResult(0).Duration.Text);
                resultDuration.ShouldBeInRange(20, 30);
            }

            Assert.Pass();
        }

        [Test]
        public void ReturnsExpectedResultWithCustomApi()
        {
            const string endPoint =
                "https://maps.googleapis.com/maps/api/distancematrix/json?sensor=false&origins=95969&destinations=95928";

            using (IApiCall client = new ApiCall(endPoint,
                new JsonSerializerSettings { ContractResolver = new UnderscoreContractResolver() }))
            {
                var result = client.Execute<DistanceResults>();
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

                var resultDuration = ParseDuration((string) result.DataItem.GetResult(0).Duration.Text);
                resultDuration.ShouldBeInRange(20, 30);
            }
        }

        /// <summary>
        /// Given the following input:
        /// https://maps.googleapis.com/maps/api/distancematrix/json?sensor=false&origins=95969&destinations=95928
        /// ...
        /// </summary>
        [Test]
        public void ReturnsJsonResults()
        {
            const string endPoint =
                "https://maps.googleapis.com/maps/api/distancematrix/json?sensor=false&origins=95969&destinations=95928";
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(endPoint).Result;

                if (response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.OK)
                {
                    Assert.Fail("No response content");
                }

                Assert.IsNotNull(response.Content);

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

                //Assert.AreEqual(expected, content);

                Assert.Inconclusive(@"It's showing s a 27-minute drive instead of 25. We need to update 
this comparison to allow for variations in travel time.");
            }

            Assert.Pass();
        }

        [Test]
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
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

                var resultDuration = ParseDuration((string) result.DataItem.GetResult(0).Duration.Text);
                resultDuration.ShouldBeInRange(20, 30);
            }
        }
    }
}