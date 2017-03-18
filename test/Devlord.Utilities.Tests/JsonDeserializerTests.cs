using System.Linq;
using System.Text.RegularExpressions;
using Devlord.Utilities;
using Devlord.Utilities.MapsApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Devlord.Utilities.Tests
{
    [TestFixture]
    public class JsonDeserializerTests
    {
        #region Public Methods and Operators

        [Test]
        public void TestCustomResolver()
        {
            var settings = new JsonSerializerSettings { ContractResolver = new UnderscoreContractResolver() };

            var bob = new { PropertyName = "asdf" };
            string json = JsonConvert.SerializeObject(bob, Formatting.Indented, settings);

            Assert.AreEqual("{\r\n  \"property_name\": \"asdf\"\r\n}", json);
        }

        [Test]
        public void TestDeserializeDynamic()
        {
            string input = @"{
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

            var settings = new JsonSerializerSettings { ContractResolver = new UnderscoreContractResolver() };

            dynamic parsedObject = JsonConvert.DeserializeObject(input, settings);

            Assert.AreEqual(typeof(JObject), parsedObject.rows[0].elements[0].GetType());
        }

        [Test]
        public void TestDeserializeStrongTyped()
        {
            string input = @"{
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

            var settings = new JsonSerializerSettings { ContractResolver = new UnderscoreContractResolver() };

            var parsedObject = JsonConvert.DeserializeObject<DistanceResults>(input, settings);

            Assert.AreEqual(typeof(DistanceElement), parsedObject.Rows.ElementAt(0).Elements.ElementAt(0).GetType());

            Assert.AreEqual("25 mins", parsedObject.GetResult(0).Duration.Text);
        }

        [Test]
        public void TestRegexProofs()
        {
            var reg = new Regex("\\s[A-Z]");
            string test = "Hello World";
            string result = reg.Replace(test, m => m.Value.ToLower());
            Assert.AreEqual("Hello world", result);

            reg = new Regex("\\s[A-Z]");
            test = "Hello World";
            result = reg.Replace(test, m => m.Value.ToLower());
            Assert.AreEqual("Hello world", result);

            reg = new Regex("\\s[A-Z]");
            test = "Hello World";
            result = reg.Replace(test, m => "_" + m.Value.Trim().ToLower());
            Assert.AreEqual("Hello_world", result);

            reg = new Regex("\\s[A-Z]");
            test = "Hello World";
            MatchCollection matches = reg.Matches(test);
            Assert.AreEqual(1, matches.Count);

            reg = new Regex("\\s?[A-Z]");
            test = "Hello World";
            matches = reg.Matches(test);
            Assert.AreEqual(2, matches.Count);

            reg = new Regex("(?<=[a-z])[A-Z]");
            test = "HelloWorld";
            result = reg.Replace(test, m => "_" + m.Value.ToLower());
            Assert.AreEqual("Hello_world", result);
        }

        #endregion
    }
}