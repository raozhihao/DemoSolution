using System;

namespace CommonModels
{
    /// <summary>
    /// 通知面板设置
    /// </summary>
    public struct NoticeModel
    {
        /// <summary>
        /// 要通知的消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 关闭超时时间(如为Zero则表示不关闭)
        /// </summary>
        public TimeSpan TimeOutClosed { get; set; }
        /// <summary>
        /// 通知类型
        /// </summary>
        public GrowType GrowType { get; set; }
        /// <summary>
        /// 是否显示关闭按钮
        /// </summary>
        public bool ShowClosed { get; set; }
        /// <summary>
        /// 是否显示当前时间
        /// </summary>
        public bool ShowCurrentTime { get; set; }

        public NoticeModel(string msg, TimeSpan time, GrowType growType = GrowType.Info, bool showClosed = true, bool showTime = true)
        {
            this.Message = msg;
            this.TimeOutClosed = time;
            this.GrowType = growType;
            this.ShowClosed = showClosed;
            this.ShowCurrentTime = showTime;
        }
    }


    public enum GrowType
    {
        Info,
        Waring,
        Erro
    }
}
