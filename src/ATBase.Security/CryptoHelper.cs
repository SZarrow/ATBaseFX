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
        private static IDESProvider _desProvider;

        static CryptoHelper()
        {
            _onewayhash = new OneWayHash();
            _rsaProvider = new RSAProvider();
            _aesProvider = new AESProvider();
            _desProvider = new DESProvider();
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
        /// 使用AES算法生成随机的Key
        /// </summary>
        public static Byte[] GenerateAESRandomKey()
        {
            return _aesProvider.GenerateRandomKey();
        }

        /// <summary>
        /// 使用DES算法生成随机的Key
        /// </summary>
        public static Byte[] GenerateDESRandomKey()
        {
            return _desProvider.GenerateRandomKey();
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
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        public static XResult<Byte[]> AESEncrypt(Byte[] data, Byte[] key, Byte[] iv)
        {
            return _aesProvider.AESEncrypt(data, key, iv);
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
        /// <param name="encryptedData"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        public static XResult<Byte[]> AESDecrypt(Byte[] encryptedData, Byte[] key, Byte[] iv)
        {
            return _aesProvider.AESDecrypt(encryptedData, key, iv);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        public static XResult<Byte[]> DESEncrypt(Byte[] data, Byte[] key)
        {
            return _desProvider.DESEncrypt(data, key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        public static XResult<Byte[]> DESEncrypt(Byte[] data, Byte[] key, Byte[] iv)
        {
            return _desProvider.DESEncrypt(data, key, iv);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="key"></param>
        public static XResult<Byte[]> DESDecrypt(Byte[] encryptedData, Byte[] key)
        {
            return _desProvider.DESDecrypt(encryptedData, key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        public static XResult<Byte[]> DESDecrypt(Byte[] encryptedData, Byte[] key, Byte[] iv)
        {
            return _desProvider.DESDecrypt(encryptedData, key, iv);
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
        /// 生成RSA签名字符串
        /// </summary>
        /// <param name="signContent"></param>
        /// <param name="privateKeyPem"></param>
        /// <param name="algName"></param>
        public static XResult<String> MakeSign(String signContent, String privateKeyPem, HashAlgorithmName algName)
        {
            return MakeSign(signContent, privateKeyPem, PrivateKeyFormat.PKCS1, algName);
        }

        /// <summary>
        /// 生成RSA签名字符串
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
        /// 生成RSA签名字符串
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
        /// 对RSA签名进行验证
        /// </summary>
        /// <param name="signNeedToVerify">验签用的Sign值</param>
        /// <param name="signContent">用来计算签名的内容</param>
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
        /// 对RSA签名进行验签
        /// </summary>
        /// <param name="signNeedToVerify">验签用的Sign值</param>
        /// <param name="signContent">用来计算签名的内容</param>
        /// <param name="publicKey"></param>
        /// <param name="algName"></param>
        public static XResult<Boolean> VerifySign(Byte[] signNeedToVerify, Byte[] signContent, String publicKey, HashAlgorithmName algName)
        {
            return _rsaProvider.VerifySign(signNeedToVerify, signContent, publicKey, algName);
        }
    }
}
