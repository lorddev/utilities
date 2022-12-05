// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DevlordConfigurationException.cs" company="Lord Design">
//   © 2022 Lord Design
// </copyright>
// <license type="GPL-3.0">
//   You may use freely and commercially without modification; if you make changes, please share back to the
//   community.
// </license>
// <author>Aaron Lord</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace Devlord.Utilities.Exceptions
{

    [Serializable]
    public class DevlordConfigurationException : Exception
    {
        public DevlordConfigurationException(string s) : base(s)
        {
        }

        protected DevlordConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}