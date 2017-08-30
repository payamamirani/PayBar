using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class AES
    {
        private static byte[] _KEY(string p)
        {
            return Encoding.UTF8.GetBytes(p);
        }

        private static byte[] _IV(string i)
        {
            return Encoding.UTF8.GetBytes(i);
        }

        public static string Encrypt(this string Text, string Pass = Key.PASSWORD, string IV = Key.IV)
        {
            try
            {
                if (Pass.Length != 16)
                    throw new Exception("طول رمز نادرست است.");

                if (IV.Length != 16)
                    throw new Exception("طول IV نادرست است.");

                var rijndaelCipher = new RijndaelManaged
                {
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7,
                    KeySize = 128,
                    BlockSize = 128,
                    Key = _KEY(Pass),
                    IV = _IV(IV)
                };

                var transform = rijndaelCipher.CreateEncryptor();

                var plainText = Encoding.UTF8.GetBytes(Text);

                var cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);

                return Convert.ToBase64String(cipherBytes);
            }
            catch
            {
                throw new Exception("خطا در رمزنگاری داده ها.");
            }
        }

        public static string Decrypt(this string Text, string Pass = Key.PASSWORD, string IV = Key.IV)
        {
            try
            {
                if (Pass.Length != 16 && Pass.Length != 32)
                    throw new Exception("طول رمز نادرست است.");

                if (IV.Length != 16)
                    throw new Exception("طول IV نادرست است.");

                var rijndaelCipher = new RijndaelManaged
                {
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7,
                    KeySize = 128,
                    BlockSize = 128,
                    Key = _KEY(Pass),
                    IV = _IV(IV)
                };

                var encryptedData = Convert.FromBase64String(Text);

                var transform = rijndaelCipher.CreateDecryptor();

                var plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                return Encoding.UTF8.GetString(plainText);
            }
            catch
            {
                throw new Exception("خطا در بازیابی داده رمز شده.");
            }
        }
    }
}