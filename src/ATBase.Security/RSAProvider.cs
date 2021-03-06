﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ATBase.Core;
using XC.RSAUtil;

namespace ATBase.Security
{
    internal class RSAProvider : IRSAProvider
    {
        public XResult<String> Encrypt(String rawText, String publicKeyPem, String charset)
        {
            if (String.IsNullOrWhiteSpace(rawText))
            {
                return new XResult<String>(null, new ArgumentNullException("rawText is null or empty"));
            }

            if (String.IsNullOrWhiteSpace(publicKeyPem))
            {
                return new XResult<String>(null, new ArgumentNullException("publicKeyPem is null"));
            }

            if (String.IsNullOrWhiteSpace(charset))
            {
                return new XResult<String>(null, new ArgumentNullException("charset is null"));
            }

            Byte[] inputData = null;
            try
            {
                inputData = Encoding.GetEncoding(charset).GetBytes(rawText);
            }
            catch (Exception ex)
            {
                return new XResult<String>(null, ex);
            }

            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream(inputData);
                var result = Encrypt(ms, publicKeyPem);
                if (result.Success)
                {
                    String encryptedString = Convert.ToBase64String(result.Value);
                    return new XResult<String>(encryptedString);
                }
                else
                {
                    return new XResult<String>(null, result.FirstException);
                }
            }
            catch (Exception ex)
            {
                return new XResult<String>(null, ex);
            }
            finally
            {
                if (ms != null) { ms.Dispose(); }
            }
        }

        public XResult<Byte[]> Encrypt(Stream stream, String publicKeyPem)
        {
            if (stream == null || stream.Length == 0 || !stream.CanRead)
            {
                return new XResult<Byte[]>(null, new ArgumentNullException("stream is null or empty"));
            }

            if (String.IsNullOrWhiteSpace(publicKeyPem))
            {
                return new XResult<Byte[]>(null, new ArgumentNullException("publicKeyPem is null"));
            }

            RSA rsa = null;
            try
            {
                rsa = CreateRSAFromPublicKey(publicKeyPem);
            }
            catch (Exception ex)
            {
                return new XResult<Byte[]>(null, ex);
            }

            if (rsa == null)
            {
                return new XResult<Byte[]>(null, new CryptographicException("CreateRSAFromPublicKey(publicKeyPem) returns null"));
            }

            Int32 maxBlockSize = rsa.KeySize / 8 - 11;

            if (stream.Length <= maxBlockSize)
            {
                try
                {
                    var inputData = new Byte[stream.Length];
                    stream.Read(inputData, 0, inputData.Length);
                    var encryptedData = rsa.Encrypt(inputData, RSAEncryptionPadding.Pkcs1);
                    return new XResult<Byte[]>(encryptedData);
                }
                catch (Exception ex)
                {
                    return new XResult<Byte[]>(null, ex);
                }
            }

            Byte[] readBuffer = new Byte[maxBlockSize];
            Int32 read = 0;

            var cryptoStream = new MemoryStream();
            try
            {
                while ((read = stream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    Byte[] toEncrypt = new Byte[read];
                    Array.Copy(readBuffer, 0, toEncrypt, 0, read);
                    var encryptedBuffer = rsa.Encrypt(toEncrypt, RSAEncryptionPadding.Pkcs1);
                    cryptoStream.Write(encryptedBuffer, 0, encryptedBuffer.Length);
                }

                return new XResult<Byte[]>(cryptoStream.ToArray());
            }
            catch (Exception ex)
            {
                return new XResult<Byte[]>(null, ex);
            }
            finally
            {
                cryptoStream.Dispose();
            }
        }

        public XResult<String> Decrypt(String encryptedString, String privateKeyPem, PrivateKeyFormat privateKeyFormat, String charset)
        {
            if (String.IsNullOrWhiteSpace(encryptedString))
            {
                return new XResult<String>(null, new ArgumentNullException("encryptedString is null"));
            }

            if (String.IsNullOrWhiteSpace(privateKeyPem))
            {
                return new XResult<String>(null, new ArgumentNullException("privateKeyPem is null"));
            }

            if (String.IsNullOrWhiteSpace(charset))
            {
                return new XResult<String>(null, new ArgumentNullException("charset is null"));
            }

            Byte[] inputData = null;
            try
            {
                inputData = Convert.FromBase64String(encryptedString);
            }
            catch (Exception ex)
            {
                return new XResult<String>(null, ex);
            }

            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream(inputData);
                var result = Decrypt(ms, privateKeyPem, privateKeyFormat);
                if (result.Success)
                {
                    try
                    {
                        String decryptedString = Encoding.GetEncoding(charset).GetString(result.Value);
                        return new XResult<String>(decryptedString);
                    }
                    catch (Exception ex)
                    {
                        return new XResult<String>(null, ex);
                    }
                }
                else
                {
                    return new XResult<String>(null, result.FirstException);
                }
            }
            catch (Exception ex)
            {
                return new XResult<String>(null, ex);
            }
            finally
            {
                if (ms != null) { ms.Dispose(); }
            }
        }

