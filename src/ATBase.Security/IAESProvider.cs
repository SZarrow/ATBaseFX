using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Core;

namespace ATBase.Security
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAESProvider
    {
        /// <summary>
        /// 生成随机的Key
        /// </summary>
        /// <param name="algName">仅支持DES，TripleDES，3DES三种，默认使用 TripleDES</param>
        Byte[] GenerateRandomKey(String algName = "TripleDES");
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        XResult<Byte[]> AESEncrypt(Byte[] data, Byte[] key);
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        XResult<Byte[]> AESEncrypt(Byte[] data, Byte[] key, Byte[] iv);
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="key"></param>
        XResult<Byte[]> AESDecrypt(Byte[] encryptedData, Byte[] key);
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        XResult<Byte[]> AESDecrypt(Byte[] encryptedData, Byte[] key, Byte[] iv);
    }
}
