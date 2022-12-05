using System.Collections.Generic;

namespace Devlord.Utilities
{
    public class SettingNotFoundException : KeyNotFoundException
    {
        public SettingNotFoundException(string setting) : base(
            (string) $"{nameof(SettingNotFoundException)}: {setting}")
        {
        }
    }
}