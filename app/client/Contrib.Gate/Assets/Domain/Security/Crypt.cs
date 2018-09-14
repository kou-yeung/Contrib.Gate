using System.Collections.Generic;
using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Security
{
    public class Crypt
    {
        static byte[] crypt;
        static int[] iv;
        static int[] key;

        public static bool Ready()
        {
            return crypt != null;
        }
        public static void Init(long timestamp, int[] iv, int[] key)
        {
            Crypt.crypt = CryptKeys(timestamp);
            Crypt.iv = iv;
            Crypt.key = key;
        }

        static byte[] CryptKeys(long timestamp)
        {
            var s = (timestamp % 1000).ToString();
            List<byte> res = new List<byte>();
            for (int i = s.Length - 1; i >= 0; i--)
            {
                res.Add(Convert.ToByte(s[i]));
            }
            return res.ToArray();
        }

        public static string Key
        {
            get
            {
                return Decrypt(key);
            }
        }
        public static string IV
        {
            get
            {
                return Decrypt(iv);
            }
        }

        static string Decrypt(int[] value)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
                sb.Append(Convert.ToChar(value[i] ^ crypt[i % crypt.Length]));
            }
            return sb.ToString();
        }

        public static string Decrypt(string base64)
        {
            if (string.IsNullOrEmpty(base64)) return "";

            using (AesManaged manager = new AesManaged())
            {
                manager.Key = Convert.FromBase64String(Key);
                manager.IV = Convert.FromBase64String(IV);

                var decryptor = manager.CreateDecryptor(manager.Key, manager.IV);

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}

