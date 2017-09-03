using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class MethodExtentions
    {
        public static string ToStringx(this object o)
        {
            try
            {
                return o.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static bool ToBoolean(this object o)
        {
            try
            {
                return Convert.ToBoolean(o);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsNotNull(this object o)
        {
            return !IsNull(o);
        }

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

        public static string ToPersian(this DateTime dt)
        {
            try
            {
                PersianCalendar persianCalendar = new PersianCalendar();
                string year = persianCalendar.GetYear(dt).ToString();
                string month = persianCalendar.GetMonth(dt).ToString().PadLeft(2, '0');
                string day = persianCalendar.GetDayOfMonth(dt).ToString().PadLeft(2, '0');
                string hour = dt.Hour.ToString().PadLeft(2, '0');
                string minute = dt.Minute.ToString().PadLeft(2, '0');
                string second = dt.Second.ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2} {3}:{4}:{5}", year, month, day, hour, minute, second);
            }
            catch
            {

                throw;
            }
        }

        public static bool SendNotification(this string message, string title, string player)
        {
            return SendNotification(message, title, new List<string> { player });
        }

        public static bool SendNotification(this string message, string title, List<string> players)
        {
            try
            {
                if (players.IsNull() || players.Count == 0)
                    throw new Exception("Player List Is Null.");

                var client = new RestClient("https://onesignal.com/api/v1/notifications");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    app_id = Consts.APP_ID,
                    contents = new { en = message },
                    headings = new { en = title },
                    include_player_ids = players
                }), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
