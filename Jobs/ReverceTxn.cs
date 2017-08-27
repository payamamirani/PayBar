using Jobs.TerminalService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities;

namespace Jobs
{
    public class ReverceTxn : IJob
    {
        TerminalServiceClient client = new TerminalServiceClient();

        public void Do()
        {
            var bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;

            while (true)
            {
                if (!bw.IsBusy)
                    bw.RunWorkerAsync();

                Thread.Sleep(500);
            };
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            var toDo = Data.Models.Generated.PayBar.Txn.Fetch("WHERE RespCode = 0 AND IsSetteled IS NULL AND BusinessDate = GETDATE()");
            foreach (var item in toDo)
            {
                var result = client.ConfirmTxn(TerminalInfo.TerminalUserName, TerminalInfo.TerminalPasswrod, TerminalInfo.TerminalNumber, item.Amount.ToLong(), item.RefNum);
                if (result == 0)
                {
                    item.IsSetteled = true;
                    item.SetteledDate = DateTime.Now;
                    item.Save();
                }
            }
        }
    }
}
