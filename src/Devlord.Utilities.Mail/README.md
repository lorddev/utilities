Devlord.Utilities.Mail
====================


A useful class library for universal utilities like sending emails and consuming RESTful APIs.

| Master      | Develop      | NuGet      |
| -----       | -----        | -----      |
| [![.NET](https://github.com/lorddev/utilities/actions/workflows/dotnet.yml/badge.svg)](https://github.com/lorddev/utilities/actions/workflows/dotnet.yml) | [![Build status](https://ci.appveyor.com/api/projects/status/i0us4v5jxi6llk3e/branch/develop?svg=true)](https://ci.appveyor.com/project/lorddev/utilities/branch/develop) | [NuGet](https://www.nuget.org/packages/Devlord.Utilities/) |

To install Devlord.Utilities.Mail, run the following command in the Package Manager Console:

    PM> Install-Package Devlord.Utilities.Mail

Or from the command-line:

    dotnet add package Devlord.Utilities.Mail

### Changes from Devlord.Utilities 6.0

* Mail utilities have been moved to the Devlord.Utilities.Mail package.
* `Devlord.Utilities.Mail` is the new namespace.

### Feature Summary

* .NET Core compatibility
* Email throttling to help you avoid being suspended by your ISP

More details below, but for even _more_ details, see the [project wiki](https://github.com/lorddev/utilities/wiki).

### Mailbot

A multithreaded SMTP queued mail sender, has a configurable throttle for mail frequency sent by a single account.

**Configuration**

```json
{
  "Devlord.Utilities": {
    "MailSettings": [
      {
        "Name": "Gmail",
        "SmtpServer": "mail.google.com",
        "SmtpPort": 587,
        "SmtpPassword": "",
        "MaxPerMinute": 500,
        "MaxPerHour": 500,
        "MaxPerDay": 500
      }
    ]
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
