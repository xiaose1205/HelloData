namespace HelloData.FWCommon.Logging
{
    /// <summary>
    /// 日志操作类
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// 唯一实例
        /// </summary>
        class Currentset
        {
            static Currentset()
            {
            }
            internal static readonly Logger Instance = new Logger();
        }
        public static Logger Current
        {
            get { return Currentset.Instance; }
        }
        public static ILog CurrentLog
        {
            get { return Currentset.Instance.CurrentLogger; }
        }

        /// <summary>
        /// 设置当前配置的日志（需要继承ILog接口）
        /// </summary>
        /// <returns> </returns>
        public ILog SetLogger
        {
            get { return CurrentLogger; }
            set { CurrentLogger = value; }
        }

        /// <summary>
        /// 当前的日志
        /// </summary>
        private ILog CurrentLogger { get; set; }
        /// <summary>
        /// 是否启动日志
        /// </summary>
        public bool IsOpenLog { get; set; }
    }
}
