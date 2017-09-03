using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class LogBiz
    {
        private static bool IsSaveExpLog
        {
            get
            {
                try
                {
                    var isSaveExpLog = System.Configuration.ConfigurationManager.AppSettings.Get("IsSaveExpLog").ToStringx();
                    if (isSaveExpLog.IsNull())
                        return false;

                    return isSaveExpLog.ToBoolean();
                }
                catch
                {
                    return false;
                }
            }
        }

        private static bool IsSaveInfoLog
        {
            get
            {
                try
                {
                    var isSaveInfoLog = System.Configuration.ConfigurationManager.AppSettings.Get("IsSaveInfoLog").ToStringx();
                    if (isSaveInfoLog.IsNull())
                        return false;

                    return isSaveInfoLog.ToBoolean();
                }
                catch
                {
                    return false;
                }
            }
        }

        private static string ExceptionLogAddress
        {
            get
            {
                var filename = string.Format("Exception-{0}-{1}-{2}.log", DateTime.Now.Year, DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString().PadLeft(2, '0'));
                try
                {
                    var logAddress = System.Configuration.ConfigurationManager.AppSettings.Get("ExceptionLogAddress").ToStringx();
                    if (logAddress.IsNull())
                        return @"C:/Logs/Exp/" + filename;
                    return Path.GetDirectoryName(logAddress) + @"/" + filename;
                }
                catch
                {
                    return @"C:/Logs/Exp/" + filename;
                }
            }
        }

        private static string InfoLogAddress
        {
            get
            {
                var filename = string.Format("Info-{0}-{1}-{2}.log", DateTime.Now.Year, DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString().PadLeft(2, '0'));
                try
                {
                    var logAddress = System.Configuration.ConfigurationManager.AppSettings.Get("InfoLogAddress").ToStringx();
                    if (logAddress.IsNull())
                        return @"C:/Logs/Info/" + filename;
                    return Path.GetDirectoryName(logAddress) + @"/" + filename;
                }
                catch
                {
                    return @"C:/Logs/Info/" + filename;
                }
            }
        }

        private static void Init(string Address)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(Address)))
                    Directory.CreateDirectory(Path.GetDirectoryName(Address));
            }
            catch
            {
            }
        }

        private static void SaveLog(string Address, string msg)
        {
            try
            {
                if (File.Exists(Address))
                    File.AppendAllText(Address, msg, Encoding.UTF8);
                else
                    File.WriteAllText(Address, msg, Encoding.UTF8);
            }
            catch
            {
            }
        }

        private static string PrepareErrorMessage(string Header, Exception ex)
        {
            try
            {
                var msg = Environment.NewLine + "********** Begin Exception **********" + Environment.NewLine + Environment.NewLine;
                msg += "Gregorian Date Time = " + DateTime.Now.ToString() + Environment.NewLine;
                msg += "Jalali Date Time = " + DateTime.Now.ToPersian() + Environment.NewLine;
                msg += Header + Environment.NewLine;
                msg += string.Format("Message : {0}{1}", ex.Message, Environment.NewLine);
                msg += string.Format("Inner Message : {0}{1}", ex.InnerException.IsNotNull() ? ex.InnerException.Message : string.Empty, Environment.NewLine);
                msg += string.Format("Stack : {0}{1}", ex.StackTrace, Environment.NewLine);
                msg += Environment.NewLine + "********** End Exception **********" + Environment.NewLine + Environment.NewLine;
                return msg;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string PrepareInfoMessage(string Messge)
        {
            try
            {
                var msg = Environment.NewLine + "********** Begin Info **********" + Environment.NewLine;
                msg += "Gregorian Date Time = " + DateTime.Now.ToString() + Environment.NewLine;
                msg += "Jalali Date Time = " + DateTime.Now.ToPersian() + Environment.NewLine;
                msg += Messge + Environment.NewLine;
                msg += Environment.NewLine + "********** End Info **********" + Environment.NewLine + Environment.NewLine;
                return msg;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static void SaveError(string Header, Exception ex)
        {
            try
            {
                if (IsSaveExpLog)
                {
                    Init(ExceptionLogAddress);
                    var msg = PrepareErrorMessage(Header, ex);
                    SaveLog(ExceptionLogAddress, msg);
                }
            }
            catch
            {
            }
        }

        public static void SaveInfo(string Message)
        {
            try
            {
                if (IsSaveInfoLog)
                {
                    Init(InfoLogAddress);
                    var msg = PrepareInfoMessage(Message);
                    SaveLog(InfoLogAddress, msg);
                }
            }
            catch
            {
            }
        }
    }
}
