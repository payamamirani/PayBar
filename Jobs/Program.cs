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
        static void Main(string[] args)
        {
            Console.WriteLine("Begin");

            ServicePointManager.ServerCertificateValidationCallback += (sender1, certificate, chain, sslPolicyErrors) => true;
            try
            {
                var jobs = Data.Models.Generated.PayBar.VwJob.Fetch("WHERE 1 = 1");

                foreach (var item in jobs)
                    Task.Run(() => CreateInstanseForJob(item));
                while (true)
                {
                    Thread.Sleep(60000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("End");
            Console.ReadLine();
        }

        static void CreateInstanseForJob(Data.Models.Generated.PayBar.VwJob item)
        {
            Type type = Type.GetType(item.ClassName);
            object instance = Activator.CreateInstance(type);
            MethodInfo method = instance.GetType().GetMethod("Do");
            object[] data = new object[0];
            Console.WriteLine(item.ClassName);
            object result = method.Invoke(instance, data);
        }
    }
}
