using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities;

namespace Jobs
{
    class Program
    {

        static List<string> RunningTask = new List<string>();

        static void Main(string[] args)
        {
            Console.WriteLine("Begin");

            ServicePointManager.ServerCertificateValidationCallback += (sender1, certificate, chain, sslPolicyErrors) => true;
            try
            {
                var jobs = Data.Models.Generated.PayBar.VwJob.Fetch("WHERE IsActive = 1");

                foreach (var item in jobs)
                    Console.WriteLine(item.ClassName);

                while (true)
                {
                    foreach (var job in jobs)
                    {
                        var task = string.Format("{0}#{1}", job.ClassName, "Do");
                        if (!RunningTask.Contains(task))
                        {
                            Task.Run(() => CreateInstanseForJob(job));
                            RunningTask.Add(task);
                        }
                    }

                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("End");
            Console.ReadLine();
        }

        static void CreateInstanseForJob(Data.Models.Generated.PayBar.VwJob job)
        {
            try
            {
                Type type = Type.GetType(job.ClassName);
                object instance = Activator.CreateInstance(type);
                MethodInfo method = instance.GetType().GetMethod("Do");
                object[] data = new object[0];
                object result = method.Invoke(instance, data);

                var task = string.Format("{0}#{1}", job.ClassName, "Do");
                RunningTask.Remove(task);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                LogBiz.SaveError("CreateInstanseForJob", ex);
            }
        }
    }
}
