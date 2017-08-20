using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayBar.Models
{
    public class Result
    {
        public bool success { get; set; }
        public string error_message { get; set; }
        public dynamic data { get; set; }
    }
}