using GeneralTool.General.Interfaces;

namespace MainWindowLib
{
    /// <summary>
    /// ILog的扩展方法
    /// </summary>
    public static class ILogEx
    {
        /// <summary>
        /// 写入 Info 日志
        /// </summary>
        /// <param name="log"></param>
        /// <param name="e"></param>
        public static void Info(this ILog log, object e) => log.Info(e + "");
        /// <summary>
        /// 写入 Error 日志
        /// </summary>
        /// <param name="log"></param>
        /// <param name="e"></param>
        public static void Error(this ILog log, object e) => log.Error(e + "");
        /// <summary>
        /// 写入 Debug 日志
        /// </summary>
        /// <param name="log"></param>
        /// <param name="e"></param>
        public static void Debug(this ILog log, object e) => log.Debug(e + "");
        /// <summary>
        /// 写入 Fail 日志
        /// </summary>
        /// <param name="log"></param>
        /// <param name="e"></param>
        public static void Fail(this ILog log, object e) => log.Fail(e + "");
    }
}
