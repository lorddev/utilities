// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DevlordTestConfiguration.cs" company="Lord Design">
//   © 2022 Lord Design
// </copyright>
// <license type="GPL-3.0">
//   You may use freely and commercially without modification; if you make changes, please share back to the
//   community.
// </license>
// <author>Aaron Lord</author>
// --------------------------------------------------------------------------------------------------------------------

using Devlord.Utilities.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Devlord.Utilities.Tests;

public class DevlordTestConfiguration
{
    public DevlordOptions Options { get; private set; }
        
    public DevlordTestConfiguration()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .AddUserSecrets<DistanceApiTests>()
            .Build();
        var services = new ServiceCollection().AddOptions();
        services.AddDevlordMail(config.GetSection("Devlord.Utilities"));
        services.AddDevlordMaps(config.GetSection("Devlord.Utilities"));
        Options = services.BuildServiceProvider().GetService<IOptions<DevlordOptions>>().Value;
        //return config;
    }
}