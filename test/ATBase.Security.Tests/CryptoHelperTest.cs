using System;
using System.Text;
using Xunit;

namespace ATBase.Security.Tests
{
    public class CryptoHelperTest
    {
        [Fact]
        public void TestDES()
        {
            var key = CryptoHelper.GenerateDESRandomKey();
            var testCase = "≤‚ ‘123";
            var encrypted = CryptoHelper.DESEncrypt(Encoding.UTF8.GetBytes(testCase), key);
            var decrypted = CryptoHelper.DESDecrypt(encrypted.Value, key);
            Assert.Equal(testCase, Encoding.UTF8.GetString(decrypted.Value));
        }
    }
}
