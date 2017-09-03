using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayBar.Models
{
    public class NotificationModel
    {
        [Required]
        public string Message { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public List<string> Players { get; set; }

        public NotificationModel()
        {
            Players = new List<string>();
        }
    }
}