using System.Linq;
using System.Net;
using System.Net.Http;
using LordDesign.Utilities;
using LordDesign.Utilities.MapsApi;
using Newtonsoft.Json;
using NUnit.Framework;

namespace LordDesign.Tests
{
    [TestFixture]
    public class DistanceApiTests
    {
        #region Public Methods and Operators

        [Test]
        public void ReturnsDeserializedResults()
        {
            const string EndPoint =
                "https://maps.googleapis.com/maps/api/distancematrix/json?sensor=false&origins=95969&destinations=95928";
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(EndPoint).Result;

                if (response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.OK)
                {
                    Assert.Fail("No response content");
                }

                Assert.IsNotNull(response.Content);

                string content = response.Content.ReadAsStringAsync().Result;

                var settings = new JsonSerializerSettings { ContractResolver = new UnderscoreContractResolver() };

                var parsedObject = JsonConvert.DeserializeObject<DistanceResults>(content, settings);

                Assert.AreEqual(typeof(DistanceElement), parsedObject.Rows.ElementAt(0).Elements.ElementAt(0).GetType());

                Assert.AreEqual("25 mins", parsedObject.GetResult(0).Duration.Text);
            }

            Assert.Pass();
        }

        [Test]
        public void ReturnsExpectedResultWithCustomApi()
        {
            const string EndPoint =
                "https://maps.googleapis.com/maps/api/distancematrix/json?sensor=false&origins=95969&destinations=95928";
          
            using (IApiCall client = new ApiCall(EndPoint, new JsonSerializerSettings { ContractResolver = new UnderscoreContractResolver() }))
            {
                IApiResult<dynamic> result = client.Execute<DistanceResults>();
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
                Assert.AreEqual("25 mins", result.DataItem.GetResult(0).Duration.Text);
            }
        }

        [Test]
        public void ReturnsResultWithCustomApiAndQueryParams()
        {
            const string BaseUri =
                "https://maps.googleapis.com/maps/api/distancematrix/json";

            // sensor=false&origins=95969&destinations=95928
            using (IApiCall client = new ApiCall(BaseUri, new JsonSerializerSettings { ContractResolver = new UnderscoreContractResolver() }))
            {
                client.QueryParams.Add("sensor", "false");
                client.QueryParams.Add("origins", "95969");
                client.QueryParams.Add("destinations", "95928");
                IApiResult<dynamic> result = client.Execute<DistanceResults>();
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
                Assert.AreEqual("25 mins", result.DataItem.GetResult(0).Duration.Text);
            }
        }

        /// <summary>
        ///     Given the following input:
        ///     https://maps.googleapis.com/maps/api/distancematrix/json?sensor=false&origins=95969&destinations=95928
        ///     ...
        /// </summary>
        [Test]
        public void ReturnsJsonResults()
        {
            const string EndPoint =
                "https://maps.googleapis.com/maps/api/distancematrix/json?sensor=false&origins=95969&destinations=95928";
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(EndPoint).Result;

                if (response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.OK)
                {
                    Assert.Fail("No response content");
                }

                Assert.IsNotNull(response.Content);

                string content = response.Content.ReadAsStringAsync().Result;

                // Assume we know how to deserialize the object.
                string expected = @"{
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

                Assert.AreEqual(expected, content);
            }

            Assert.Pass();
        }

        #endregion
    }
}