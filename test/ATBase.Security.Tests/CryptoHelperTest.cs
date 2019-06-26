using System;
using System.IO;
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
            var testCase = File.ReadAllText(@"C:\Users\Administrator\Desktop\1.txt", Encoding.UTF8);
            var encrypted = CryptoHelper.DESEncrypt(Encoding.UTF8.GetBytes(testCase), key, key);
            var decrypted = CryptoHelper.DESDecrypt(encrypted.Value, key);
            Assert.Equal(testCase, Encoding.UTF8.GetString(decrypted.Value));
        }
    }
}
