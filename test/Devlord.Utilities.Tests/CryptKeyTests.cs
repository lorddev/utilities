using System.Text;
using Devlord.Utilities.Cryptography;
using Xunit;
using Xunit.Abstractions;

namespace Devlord.Utilities.Tests
{
    public class CryptKeyTests
    {
        public CryptKeyTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private readonly ITestOutputHelper _output;

        [Theory(Skip = "This is meant for outputing arrays that we can copy to code. Run it manually.")]
        public void MakeNewKeyAsString()
        {
            var key = Crypt.MakeKey();
            var builder = new StringBuilder("var newKey = new byte[]");
            builder.AppendLine("{");

            var lastIndex = key.Length - 1;

            for (var i = 0; i < key.Length; i++)
            {
                builder.Append("    " + key[i]);
                if (i != lastIndex)
                {
                    builder.Append(',');
                }

                builder.AppendLine();
            }

            builder.AppendLine("};");

            _output.WriteLine(builder.ToString());
        }

        [Fact]
        public void VerifyBCryptSimple()
        {
            // Arrange
            const string clearText = "P@s5W1rdBruh";
            var encryptor = new Crypt();
            var ciphered = encryptor.OneWay(clearText);

            // first: "$2a$10$3btSI0hlWbdg5.xdb/x0suZGjxwiFsw7omOFvlLnoduPH3BB5Af6W"

            // Act
            var result = encryptor.Verify(Crypt.SafeUtf8.GetBytes(clearText),
                "$2a$10$3btSI0hlWbdg5.xdb/x0suZGjxwiFsw7omOFvlLnoduPH3BB5Af6W");

            // Assert
            Assert.True(result);

            result = encryptor.Verify(new byte[]
                {
                    80,
                    64,
                    115,
                    53,
                    87,
                    49,
                    114,
                    100,
                    66,
                    114,
                    117,
                    104
                }, "$2a$10$L048plNTmEuy4znjJe5K8uS0y9EPWp0vNvicaLYcqlmaT/bJawGJy"

            );

            result.ShouldBeTrue();

            result = encryptor.Verify(Crypt.SafeUtf8.GetBytes("fakePass"),
                "$2a$10$/.Ca2J2etjIfXPUCWAKB5.ldzqDw00A7MYuggwFCwzkX3GPZ5Ptfu");

            Assert.False(result);
        }
    }
}