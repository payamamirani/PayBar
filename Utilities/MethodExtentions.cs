using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class MethodExtentions
    {
        public static bool IsNull(this object o)
        {
            try
            {
                if (o == null)
                    return true;
                return false;
            }
            catch
            {
                throw;
            }
        }

        public static bool IsNull(this string s)
        {
            try
            {
                if (String.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
                    return true;
                return false;
            }
            catch
            {
                throw;
            }
        }

        public static string GenerateKey()
        {
            var str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$%^&*()_-=+";
            var rand = new Random();
            var key = "";

            for (int i = 0; i < 16; i++)
                key += str.Substring(rand.Next(str.Length), 1);

            return key;
        }

        public static string GenerateToken()
        {
            var str = "123456789";
            var rand = new Random();
            var key = "";

            for (int i = 0; i < 5; i++)
                key += str.Substring(rand.Next(str.Length), 1);

            return key;
        }

        public static long ToLong(this object o)
        {
            try
            {
                return Convert.ToInt64(o);
            }
            catch
            {
                return 0;
            }
        }

        public static long? ToNullableLong(this object o)
        {
            try
            {
                return Convert.ToInt64(o);
            }
            catch
            {
                return null;
            }
        }

        public static int ToInt(this object o)
        {
            try
            {
                return Convert.ToInt32(o);
            }
            catch
            {
                return 0;
            }
        }

        public static int? ToNullableInt(this object o)
        {
            try
            {
                return Convert.ToInt32(o);
            }
            catch
            {
                return null;
            }
        }
    }
}
