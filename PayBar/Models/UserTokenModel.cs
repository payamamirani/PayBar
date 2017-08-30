using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayBar.Models
{
    public class UserTokenModel : UserModel
    {
        [Required]
        public string Token { get; set; }
    }
}