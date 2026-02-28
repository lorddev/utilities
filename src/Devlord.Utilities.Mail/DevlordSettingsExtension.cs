// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DevlordSettingsExtension.cs" company="Lord Design">
//   © 2022 Lord Design
// </copyright>
// <license type="GPL-3.0">
//   You may use freely and commercially without modification; if you make changes, please share back to the
//   community.
// </license>
// <author>Aaron Lord</author>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Devlord.Utilities.Mail
{
    public static class DevlordSettingsExtension
    {
        public static IServiceCollection AddDevlordMail(this IServiceCollection services,
            IConfiguration namedConfigurationSection)
        {
            services.Configure<DevlordOptions>(namedConfigurationSection);
            services.AddSingleton<IMailbotFactory, MailbotFactory>();

            return services;
        }
    }
}