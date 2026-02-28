// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMailbotFactory.cs" company="Lord Design">
//   © 2022 Lord Design
// </copyright>
// <license type="GPL-3.0">
//   You may use freely and commercially without modification; if you make changes, please share back to the
//   community.
// </license>
// <author>Aaron Lord</author>
// --------------------------------------------------------------------------------------------------------------------

namespace Devlord.Utilities.Mail
{
    public interface IMailbotFactory
    {
        Mailbot GetMailbot(string smtpServer);
    }
}