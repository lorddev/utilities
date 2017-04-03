using System.Diagnostics;
using System.Text;
using Xunit;

namespace Devlord.Utilities.Tests
{
    public class CryptKeyTests
    {
        [Fact]
        public void MakeNewKeyAsString()
        {
            var key = Crypt.MakeKey();
            var builder = new StringBuilder("var newKey = new byte[]");
            builder.AppendLine("{");

            var lastIndex = key.Length - 1;

            for (var i = 0; i < key.Length; i++)
            {
                builder.Append("    " + key[i]);
                if (i == lastIndex)
                {
                    builder.Append(',');
                }

                builder.AppendLine();
            }

            builder.AppendLine("};");

            Debug.Write(key.ToString());
        }
    }
}