using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayBar.Models
{
    public class UserModel
    {
        [Required]
        public string IMEI { get; set; }
    }
}