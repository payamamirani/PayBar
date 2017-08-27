using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayBar.Models
{
    public class TxnModel
    {
        public long Amount { get; set; }
        public string CardNo { get; set; }
        public string CVV2 { get; set; }
        public string ExpDate { get; set; }
        public string Pin { get; set; }
    }
}