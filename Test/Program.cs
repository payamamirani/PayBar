using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin");

            try
            {
                var bw = new BackgroundWorker();
                bw.WorkerSupportsCancellation = true;
                bw.WorkerReportsProgress = true;
                bw.DoWork += bw_DoWork;
                bw.ProgressChanged += bw_ProgressChanged;
                bw.RunWorkerCompleted += bw_RunWorkerCompleted;

                while (true)
                {
                    Console.WriteLine("Enter Char :");
                    var c = Console.ReadLine();
                    if (c == ".")
                        break;

                    if (c == "B")
                    {
                        if (!bw.IsBusy)
                        {
                            bw.RunWorkerAsync();
                        }
                        else
                        {
                            Console.WriteLine("Background Worker Is Busy Now.");
                        }
                    }

                    if (c == "C")
                    {
                        if (bw.WorkerSupportsCancellation)
                        {
                            bw.CancelAsync();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("End");
            Console.ReadLine();
        }

        static void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled))
                Console.WriteLine("Canceled!");
            else if (!(e.Error == null))
                Console.WriteLine("Error: " + e.Error.Message);
            else
                Console.WriteLine("Done!");
        }

        static void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.Write("\r{0}%\t", e.ProgressPercentage.ToString());
        }

        static void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            for (int i = 1; (i <= 100); i++)
            {
                if ((worker.CancellationPending))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // Perform a time consuming operation and report progress.
                    System.Threading.Thread.Sleep(500);
                    worker.ReportProgress((i));
                }
            }
        }
    }
}
