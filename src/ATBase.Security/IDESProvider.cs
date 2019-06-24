using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Core;

namespace ATBase.Security
{
    /// <summary>
    /// DES加密解密
    /// </summary>
    public interface IDESProvider
    {
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        XResult<Byte[]> DESEncrypt(Byte[] data, Byte[] key);
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        XResult<Byte[]> DESEncrypt(Byte[] data, Byte[] key, Byte[] iv);
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="key"></param>
        XResult<Byte[]> DESDecrypt(Byte[] encryptedData, Byte[] key);
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        XResult<Byte[]> DESDecrypt(Byte[] encryptedData, Byte[] key, Byte[] iv);
    }
}
