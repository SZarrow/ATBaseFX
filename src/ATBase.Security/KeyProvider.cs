using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ATBase.Security
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="algName"></param>
        public static Byte[] GenerateRandomKey(String algName)
        {
            Int32 keySize = 128;
            switch (algName)
            {
                case "DES":
                    keySize = 64;
                    break;
                case "3DES":
                    keySize = 192;
                    break;
            }

            SymmetricAlgorithm alg = null;
            try
            {
                alg = SymmetricAlgorithm.Create(algName);
                alg.KeySize = keySize;
                return alg.Key;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (alg != null) { alg.Dispose(); }
            }
        }
    }
}
