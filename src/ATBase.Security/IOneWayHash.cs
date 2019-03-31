using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ATBase.Core;

namespace ATBase.Security
{
    /// <summary>
    /// 单向函数
    /// </summary>
    public interface IOneWayHash
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="algName"></param>
        XResult<Byte[]> ComputeHash(Byte[] input, HashAlgorithmName algName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="algName"></param>
        /// <param name="convertToBase64String"></param>
        XResult<String> ComputeHash(String input, HashAlgorithmName algName, Boolean convertToBase64String = false);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="algName"></param>
        /// <param name="convertToBase64String"></param>
        XResult<String> ComputeHash(Stream input, HashAlgorithmName algName, Boolean convertToBase64String = false);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="algName"></param>
        XResult<Byte[]> ComputeHMACHash(Byte[] input, Byte[] key, HashAlgorithmName algName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="algName"></param>
        /// <param name="convertToBase64String"></param>
        XResult<String> ComputeHMACHash(Stream input, String key, HashAlgorithmName algName, Boolean convertToBase64String = false);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="algName"></param>
        /// <param name="convertToBase64String"></param>
        XResult<String> ComputeHMACHash(String input, String key, HashAlgorithmName algName, Boolean convertToBase64String = false);
    }
}
