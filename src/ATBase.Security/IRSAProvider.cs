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
    public interface IRSAProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawText"></param>
        /// <param name="publicKeyPem"></param>
        /// <param name="charset"></param>
        XResult<String> Encrypt(String rawText, String publicKeyPem, String charset);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="publicKeyPem"></param>
        XResult<Byte[]> Encrypt(Stream stream, String publicKeyPem);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <param name="privateKeyPem"></param>
        /// <param name="privateKeyFormat"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        XResult<String> Decrypt(String encryptedString, String privateKeyPem, PrivateKeyFormat privateKeyFormat, String charset);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="privateKeyPem"></param>
        /// <param name="privateKeyFormat"></param>
        XResult<Byte[]> Decrypt(Stream stream, String privateKeyPem, PrivateKeyFormat privateKeyFormat);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="signContent"></param>
        /// <param name="privateKeyPem"></param>
        /// <param name="privateKeyFormat"></param>
        /// <param name="algName"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        XResult<String> MakeSign(String signContent, String privateKeyPem, PrivateKeyFormat privateKeyFormat, HashAlgorithmName algName, String charset);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="signContent"></param>
        /// <param name="privateKeyPem"></param>
        /// <param name="privateKeyFormat"></param>
        /// <param name="algName"></param>
        /// <param name="charset"></param>
        XResult<Byte[]> MakeSign(Byte[] signContent, String privateKeyPem, PrivateKeyFormat privateKeyFormat, HashAlgorithmName algName, String charset);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="signNeedToVerify"></param>
        /// <param name="signContent"></param>
        /// <param name="publicKeyPem"></param>
        /// <param name="algName"></param>
        /// <param name="charset"></param>
        XResult<Boolean> VerifySign(String signNeedToVerify, String signContent, String publicKeyPem, HashAlgorithmName algName, String charset);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="signNeedToVerify"></param>
        /// <param name="signContent"></param>
        /// <param name="publicKeyPem"></param>
        /// <param name="algName"></param>
        XResult<Boolean> VerifySign(Byte[] signNeedToVerify, Byte[] signContent, String publicKeyPem, HashAlgorithmName algName);
    }
}
