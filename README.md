lorddev utilities
====================

This is a useful class library for universal utilities such as error logging, email sending, and web service wrappers.

### Dependencies

- ELMAH
- JSON.NET
- [Encryptamajig](https://github.com/jbubriski/Encryptamajig) 

### ApiCall

The `ApiCall` class wraps the .NET HttpClient, and returns a deserialized object using Generics. It also features a Dictionary for query parameter input. The `WebApiCall` subclass will build your endpoint for ASP.NET MVC Web API based on the supplied controller, action, and id as input parameters.

### Logger

This class simply wraps the basic ELMAH exception logger and will log to the elmah.axd source if the current context is an http application, and to a file if it is not. This is useful for business logic layers in which a class's usage may be over http or in a service.

#### Usage

    using Devlord.Utilities;
    
    public class MyClass
    {
        public void DoMyThing()
        {
            try
            {
                // Todo: Do something...
            }
            catch (Exception e)
            {
                Logger.Log(e);
            }
        }
    }

Now you can do all of your exception logging with just one simple line of code.

### Interfaces

`IPaginable` - provides an interface indicating that the implementing collection class will perform the proper Skip/Take functions to return a certain page of the results.

`IEachified` - Indicates that a collection can perform a delegate on each item in its collection apart from the mission IEnumerable<T>.ForEach() (for which I also have an extension in this library in `Extensions.cs`)

### Distance API

POCO classes for accessing the Google Maps distance API.

Usage with a custom JSON contract resolver that converts the Google JSON property names to POCO object properties.

    const string BaseUri = "https://maps.googleapis.com/maps/api/distancematrix/json";

    using (IApiCall client = new ApiCall(BaseUri,
            new JsonSerializerSettings { ContractResolver = new UnderscoreContractResolver() }))
    {
        client.QueryParams.Add("sensor", "false");
        client.QueryParams.Add("origins", "95969");
        client.QueryParams.Add("destinations", "95928");
        IApiResult<dynamic> result = client.Execute<DistanceResults>();
        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual("25 mins", result.DataItem.GetResult(0).Duration.Text);
    }

### DataManager

A handy generic DataManager abstract base class for your business layer that serves to enforce a CRUD contract between your business entities and your data layer. Useful for pagination as well.

### Mailbot

A multithreaded SMTP queued mail sender, has a throttle based on Google Apps maximums for mail frequency received at a single account. Important: Create a test project and call `Crypt.HideSecretPassword()` using the same byte array used in the Mailbot class. Store the resulting encrypted password in the config file, at `Devlord.Utilities.Properties.Settings/SmtpPassword`.

### Crypt

Bi-directional encryption methods useful for storing credit cards or saving passwords in configuration files. (Not recommend for use with user's site credentials as those should be one-way, and this encryption is random so two identical strings when encrypted will never match. By design.)

### RssConverter

This class makes it easy to download an RSS feed and parse it. It reads the data with LinqToXml and can output it in JSON format for easy portability. You can utilize the `PostFilter` delegate to filter the results even further.

### RestRouteHandler

A REST route handler that can be used in Global.asax to convert a REST url request to a query-string name-value pair on the server side. (I will probably need to provide examples for its usage.)

### License

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.

Contact Aaron Lord: "lorddev" on GitHub or "devlords" on Twitter.