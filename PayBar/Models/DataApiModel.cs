using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Utilities;

namespace PayBar.Models
{
    public class DataApiModel<T> where T : class
    {
        [Required]
        public string Data { get; set; }

        //public Data.Models.Generated.PayBar.User User
        //{
        //    get
        //    {
        //        return Newtonsoft.Json.JsonConvert.DeserializeObject<Data.Models.Generated.PayBar.User>(AuthenticationHeaderValue.Parse(HttpContext.Current.Request.Headers["Authorization"]))
        //    }
        //}

        public T DecryptData
        {
            get
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Data.Decrypt());
            }
        }
    }
}