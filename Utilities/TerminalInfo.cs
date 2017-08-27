using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class TerminalInfo
    {
        private const int _TerminalNo = 33972490;
        private const string _TermUser = "INTEK";
        private const string _TermPass = "MOBIN";
        private const int _PosCondition = 59;

        public static int TerminalNumber { get { return _TerminalNo; } }

        public static string TerminalUserName { get { return _TermUser; } }

        public static string TerminalPasswrod { get { return _TermPass; } }
        public static int TerminalTypeCode { get { return _PosCondition; } }

    }
}
