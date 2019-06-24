using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ATBase.Core;

namespace ATBase.Security
{
    internal class AESProvider : IAESProvider
    {
        private const Int32 DefaultKeySize = 256;
        private const Int32 DefaultBlockSize = 128;
        private const CipherMode DefaultCipherMode = CipherMode.CBC;
        private const PaddingMode DefaultPaddingMode = PaddingMode.PKCS7;
        private static readonly Byte[] DefaultIV = Convert.FromBase64String("AAAAAAAAAAAAAAAAAAAAAA==");

        public XResult<Byte[]> AESEncrypt(Byte[] data, Byte[] key)
        {
            return AESEncrypt(data, key, DefaultIV);
        }

        public XResult<Byte[]> AESEncrypt(Byte[] data, Byte[] key, Byte[] iv)
        {
            if (data == null || data.Length == 0)
            {
                return new XResult<Byte[]>(null, new ArgumentNullException(nameof(data)));
            }

            if (key == null || key.Length == 0)
            {
                return new XResult<Byte[]>(null, new ArgumentNullException(nameof(key)));
            }

            Byte[] encrypted = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = DefaultKeySize;
                aesAlg.BlockSize = DefaultBlockSize;
                aesAlg.Mode = DefaultCipherMode;
                aesAlg.Padding = DefaultPaddingMode;

                aesAlg.Key = key;
                aesAlg.IV = iv;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                MemoryStream msEncrypt = null;
                CryptoStream csEncrypt = null;
                try
                {
                    msEncrypt = new MemoryStream();
                    csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                    csEncrypt.Write(data, 0, data.Length);

                    csEncrypt.FlushFinalBlock();
                    encrypted = msEncrypt.ToArray();
                }
                catch (Exception ex)
                {
                    return new XResult<Byte[]>(null, ex);
                }
                finally
                {
                    if (csEncrypt != null) { csEncrypt.Dispose(); }
                    if (msEncrypt != null) { msEncrypt.Dispose(); }
                }

                aesAlg.Clear();
            }

            return new XResult<Byte[]>(encrypted);
        }

        public XResult<Byte[]> AESDecrypt(Byte[] encryptedData, Byte[] key)
        {
            return AESDecrypt(encryptedData, key, DefaultIV);
        }

        public XResult<Byte[]> AESDecrypt(Byte[] encryptedData, Byte[] key, Byte[] iv)
        {
            if (encryptedData == null || encryptedData.Length == 0)
            {
                return new XResult<Byte[]>(null, new ArgumentNullException(nameof(encryptedData)));
            }

            if (key == null || key.Length == 0)
            {
                return new XResult<Byte[]>(null, new ArgumentNullException(nameof(key)));
            }

            Byte[] decryptedData = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = DefaultKeySize;
                aesAlg.BlockSize = DefaultBlockSize;
                aesAlg.Mode = DefaultCipherMode;
                aesAlg.Padding = DefaultPaddingMode;

                aesAlg.Key = key;
                aesAlg.IV = iv;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                MemoryStream msDecrypt = null;
                CryptoStream csDecrypt = null;
                try
                {
                    msDecrypt = new MemoryStream(encryptedData);
                    csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

                    Byte[] buffer = new Byte[2048];
                    Int32 read = 0;

                    using (MemoryStream resultStream = new MemoryStream())
                    {
                        while ((read = csDecrypt.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            resultStream.Write(buffer, 0, read);
                        }

                        decryptedData = resultStream.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    return new XResult<Byte[]>(null, ex);
                }
                finally
                {
                    if (csDecrypt != null) { csDecrypt.Dispose(); }
                    if (msDecrypt != null) { msDecrypt.Dispose(); }
                }

                aesAlg.Clear();
            }

            return new XResult<Byte[]>(decryptedData);
        }

        public Byte[] GenerateRandomKey()
        {
            using (var aes = Aes.Create())
            {
                aes.GenerateKey();
                aes.KeySize = DefaultKeySize;
                return aes.Key;
            }
        }
    }
}
