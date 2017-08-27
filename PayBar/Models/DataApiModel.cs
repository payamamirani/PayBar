using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Utilities;
using Data.Models.Generated.PayBar;
using Newtonsoft.Json;

namespace PayBar.Models
{
    public class DataApiModel<T>
        where T : class
    {
        private string Password = "";
        private string TempPassword = "";

        [Required]
        public string CellNo { get; set; }

        [Required]
        public string Data { get; set; }

        private Data.Models.Generated.PayBar.User _user;

        public Data.Models.Generated.PayBar.User CallerUser
        {
            get
            {
                if (typeof(T) == typeof(Data.Models.Generated.PayBar.User))
                    return null;

                if (_user == null)
                    _user = User.FirstOrDefault("where cellno = @0", CellNo);

                Password = _user.MasterKey.Decrypt();

                return _user;
            }
        }

        public T DecryptData
        {
            get
            {
                if (typeof(T) == typeof(TxnModel))
                {
                    if (CallerUser.TxnKey.IsNull())
                        throw new Exception("کلید تراکنش وجود ندارد.");
                    TempPassword = CallerUser.TxnKey.Decrypt(CallerUser.MasterKey.Decrypt());
                }

                var a = Password.IsNull() ? Data.Decrypt() : (TempPassword.IsNull() ? Data.Decrypt(Password) : Data.Decrypt(Password).Decrypt(TempPassword));

                if (typeof(T) == typeof(Data.Models.Generated.PayBar.Merchant))
                {
                    var merchantId = JsonConvert.DeserializeObject<MerchantQrModel>(a).ID.Decrypt();
                    return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(new { ID = merchantId }));
                }

                return JsonConvert.DeserializeObject<T>(a);
            }
        }
    }
}