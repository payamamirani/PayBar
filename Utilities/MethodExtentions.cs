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
            var str = "0123456789ABCDEF";
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
    }
}