        public XResult<Byte[]> Decrypt(Stream stream, String privateKeyPem, PrivateKeyFormat privateKeyFormat)
        {
            if (stream == null || stream.Length == 0)
            {
                return new XResult<Byte[]>(null, new ArgumentNullException("stream is null or empty"));
            }

            if (String.IsNullOrWhiteSpace(privateKeyPem))
            {
                return new XResult<Byte[]>(null, new ArgumentNullException("privateKeyPem is null"));
            }

            RSA rsa = null;
            try
            {
                rsa = CreateRSAFromPrivateKey(privateKeyPem, privateKeyFormat);
            }
            catch (Exception ex)
            {
                return new XResult<Byte[]>(null, ex);
            }

            if (rsa == null)
            {
                return new XResult<Byte[]>(null, new CryptographicException("CreateRSAFromPrivateKey(privateKeyPem) returns null"));
            }

            if (stream.Length > 0 && stream.CanSeek && stream.Position != 0)
            {
                stream.Position = 0;
            }

            Int32 maxBlockSize = rsa.KeySize / 8;

            if (stream.Length <= maxBlockSize)
            {
                try
                {
                    var inputData = new Byte[stream.Length];
                    stream.Read(inputData, 0, inputData.Length);
                    var decryptedData = rsa.Decrypt(inputData, RSAEncryptionPadding.Pkcs1);
                    return new XResult<Byte[]>(decryptedData);
                }
                catch (Exception ex)
                {
                    return new XResult<Byte[]>(null, ex);
                }
            }

            Byte[] readBuffer = new Byte[maxBlockSize];
            Int32 read = 0;

            var dataStream = stream;
            var cryptoStream = new MemoryStream();
            try
            {
                while ((read = dataStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    Byte[] toDecrypt = new Byte[read];
                    Array.Copy(readBuffer, 0, toDecrypt, 0, read);
                    var decryptedBuffer = rsa.Decrypt(toDecrypt, RSAEncryptionPadding.Pkcs1);
                    cryptoStream.Write(decryptedBuffer, 0, decryptedBuffer.Length);
                }

                return new XResult<Byte[]>(cryptoStream.ToArray());
            }
            catch (Exception ex)
            {
                return new XResult<Byte[]>(null, ex);
            }
            finally
            {
                dataStream.Dispose();
                cryptoStream.Dispose();
            }
        }

        public XResult<String> MakeSign(String signContent, String privateKeyPem, PrivateKeyFormat privateKeyFormat, HashAlgorithmName algName, String charset)
        {
            if (String.IsNullOrWhiteSpace(signContent))
            {
                return new XResult<String>(null, new ArgumentNullException("signContent is null"));
            }

            if (String.IsNullOrWhiteSpace(privateKeyPem))
            {
                return new XResult<String>(null, new ArgumentNullException("privateKeyPem is null"));
            }

            RSA rsa = null;
            try
            {
                rsa = CreateRSAFromPrivateKey(privateKeyPem, privateKeyFormat);
            }
            catch (Exception ex)
            {
                return new XResult<String>(null, ex);
            }

            if (rsa == null)
            {
                return new XResult<String>(null, new CryptographicException("CreateRSAFromPrivateKey(privateKeyPem) returns null"));
            }

            Byte[] signContentData = null;
            try
            {
                signContentData = Encoding.GetEncoding(charset).GetBytes(signContent);
                var signResult = MakeSign(signContentData, privateKeyPem, privateKeyFormat, algName, charset);
                if (!signResult.Success)
                {
                    return new XResult<String>(null, signResult.FirstException);
                }
                String signString = Convert.ToBase64String(signResult.Value);
                return new XResult<String>(signString);
            }
            catch (Exception ex)
            {
                return new XResult<String>(null, ex);
            }
        }

        public XResult<Byte[]> MakeSign(Byte[] signContent, String privateKeyPem, PrivateKeyFormat privateKeyFormat, HashAlgorithmName algName, String charset)
        {
            if (signContent == null || signContent.Length == 0)
            {
                return new XResult<Byte[]>(null, new ArgumentNullException("signContent is null"));
            }

            if (String.IsNullOrWhiteSpace(privateKeyPem))
            {
                return new XResult<Byte[]>(null, new ArgumentNullException("privateKeyPem is null"));
            }

            RSA rsa = null;
            try
            {
                rsa = CreateRSAFromPrivateKey(privateKeyPem, privateKeyFormat);
            }
            catch (Exception ex)
            {
                return new XResult<Byte[]>(null, ex);
            }

            if (rsa == null)
            {
                return new XResult<Byte[]>(null, new CryptographicException("CreateRSAFromPrivateKey(privateKeyPem) returns null"));
            }

            try
            {
                var signData = rsa.SignData(signContent, algName, RSASignaturePadding.Pkcs1);
                return new XResult<Byte[]>(signData);
            }
            catch (Exception ex)
            {
                return new XResult<Byte[]>(null, ex);
            }
        }

        public XResult<Boolean> VerifySign(String signNeedToVerfy, String signContent, String publicKeyPem, HashAlgorithmName algName, String charset)
        {
            Byte[] signContentData = null;
            Byte[] signNeedToVerifyData = null;
            try
            {
                signContentData = Encoding.GetEncoding(charset).GetBytes(signContent);
                signNeedToVerifyData = Convert.FromBase64String(signNeedToVerfy);
                return VerifySign(signNeedToVerifyData, signContentData, publicKeyPem, algName);
            }
            catch (Exception ex)
            {
                return new XResult<Boolean>(false, ex);
            }
        }

        public XResult<Boolean> VerifySign(Byte[] signNeedToVerify, Byte[] signContent, String publicKeyPem, HashAlgorithmName algName)
        {
            RSA rsa = null;
            try
            {
                rsa = CreateRSAFromPublicKey(publicKeyPem);
            }
            catch (Exception ex)
            {
                return new XResult<Boolean>(false, ex);
            }

            if (rsa == null)
            {
                return new XResult<Boolean>(false, new CryptographicException("CreateRSAFromPublicKey(publicKeyPem) returns null"));
            }

            try
            {
                var result = rsa.VerifyData(signContent, signNeedToVerify, algName, RSASignaturePadding.Pkcs1);
                return new XResult<Boolean>(result);
            }
            catch (Exception ex)
            {
                return new XResult<Boolean>(false, ex);
            }
        }

        private RSA CreateRSAFromPublicKey(String publicKeyPem)
        {
            Byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            Byte[] x509key;
            Byte[] seq = new Byte[15];
            Int32 x509size;

            x509key = Convert.FromBase64String(publicKeyPem.Replace(Environment.NewLine, String.Empty));
            x509size = x509key.Length;

            using (var mem = new MemoryStream(x509key))
            {
                using (var binr = new BinaryReader(mem))
                {
                    Byte bt = 0;
                    UInt16 twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                    {
                        binr.ReadByte();
                    }
                    else if (twobytes == 0x8230)
                    {
                        binr.ReadInt16();
                    }
                    else
                    {
                        return null;
                    }

                    seq = binr.ReadBytes(15);
                    if (!CompareBytearrays(seq, SeqOID))
                    {
                        return null;
                    }

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8103)
                    {
                        binr.ReadByte();
                    }
                    else if (twobytes == 0x8203)
                    {
                        binr.ReadInt16();
                    }
                    else
                    {
                        return null;
                    }

                    bt = binr.ReadByte();
                    if (bt != 0x00)
                    {
                        return null;
                    }

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                    {
                        binr.ReadByte();
                    }
                    else if (twobytes == 0x8230)
                    {
                        binr.ReadInt16();
                    }
                    else
                    {
                        return null;
                    }

                    twobytes = binr.ReadUInt16();
                    Byte lowbyte = 0x00;
                    Byte highbyte = 0x00;

                    if (twobytes == 0x8102)
                    {
                        lowbyte = binr.ReadByte();
                    }
                    else if (twobytes == 0x8202)
                    {
                        highbyte = binr.ReadByte();
                        lowbyte = binr.ReadByte();
                    }
                    else
                    {
                        return null;
                    }

                    Byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    Int32 modsize = BitConverter.ToInt32(modint, 0);

                    Int32 firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {
                        binr.ReadByte();
                        modsize -= 1;
                    }

                    Byte[] modulus = binr.ReadBytes(modsize);

                    if (binr.ReadByte() != 0x02)
                    {
                        return null;
                    }

                    Int32 expbytes = (Int32)binr.ReadByte();
                    Byte[] exponent = binr.ReadBytes(expbytes);

                    var rsa = RSA.Create();
                    var rsaKeyInfo = new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = exponent
                    };
                    rsa.ImportParameters(rsaKeyInfo);
                    return rsa;
                }
            }
        }

        private RSA CreateRSAFromPrivateKey(String privateKeyPem, PrivateKeyFormat privateKeyFormat)
        {
            privateKeyPem = ConvertPrivateKeyWithFormat(privateKeyPem, privateKeyFormat);

            var RSAparams = new RSAParameters();

            var privateKeyBits = Convert.FromBase64String(privateKeyPem);
            using (var binaryReader = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                Byte bt = 0;
                UInt16 twobytes = 0;

                twobytes = binaryReader.ReadUInt16();
                if (twobytes == 0x8130)
                {
                    binaryReader.ReadByte();
                }
                else if (twobytes == 0x8230)
                {
                    binaryReader.ReadInt16();
                }
                else
                {
                    throw new Exception("Unexpected value read binr.ReadUInt16()");
                }

                twobytes = binaryReader.ReadUInt16();
                if (twobytes != 0x0102)
                {
                    throw new Exception("Unexpected version");
                }

                bt = binaryReader.ReadByte();
                if (bt != 0x00)
                {
                    throw new Exception("Unexpected value read binr.ReadByte()");
                }

                RSAparams.Modulus = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
                RSAparams.Exponent = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
                RSAparams.D = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
                RSAparams.P = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
                RSAparams.Q = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
                RSAparams.DP = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
                RSAparams.DQ = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
                RSAparams.InverseQ = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
            }

            var rsa = RSA.Create();
            rsa.ImportParameters(RSAparams);

            return rsa;
        }

        private String ConvertPrivateKeyWithFormat(String privateKeyPem, PrivateKeyFormat privateKeyFormat)
        {
            if (privateKeyFormat == PrivateKeyFormat.PKCS8)
            {
                String pkcs1 = null;
                try
                {
                    pkcs1 = RsaKeyConvert.PrivateKeyPkcs8ToPkcs1(privateKeyPem);
                }
                catch (Exception)
                {
                    return privateKeyPem;
                }

                if (String.IsNullOrWhiteSpace(pkcs1))
                {
                    return privateKeyPem;
                }

                var firstNewLineIndex = pkcs1.IndexOf(Environment.NewLine);
                var lastEndIndex = pkcs1.LastIndexOf("-----END");
                var length = lastEndIndex > 0 ? lastEndIndex - firstNewLineIndex : pkcs1.Length - firstNewLineIndex;

                if (firstNewLineIndex >= 0 && length > 0)
                {
                    privateKeyPem = pkcs1.Substring(firstNewLineIndex, length);
                }
            }

            return privateKeyPem.Replace(Environment.NewLine, String.Empty);
        }

        private Int32 GetIntegerSize(BinaryReader binaryReader)
        {
            Byte bt = 0;
            Byte lowbyte = 0x00;
            Byte highbyte = 0x00;
            Int32 count = 0;

            bt = binaryReader.ReadByte();
            if (bt != 0x02)
            {
                return 0;
            }

            bt = binaryReader.ReadByte();

            if (bt == 0x81)
            {
                count = binaryReader.ReadByte();
            }
            else if (bt == 0x82)
            {
                highbyte = binaryReader.ReadByte();
                lowbyte = binaryReader.ReadByte();
                Byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binaryReader.ReadByte() == 0x00)
            {
                count -= 1;
            }

            binaryReader.BaseStream.Seek(-1, SeekOrigin.Current);

            return count;
        }

        private Boolean CompareBytearrays(Byte[] a, Byte[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }

            Int32 i = 0;
            foreach (Byte c in a)
            {
                if (c != b[i])
                {
                    return false;
                }

                i++;
            }

            return true;
        }
    }
}
