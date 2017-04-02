using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Devlord.Utilities.Text
{
    public class StringFormat
    {
        #region Public Methods and Operators

        public static IEnumerable<T> Parse<T>(string format, string input) where T : struct
        {
            string template = Regex.Replace(format, @"[\\\^\$\.\|\?\*\+\(\)]", m => "\\" + m.Value);
            string pattern = "^" + Regex.Replace(template, @"\{[0-9]+\}", "(.*?)") + "$";

            var r = new Regex(pattern);
            Match match = r.Match(input);

            var ret = new List<T>();

            for (int i = 1; i < match.Groups.Count; i++)
            {
                var c = GetConverter<T>();
                ret.Add(c.Invoke(match.Groups[i].Value));
            }

            return ret.AsEnumerable();
        }

        private static Converter<string, T> GetConverter<T>()
        { // Box it.
            Converter<string, T> c = s => (T)(object)double.Parse(s);

            if (typeof(T) == typeof(int))
            {
                c = s => (T)(object)int.Parse(s);
            }

            if (typeof(T) == typeof(decimal))
            {
                c = s => (T)(object)decimal.Parse(s);
            }

            return c;
        }

        public delegate TOutput Converter<in TInput, out TOutput>(TInput input);

        #endregion
    }
}