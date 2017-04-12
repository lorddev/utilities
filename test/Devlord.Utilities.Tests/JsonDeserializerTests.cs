using System.Linq;
using System.Text.RegularExpressions;
using Devlord.Utilities.MapsApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Devlord.Utilities.Tests
{
    public class JsonDeserializerTests
    {
        [Fact]
        public void TestCustomResolver()
        {
            var settings = new JsonSerializerSettings { ContractResolver = new UnderscoreContractResolver() };

            var bob = new { PropertyName = "asdf" };
            var json = JsonConvert.SerializeObject(bob, Formatting.Indented, settings);
            // For macos compatibility
            json.Replace("\r\n", "\n");
            Assert.Equal("{\n  \"property_name\": \"asdf\"\n}", json);
        }

        [Fact]
        public void TestDeserializeDynamic()
        {
            var input = @"{
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

            Assert.Equal(typeof(JObject), parsedObject.rows[0].elements[0].GetType());
        }

        [Fact]
        public void TestDeserializeStrongTyped()
        {
            var input = @"{
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

            Assert.Equal(typeof(DistanceElement), parsedObject.Rows.ElementAt(0).Elements.ElementAt(0).GetType());

            Assert.Equal("25 mins", parsedObject.GetResult(0).Duration.Text);
        }

        [Fact]
        public void TestRegexProofs()
        {
            var reg = new Regex("\\s[A-Z]");
            var test = "Hello World";
            var result = reg.Replace(test, m => m.Value.ToLower());
            Assert.Equal("Hello world", result);

            reg = new Regex("\\s[A-Z]");
            test = "Hello World";
            result = reg.Replace(test, m => m.Value.ToLower());
            Assert.Equal("Hello world", result);

            reg = new Regex("\\s[A-Z]");
            test = "Hello World";
            result = reg.Replace(test, m => "_" + m.Value.Trim().ToLower());
            Assert.Equal("Hello_world", result);

            reg = new Regex("\\s[A-Z]");
            test = "Hello World";
            var matches = reg.Matches(test);
            Assert.Equal(1, matches.Count);

            reg = new Regex("\\s?[A-Z]");
            test = "Hello World";
            matches = reg.Matches(test);
            Assert.Equal(2, matches.Count);

            reg = new Regex("(?<=[a-z])[A-Z]");
            test = "HelloWorld";
            result = reg.Replace(test, m => "_" + m.Value.ToLower());
            Assert.Equal("Hello_world", result);
        }
    }
}