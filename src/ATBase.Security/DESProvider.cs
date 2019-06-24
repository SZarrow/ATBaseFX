using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ATBase.Core;

namespace ATBase.Security
{
    /// <summary>
    /// 
    /// </summary>
    public class DESProvider : IDESProvider
    {
        private const Int32 DefaultKeySize = 64;
        private const Int32 DefaultBlockSize = 64;
        private const CipherMode DefaultCipherMode = CipherMode.CBC;
        private const PaddingMode DefaultPaddingMode = PaddingMode.PKCS7;
        private static readonly Byte[] DefaultIV = Convert.FromBase64String("AAAAAAAAAAA=");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="key"></param>
        public XResult<Byte[]> DESDecrypt(Byte[] encryptedData, Byte[] key)
        {
            return DESDecrypt(encryptedData, key, DefaultIV);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        public XResult<Byte[]> DESDecrypt(Byte[] encryptedData, Byte[] key, Byte[] iv)
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

            using (DES desAlg = DES.Create())
            {
                desAlg.KeySize = DefaultKeySize;
                desAlg.BlockSize = DefaultBlockSize;
                desAlg.Mode = DefaultCipherMode;
                desAlg.Padding = DefaultPaddingMode;

                desAlg.Key = key;
                desAlg.IV = iv;

                var decryptor = desAlg.CreateDecryptor(desAlg.Key, desAlg.IV);

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

                desAlg.Clear();
            }

            return new XResult<Byte[]>(decryptedData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        public XResult<Byte[]> DESEncrypt(Byte[] data, Byte[] key)
        {
            return DESEncrypt(data, key, DefaultIV);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        public XResult<Byte[]> DESEncrypt(Byte[] data, Byte[] key, Byte[] iv)
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

            using (DES desAlg = DES.Create())
            {
                desAlg.KeySize = DefaultKeySize;
                desAlg.BlockSize = DefaultBlockSize;
                desAlg.Mode = DefaultCipherMode;
                desAlg.Padding = DefaultPaddingMode;

                desAlg.Key = key;
                desAlg.IV = iv;

                var encryptor = desAlg.CreateEncryptor(desAlg.Key, desAlg.IV);

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

                desAlg.Clear();
            }

            return new XResult<Byte[]>(encrypted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Byte[] GenerateRandomKey()
        {
            using (var des = DES.Create())
            {
                des.GenerateKey();
                des.KeySize = DefaultKeySize;
                return des.Key;
            }
        }
    }
}
