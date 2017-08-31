using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayBar.Models
{
    public class MerchantModel
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string Family { get; set; }

        public string EnName { get; set; }

        public string EnFamily { get; set; }

        public string FullName { get { return string.Format("{0} {1}", this.Name, this.Family); } }

        public string EnFullName { get { return string.Format("{0} {1}", this.EnName, this.EnFamily); } }

        public string PicUrl { get; set; }

        public string Mcc { get; set; }

        public string CellNo { get; set; }

        public string Address { get; set; }
    }
}