using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;

namespace Devlord.Utilities
{
    public class UnderscoreContractResolver : DefaultContractResolver
    {
        #region Methods

        protected override string ResolvePropertyName(string propertyName)
        {
            return Regex.Replace(propertyName, "(?<=[a-z])[A-Z]", m => "_" + m).ToLower();
        }

        #endregion
    }
}