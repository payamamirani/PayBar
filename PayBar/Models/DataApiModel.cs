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

        public T DecryptData
        {
            get
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Data.Decrypt());
            }
        }
    }
}