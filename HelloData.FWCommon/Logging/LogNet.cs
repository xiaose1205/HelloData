using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
namespace HelloData.FWCommon.Logging
{
    public class LogNet : ILog
    {
        private static log4net.ILog _loggerError = log4net.LogManager.GetLogger("logerror");
        private static log4net.ILog _loggerInfo = log4net.LogManager.GetLogger("loginfo");
        public void Debug(string message)
        {
            if (!string.IsNullOrEmpty(message))
                _loggerInfo.Debug(message);
        }

        public void Debug(string message, Exception exception)
        {
            if (!string.IsNullOrEmpty(message))
                _loggerInfo.Debug(message, exception);
        }

        public void Error(string message)
        {
            if (!string.IsNullOrEmpty(message))
                _loggerError.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            if (!string.IsNullOrEmpty(message))
                _loggerError.Error(message, exception);
        }

        public void Info(string message)
        {
            if (!string.IsNullOrEmpty(message))
                _loggerInfo.Info(message);

        }

        public void Info(string message, Exception exception)
        {
            if (!string.IsNullOrEmpty(message))
                _loggerInfo.Info(message, exception);
        }

        public void InitName(string name)
        {

        }

        public void InitName(Type type)
        {

        }

        public void Warn(string message)
        {
            if (!string.IsNullOrEmpty(message))
                _loggerError.Warn(message);
        }

        public void Warn(string message, Exception exception)
        {
            if (!string.IsNullOrEmpty(message))
                _loggerError.Warn(message, exception);
        }


        public void InfoWithSql(string mseeage)
        {

        }
    }
}
