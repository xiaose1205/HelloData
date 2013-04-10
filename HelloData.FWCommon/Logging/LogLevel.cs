using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.FWCommon.Logging
{
    [Flags]
    public enum LogLevel
    {
        All = 8,
        Debug = 0,
        Error = 1,
        Info = 2,
        Warn = 4
    }
}
