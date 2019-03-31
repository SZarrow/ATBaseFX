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
    internal class OneWayHash : IOneWayHash
    {
        public XResult<Byte[]> ComputeHash(Byte[] input, HashAlgorithmName algName)
        {
            if (input == null || input.Length == 0)
            {
                return new XResult<Byte[]>(null, new ArgumentNullException(nameof(input)));
            }

            Byte[] result = null;
            HashAlgorithm alg = null;

            try
            {
                alg = GetHashAlgorithm(algName);
                result = alg.ComputeHash(input);
            }
            catch (Exception ex)
            {
                return new XResult<Byte[]>(null, ex); ;
            }
            finally
            {
                if (alg != null)
                {
                    alg.Dispose();
                }
            }

            if (result == null || result.Length == 0)
            {
                return new XResult<Byte[]>(null, new ArgumentNullException("the computed result is null")); ;
            }

            return new XResult<Byte[]>(result);
        }

        public XResult<String> ComputeHash(String input, HashAlgorithmName algName, Boolean convertToBase64String = false)
        {
            if (String.IsNullOrEmpty(input))
            {
                return new XResult<String>(null, new ArgumentNullException(nameof(input)));
            }

            Byte[] data = null;
            Byte[] result = null;
            HashAlgorithm alg = null;

            try
            {
                data = Encoding.UTF8.GetBytes(input);
                alg = GetHashAlgorithm(algName);
                result = alg.ComputeHash(data);
            }
            catch (Exception ex)
            {
                return new XResult<String>(null, ex);
            }
            finally
            {
                if (alg != null)
                {
                    alg.Dispose();
                }
            }

            if (result == null || result.Length == 0)
            {
                return new XResult<String>(null, new ArgumentNullException("the computed result is null"));
            }

            if (!convertToBase64String)
            {

                StringBuilder sb = new StringBuilder();
                foreach (var b in result)
                {
                    sb.Append(b.ToString("x2"));
                }

                return new XResult<String>(sb.ToString());
            }
            else
            {
                return new XResult<String>(Convert.ToBase64String(result));
            }
        }

        public XResult<String> ComputeHash(Stream input, HashAlgorithmName algName, Boolean convertToBase64String = false)
        {
            if (input == null || input.Length == 0)
            {
                return new XResult<String>(null, new ArgumentNullException(nameof(input)));
            }

            Byte[] result = null;
            HashAlgorithm alg = null;

            try
            {
                alg = GetHashAlgorithm(algName);
                result = alg.ComputeHash(input);
            }
            catch (Exception ex)
            {
                return new XResult<String>(null, ex); ;
            }
            finally
            {
                if (alg != null)
                {
                    alg.Dispose();
                }
            }

            if (result == null || result.Length == 0)
            {
                return new XResult<String>(null, new ArgumentNullException("the computed result is null")); ;
            }

            if (!convertToBase64String)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var b in result)
                {
                    sb.Append(b.ToString("x2"));
                }

                return new XResult<String>(sb.ToString());
            }
            else
            {
                return new XResult<String>(Convert.ToBase64String(result));
            }
        }

        public XResult<Byte[]> ComputeHMACHash(Byte[] input, Byte[] key, HashAlgorithmName algName)
        {
            if (input == null || input.Length == 0)
            {
                return new XResult<Byte[]>(null, new ArgumentNullException(nameof(input)));
            }

            if (key == null || key.Length == 0)
            {
                return new XResult<Byte[]>(null, new ArgumentNullException(nameof(key)));
            }

            HMAC hmac = null;
            Byte[] result = null;
            try
            {
                hmac = GetHMACHashAlgorithm(algName, key);
                result = hmac.ComputeHash(input);
            }
            catch (Exception ex)
            {
                return new XResult<Byte[]>(null, ex);
            }

            if (result == null || result.Length == 0)
            {
                return new XResult<Byte[]>(null, new ArgumentNullException("hmac.ComputeHash(input) returns null"));
            }

            return new XResult<Byte[]>(result);
        }

        public XResult<String> ComputeHMACHash(Stream input, String key, HashAlgorithmName algName, Boolean convertToBase64String = false)
        {
            if (input == null || input.Length == 0)
            {
                return new XResult<String>(null, new ArgumentNullException(nameof(input)));
            }

            if (String.IsNullOrWhiteSpace(key))
            {
                return new XResult<String>(null, new ArgumentNullException(nameof(key)));
            }

            Byte[] keyData = null;
            try
            {
                keyData = Encoding.UTF8.GetBytes(key);
            }
            catch (Exception ex)
            {
                return new XResult<String>(null, ex);
            }

            HMAC hmac = null;
            Byte[] result = null;
            try
            {
                hmac = GetHMACHashAlgorithm(algName, keyData);
                result = hmac.ComputeHash(input);
            }
            catch (Exception ex)
            {
                return new XResult<String>(null, ex);
            }

            if (result == null || result.Length == 0)
            {
                return new XResult<String>(null, new ArgumentNullException("hmac.ComputeHash(input) returns null"));
            }

            if (!convertToBase64String)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
                return new XResult<String>(sb.ToString());
            }
            else
            {
                return new XResult<String>(Convert.ToBase64String(result));
            }
        }

        public XResult<String> ComputeHMACHash(String input, String key, HashAlgorithmName algName, Boolean convertToBase64String = false)
        {
            if (String.IsNullOrWhiteSpace(input))
            {
                return new XResult<String>(null, new ArgumentNullException(nameof(input)));
            }

            if (String.IsNullOrWhiteSpace(key))
            {
                return new XResult<String>(null, new ArgumentNullException(nameof(key)));
            }

            Byte[] keyData = null;
            try
            {
                keyData = Encoding.UTF8.GetBytes(key);
            }
            catch (Exception ex)
            {
                return new XResult<String>(null, ex);
            }

            Byte[] inputData = null;
            try
            {
                inputData = Encoding.UTF8.GetBytes(input);
            }
            catch (Exception ex)
            {
                return new XResult<String>(null, ex);
            }

            HMAC hmac = null;
            Byte[] result = null;
            try
            {
                hmac = GetHMACHashAlgorithm(algName, keyData);
                result = hmac.ComputeHash(inputData);
            }
            catch (Exception ex)
            {
                return new XResult<String>(null, ex);
            }

            if (result == null || result.Length == 0)
            {
                return new XResult<String>(null, new ArgumentNullException("hmac.ComputeHash(inputData) returns null"));
            }

            if (!convertToBase64String)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
                return new XResult<String>(sb.ToString());
            }
            else
            {
                return new XResult<String>(Convert.ToBase64String(result));
            }
        }

        private HashAlgorithm GetHashAlgorithm(HashAlgorithmName algName)
        {
            return HashAlgorithm.Create(algName.Name);
        }

        private HMAC GetHMACHashAlgorithm(HashAlgorithmName algName, Byte[] key)
        {
            if (key != null && key.Length > 0)
            {
                switch (algName.Name)
                {
                    case "MD5":
                        return new HMACMD5(key);
                    case "SHA1":
                        return new HMACSHA1(key);
                    case "SHA256":
                        return new HMACSHA256(key);
                    case "SHA384":
                        return new HMACSHA384(key);
                    case "SHA512":
                        return new HMACSHA512(key);
                }
            }

            return HMAC.Create(algName.Name);
        }
    }
}
