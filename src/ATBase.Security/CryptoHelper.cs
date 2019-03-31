using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using ATBase.Core;

namespace ATBase.Security
{
    /// <summary>
    /// 
    /// </summary>
    public static class CryptoHelper
    {
        private static IOneWayHash _onewayhash;
        private static IRSAProvider _rsaProvider;
        private static IAESProvider _aesProvider;

        static CryptoHelper()
        {
            _onewayhash = new OneWayHash();
            _rsaProvider = new RSAProvider();
            _aesProvider = new AESProvider();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static XResult<String> GetMD5(String input)
        {
            return _onewayhash.ComputeHash(input, HashAlgorithmName.MD5);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static XResult<Byte[]> GetMD5(Byte[] input)
        {
            return _onewayhash.ComputeHash(input, HashAlgorithmName.MD5);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static XResult<String> GetSHA1(String input)
        {
            return _onewayhash.ComputeHash(input, HashAlgorithmName.SHA1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static XResult<Byte[]> GetSHA1(Byte[] input)
        {
            return _onewayhash.ComputeHash(input, HashAlgorithmName.SHA1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static XResult<String> GetSHA256(String input)
        {
            return _onewayhash.ComputeHash(input, HashAlgorithmName.SHA256);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="algorithmName"></param>
        public static XResult<String> ComputeHMAC(String input, String key, HashAlgorithmName algorithmName)
        {
            return _onewayhash.ComputeHMACHash(input, key, algorithmName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static XResult<Byte[]> GetSHA256(Byte[] input)
        {
            return _onewayhash.ComputeHash(input, HashAlgorithmName.SHA256);
        }

        /// <summary>
        /// 生成随机的Key
        /// </summary>
        /// <param name="algName">仅支持DES，TripleDES，3DES三种，默认使用 TripleDES</param>
        public static Byte[] GenerateRandomKey(String algName = "TripleDES")
        {
            return _aesProvider.GenerateRandomKey(algName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        public static XResult<Byte[]> AESEncrypt(Byte[] data, Byte[] key)
        {
            return _aesProvider.AESEncrypt(data, key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="key"></param>
        public static XResult<Byte[]> AESDecrypt(Byte[] encryptedData, Byte[] key)
        {
            return _aesProvider.AESDecrypt(encryptedData, key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawText"></param>
        /// <param name="publicKeyPem"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static XResult<String> RSAEncrypt(String rawText, String publicKeyPem, String charset = "UTF-8")
        {
            return _rsaProvider.Encrypt(rawText, publicKeyPem, charset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="publicKeyPem"></param>
        /// <returns></returns>
        public static XResult<Byte[]> RSAEncrypt(Stream stream, String publicKeyPem)
        {
            return _rsaProvider.Encrypt(stream, publicKeyPem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="publicKeyPem"></param>
        /// <returns></returns>
        public static XResult<Byte[]> RSAEncrypt(Byte[] rawData, String publicKeyPem)
        {
            using (var ms = new MemoryStream(rawData))
            {
                return _rsaProvider.Encrypt(ms, publicKeyPem);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <param name="privateKeyPem"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static XResult<String> RSADecrypt(String encryptedString, String privateKeyPem, String charset = "UTF-8")
        {
            return RSADecrypt(encryptedString, privateKeyPem, PrivateKeyFormat.PKCS1, charset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <param name="privateKeyPem"></param>
        /// <param name="privateKeyFormat"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static XResult<String> RSADecrypt(String encryptedString, String privateKeyPem, PrivateKeyFormat privateKeyFormat, String charset = "UTF-8")
        {
            return _rsaProvider.Decrypt(encryptedString, privateKeyPem, privateKeyFormat, charset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="privateKeyPem"></param>
        /// <returns></returns>
        public static XResult<Byte[]> RSADecrypt(Stream stream, String privateKeyPem)
        {
            return RSADecrypt(stream, privateKeyPem, PrivateKeyFormat.PKCS1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="privateKeyPem"></param>
        /// <param name="privateKeyFormat"></param>
        /// <returns></returns>
        public static XResult<Byte[]> RSADecrypt(Stream stream, String privateKeyPem, PrivateKeyFormat privateKeyFormat)
        {
            return _rsaProvider.Decrypt(stream, privateKeyPem, privateKeyFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signContent"></param>
        /// <param name="privateKeyPem"></param>
        /// <param name="algName"></param>
        /// <returns></returns>
        public static XResult<String> MakeSign(String signContent, String privateKeyPem, HashAlgorithmName algName)
        {
            return MakeSign(signContent, privateKeyPem, PrivateKeyFormat.PKCS1, algName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signContent"></param>
        /// <param name="privateKeyPem"></param>
        /// <param name="privateKeyFormat"></param>
        /// <param name="algName"></param>
        /// <returns></returns>
        public static XResult<String> MakeSign(String signContent, String privateKeyPem, PrivateKeyFormat privateKeyFormat, HashAlgorithmName algName)
        {
            if (String.IsNullOrWhiteSpace(signContent))
            {
                return new XResult<String>(null, new ArgumentNullException("signContent is null"));
            }

            if (String.IsNullOrWhiteSpace(privateKeyPem))
            {
                return new XResult<String>(null, new ArgumentNullException("privateKeyPem is null"));
            }

            return _rsaProvider.MakeSign(signContent, privateKeyPem, privateKeyFormat, algName, "UTF-8");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signContent"></param>
        /// <param name="privateKeyPem"></param>
        /// <param name="privateKeyFormat"></param>
        /// <param name="algName"></param>
        public static XResult<Byte[]> MakeSign(Byte[] signContent, String privateKeyPem, PrivateKeyFormat privateKeyFormat, HashAlgorithmName algName)
        {
            if (signContent == null || signContent.Length == 0)
            {
                return new XResult<Byte[]>(null, new ArgumentNullException(nameof(signContent)));
            }

            if (String.IsNullOrWhiteSpace(privateKeyPem))
            {
                return new XResult<Byte[]>(null, new ArgumentNullException(nameof(privateKeyPem)));
            }

            return _rsaProvider.MakeSign(signContent, privateKeyPem, privateKeyFormat, algName, "UTF-8");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signNeedToVerify"></param>
        /// <param name="signContent"></param>
        /// <param name="publicKeyPem"></param>
        /// <param name="algName"></param>
        /// <returns></returns>
        public static XResult<Boolean> VerifySign(String signNeedToVerify, String signContent, String publicKeyPem, HashAlgorithmName algName)
        {
            if (String.IsNullOrWhiteSpace(signNeedToVerify))
            {
                return new XResult<Boolean>(false, new ArgumentNullException("signNeedToVerify is null"));
            }

            if (String.IsNullOrWhiteSpace(signContent))
            {
                return new XResult<Boolean>(false, new ArgumentNullException("signContent is null"));
            }

            if (String.IsNullOrWhiteSpace(publicKeyPem))
            {
                return new XResult<Boolean>(false, new ArgumentNullException("publicKeyPem is null"));
            }

            return _rsaProvider.VerifySign(signNeedToVerify, signContent, publicKeyPem, algName, "UTF-8");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signNeedToVerify"></param>
        /// <param name="signContent"></param>
        /// <param name="publicKey"></param>
        /// <param name="algName"></param>
        public static XResult<Boolean> VerifySign(Byte[] signNeedToVerify, Byte[] signContent, String publicKey, HashAlgorithmName algName)
        {
            return _rsaProvider.VerifySign(signNeedToVerify, signContent, publicKey, algName);
        }
    }
}
