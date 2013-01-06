using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.FrameWork.Logging
{
    public interface ILog
    {
        void Debug(string message);
        void Debug(string message, Exception exception);
        void Error(string message);
        void Error(string message, Exception exception);
        void Info(string message);
        void Info(string message, Exception exception);
        void InitName(string name);
        void InitName(Type type);
        void Warn(string message);
        void Warn(string message, Exception exception);
    }
}
