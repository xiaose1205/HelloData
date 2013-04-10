using System.Diagnostics;

namespace HelloData.FrameWork.AOP
{
    /// <summary>
    /// aop时间截取
    /// </summary>
    public class AopTimer
    {
        readonly Stopwatch _watch = new Stopwatch();
        public void Begin()
        {
            if (!AppCons.LogSqlExcu) return;
            _watch.Reset();
            _watch.Start();
        }
        public void BeginWithMessage(string message)
        {
            if (!AppCons.LogSqlExcu) return;
            _watch.Reset();
            _watch.Start();
            HelloData.FWCommon.Logging.Logger.CurrentLog.Info(message);
        }
        public void LogMessage(string message)
        {
            if (!AppCons.LogSqlExcu) return;
            HelloData.FWCommon.Logging.Logger.CurrentLog.Info(message);
        }

        public void End()
        {
            if (!AppCons.LogSqlExcu) return;
            HelloData.FWCommon.Logging.Logger.CurrentLog.Info(string.Format("耗时: {0} ms\r\n******", _watch.ElapsedMilliseconds));
        }

        public void EndWithMeessage(string message)
        {
            if (!AppCons.LogSqlExcu) return;
            _watch.Stop();
            HelloData.FWCommon.Logging.Logger.CurrentLog.Info(message);
        }
    }
}
