using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Utilities;
using Data.Models.Generated.PayBar;

namespace PayBar.Models
{
    public class MerchantEnc
    {
        public string ID { get; set; }
        public string CellNo { get; set; }
        public string IMEI { get; set; }
    }

    public class DataApiModel<T>
        where T : class
    {
        private string Password = "";

        [Required]
        public string CellNo { get; set; }

        [Required]
        public string Data { get; set; }

        public Data.Models.Generated.PayBar.User CarllerUser
        {
            get
            {
                if (typeof(T) == typeof(Data.Models.Generated.PayBar.User))
                    return null;

                var u = User.FirstOrDefault("where cellno = @0", CellNo);
                Password = u.MasterKey.Decrypt();
                return u;
            }
        }

        public T DecryptData
        {
            get
            {
                var a = Password.IsNull() ? Data.Decrypt() : Data.Decrypt(Password);

                if (typeof(T) == typeof(Data.Models.Generated.PayBar.Merchant))
                {
                    var merchantId = Newtonsoft.Json.JsonConvert.DeserializeObject<MerchantEnc>(a).ID.Decrypt();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(new { ID = merchantId }));
                }

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(a);
            }
        }
    }
}