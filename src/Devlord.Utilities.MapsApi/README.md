Devlord.Utilities.MapsApi
====================


A useful class library for universal utilities like sending emails and consuming RESTful APIs.


| Build       | NuGet      |
| -----       | -----      |
| [![.NET](https://github.com/lorddev/utilities/actions/workflows/dotnet.yml/badge.svg)](https://github.com/lorddev/utilities/actions/workflows/dotnet.yml) | [NuGet](https://www.nuget.org/packages/Devlord.Utilities.MapsApi/) |

To install Devlord.Utilities.MapsApi, run the following command in the Package Manager Console:

    PM> Install-Package Devlord.Utilities.MapsApi

Or from the command-line:

    dotnet add package Devlord.Utilities.MapsApi

### Changes from Devlord.Utilities 6.0

* UnderscoreContractResolver has been removed in favor of System.Text.Json snake case handling.
* ValueText.Value has been changed from string to decimal.

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

**Configuration**

```json
{
  "Devlord.Utilities": {
    "GoogleMapsApiKey": "",
  }
}
```

### Contributing

Yes, please!

### License

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published
by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.

Contact [lorddev](https://github.com/lorddev) on GitHub or [@devlord@hachyderm.io](https://hachyderm.io/@devlord) on Mastodon.
