using PayBar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using Utilities;

namespace PayBar.Filtters
{
    public class BasicAuthHttpModule : IHttpModule
    {
        private const string Realm = "My Realm";

        public void Init(HttpApplication context)
        {
            // Register event handlers
            context.AuthenticateRequest += OnApplicationAuthenticateRequest;
            context.EndRequest += OnApplicationEndRequest;
        }

        private static void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;
            var authHeader = request.Headers["Authorization"];
            if (authHeader == null)
                AccessDenied();

            var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

            // RFC 2617 sec 1.2, "scheme" name is case-insensitive
            if (authHeaderVal.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) && authHeaderVal.Parameter != null)
            {
                AuthenticateUser(authHeaderVal.Parameter);
            }
        }

        private static void OnApplicationEndRequest(object sender, EventArgs e)
        {
            var response = HttpContext.Current.Response;
            if (response.StatusCode == 401)
            {
                response.Headers.Add("WWW-Authenticate",
                    string.Format("Basic realm=\"{0}\"", Realm));
            }
        }

        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        private static void AuthenticateUser(string credentials)
        {
            try
            {
                credentials = Encoding.GetEncoding("iso-8859-1").GetString(Convert.FromBase64String(credentials)).Decrypt();

                int separator = credentials.IndexOf(':');
                string cellno = credentials.Substring(0, separator);
                string imei = credentials.Substring(separator + 1);

                if (CheckPassword(cellno, imei))
                {
                    var identity = new GenericIdentity(cellno);
                    SetPrincipal(new GenericPrincipal(identity, null));
                }
                else
                {
                    // Invalid username or password.
                    AccessDenied();
                }
            }
            catch (FormatException)
            {
                // Credentials were not formatted correctly.
                AccessDenied();
            }
        }

        public static void AccessDenied()
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.StatusCode = 401;
            HttpContext.Current.Response.Output.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Result { success = false, error_message = "Access is denied.", data = null }));
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.End();
        }

        // TODO: Here is where you would validate the username and password.
        private static bool CheckPassword(string cellno, string imei)
        {
            return true;
        }

        public void Dispose()
        {
        }
    }
}