devlord utilities
====================
A useful class library for universal utilities such as error logging, email sending, and web service wrappers.

| Master      | Develop      | Nuget      |
| -----       | -----        | -----      |
| [![Build status](https://ci.appveyor.com/api/projects/status/i0us4v5jxi6llk3e/branch/master?svg=true)](https://ci.appveyor.com/project/lorddev/utilities/branch/master) | [![Build status](https://ci.appveyor.com/api/projects/status/i0us4v5jxi6llk3e/branch/develop?svg=true)](https://ci.appveyor.com/project/lorddev/utilities/branch/develop) | [![NuGet Badge](https://buildstats.info/nuget/Devlord.Utilities)](https://www.nuget.org/packages/Devlord.Utilities/) |

### 5.0 Release notes

* Incremented major version to 5.0 because of breaking changes in .NET Core
    - There is a question of whether these features are truly deprecated or just haven't been finished yet, because I've seen reports
      that some of the features will be added in .NET Core 2.0.

* Changed service timers due to the .NET System.Timers.Timer class having been deprecated. I tried to keep the public interface intact, 
as well as the behavior. But you'll need to change "ElapsedEventArgs" to "ServiceTimerState" in your events.

        private static void LoopedElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Test message ONE");
        }
        // to
        private static void LoopedElapsed(object sender, ServiceTimerState e)
        {
            Console.WriteLine("Test message ONE");
        }
* Added MailKit dependency when using this library in .NET Core.
* Breaking changes to Mailbot class: Use `GetInstance(string smtpServer)` instead of the `Instance` property. Throttles are now smtp-server-specific.
  Also, app.config kind of doesn't work anymore since converting to .NET Core, so let me know if you have a fix.
* Changed WebClient to HttpClient, made RssConvert async.
* Absorbed [Encryptamajig](https://github.com/jbubriski/Encryptamajig) since it didn't appear to be maintained and I needed it updated to .NET Core.
* Note that `RestRouteHandler` is only compatible with .NET v4.5.1. If you are using this feature, make sure you have the right target set.
* Had some trouble with future-compatibility for `app.config`, so we've converted to the newer "ConfigurationBuilder" method. To set your settings for
  items in `Devlord.Utilities.Settings`, copy `devlord.utilities.json` into your project, and set "Copy to Output Directory" to either
  "Copy if newer" or "Copy always".
* `DRMapper.ParseRecord<T>` overload with rowIndex has been deprecated.

### ApiCall

The `ApiCall` class wraps the .NET HttpClient, and returns a deserialized object using Generics. It also features a Dictionary for query parameter 
input. The `WebApiCall` subclass will build your endpoint for ASP.NET MVC Web API based on the supplied controller, action, and id as input parameters.

### Logger

This class simply wraps the basic ELMAH exception logger and will log to the elmah.axd source if the current context is an http application, 
and to a file if it is not. This is useful for business logic layers in which a class's usage may be over http or in a service.

#### Usage

    try
    {
        // Todo: Do something...
    }
    catch (Exception e)
    {
        Logger.Log(e);
    }

Now you can do all of your exception logging with just one simple line of code.

### Interfaces

`IPaginable` - provides an interface indicating that the implementing collection class will perform the proper Skip/Take functions to return a 
certain page of the results.

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

A handy generic DataManager abstract base class for your business layer that serves to enforce a CRUD contract between your business entities
and your data layer. Useful for pagination as well.

### Mailbot

A multithreaded SMTP queued mail sender, has a throttle based on Google Apps maximums for mail frequency sent by a single account.
Important: Create a test project and call `Crypt.HideSecretPassword()` using the same byte array used in the Mailbot class. Store the resulting encrypted password in the config file, at `Devlord.Utilities.Properties.Settings/SmtpPassword`.

### Crypt

Bi-directional encryption methods useful for storing credit cards or encrypting passwords or API keys to save in a project's configuration files. 
This is not recommend for use with user's site credentials as those should be one-way; also, site login credentials often require an encrypted password 
to match a _stored_ encrypted password, and this algorithm doesn't create identical strings every time.

### RssConverter

This class makes it easy to download an RSS feed and parse it. It reads the data with LinqToXml and can output it in JSON format for easy portability. 
You can utilize the `PostFilter` delegate to filter the results even further. (Sorry, I can't remember what project I needed this for.)

### RestRouteHandler

A REST route handler that can be used in Global.asax to convert a REST url request to a query-string name-value pair on the server side. (I will 
probably need to provide examples for its usage.)

### DRMapper

Allows mapping of an `IDataReader` to POCO classes using reflection. It's faster than you might expect.

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
