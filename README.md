devlord utilities
====================


A useful class library for universal utilities like encryption, sending emails, and consuming RESTful APIs.

| Master      | Develop      | NuGet      | Slack     |
| -----       | -----        | -----      |------     |
| [![Build status](https://ci.appveyor.com/api/projects/status/i0us4v5jxi6llk3e/branch/master?svg=true)](https://ci.appveyor.com/project/lorddev/utilities/branch/master) | [![Build status](https://ci.appveyor.com/api/projects/status/i0us4v5jxi6llk3e/branch/develop?svg=true)](https://ci.appveyor.com/project/lorddev/utilities/branch/develop) | [![NuGet Package](https://buildstats.info/nuget/Devlord.Utilities)](https://www.nuget.org/packages/Devlord.Utilities/) | [![Join the team on Slack](https://slack-devlords-org.herokuapp.com/badge.svg)](https://slack-devlords-org.herokuapp.com/)

To install Devlord.Utilities, run the following command in the Package Manager Console:

    PM> Install-Package Devlord.Utilities

### Feature Summary

* .NET Core compatibility
* System.Threading.Timers service clocks
* Email throttling to help you avoid being suspended by your ISP
* Generic HTTP API wrappers
* Two-way AES encryption
* "Micro-ORM" DataReader mapper for when using a full-featured ORM would be overkill

More details below, but for even _more_ details, see the [project wiki](https://github.com/lorddev/utilities/wiki).

### Service Timers

We've provided 3 types of timers for back-end services to execute operations 
* At a certain time
* At a certain regular interval
* Continuously, repeating an action as soon as the previous action is complete

### ApiCall

The `ApiCall` class wraps the .NET HttpClient, and returns a deserialized object using Generics. It also features a Dictionary 
for query parameter input. The `WebApiCall` subclass will build your endpoint for ASP.NET MVC Web API based on the supplied 
controller, action, and id as input parameters.

### Distance API

POCO classes for accessing the Google Maps distance API.

Usage with a custom JSON contract resolver that converts the Google JSON property names to POCO object properties.

```csharp
    const string BaseUri = "https://maps.googleapis.com/maps/api/distancematrix/json";

    using (IApiCall client = new ApiCall(BaseUri,
            new JsonSerializerSettings { ContractResolver = new UnderscoreContractResolver() }))
    {
        client.QueryParams.Add("sensor", "false");
        client.QueryParams.Add("origins", "95969");
        client.QueryParams.Add("destinations", "95928");
        IApiResult<dynamic> result = client.Execute<DistanceResults>();
    }
```

### DataManager

A handy generic DataManager abstract base class for your business layer that serves to enforce a CRUD contract between your 
business entities and your data layer. Useful for pagination as well.

### Mailbot

A multithreaded SMTP queued mail sender, has a throttle based on Google Apps maximums for mail frequency sent by a single account.

### Crypt

Bidirectional encryption methods useful for storing credit cards or encrypting passwords or API keys to save in a project's
configuration files. (Not recommend for use with user's site credentials as those should use one-way encryption.)

### RssConverter

This class makes it easy to download an RSS feed and parse it. It reads the data with LinqToXml and can output it in JSON format 
for easy portability. 
You can utilize the `PostFilter` delegate to filter the results even further. (Sorry, I can't remember what project I needed this for.)

### DRMapper

Allows mapping of an `IDataReader` to POCO classes using reflection. It's faster than you might expect.

### Pagination

```csharp
    var query = from a in context.Addresses where a.IsActive select a;
    var results = query.GetPage(pageNumber, pageSize); 
```

### Contributing

Yes, please!

### License

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published
by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.

Contact [lorddev](https://github.com/lorddev) on GitHub or [@devlords](https://twitter.com/devlords) on Twitter.
