using CommonModels;
using GeneralTool.General.Interfaces;
using MainWindowLib.Windows;
using System;
using System.Windows;

namespace MainWindowLib
{
    /// <summary>
    /// 消息通知面板
    /// </summary>
    public static class NoticeGlobal
    {
        private static int maxNoticeCount = 5;
        /// <summary>
        /// 获取或设置能够打开的通知面板最大数量,默认为5
        /// </summary>
        public static int MaxNoticCount
        {
            get => maxNoticeCount;
            set
            {
                maxNoticeCount = value;
                if (ParentWindow != null)
                    ParentWindow.MaxCount = maxNoticeCount;

            }
        }

        /// <summary>
        /// 设置或获取日志组件
        /// </summary>
        public static ILog Log { get; set; }

        /// <summary>
        /// 设置或获取是否显示日志(默认为true)
        /// </summary>
        public static bool ShowLog { get; set; } = true;

        /// <summary>
        /// 已开启面板事件
        /// </summary>
        public static event Action<NoticeModel> OpenGrowEvent;

        private static NoticeWindow ParentWindow;
        static NoticeGlobal()
        {
            if (Application.Current == null)
            {
                throw new Exception("Application.Current.MainWindow 为null,无法使用此类");
            }

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Application.Current.Exit += Current_Exit;
            }));

        }

        private static void Current_Exit(object sender, ExitEventArgs e)
        {
            ParentWindow?.Close();
        }


        /// <summary>
        /// 打开面板
        /// </summary>
        /// <param name="model">面板信息</param>
        public static void GrowInfo(NoticeModel model)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (ParentWindow == null)
                {
                    ParentWindow = new NoticeWindow();
                    ParentWindow.Init();
                    ParentWindow.Closed += ParentWindow_Closed;
                    ParentWindow.Show();

                }

                var logMessage = $"全局通知 [{model.GrowType}] : {model.Message}";
                if (ShowLog)
                {
                    switch (model.GrowType)
                    {
                        case GrowType.Info:
                            Log?.Info(logMessage);
                            break;
                        case GrowType.Waring:
                            Log?.Waring(logMessage);
                            break;
                        case GrowType.Erro:
                            Log?.Error(logMessage);
                            break;
                    }
                }
                else Log?.Debug(logMessage);

                ParentWindow.MaxCount = MaxNoticCount;
                ParentWindow.SetInfo(model);
                OpenGrowEvent?.Invoke(model);
            }));

        }

        private static void ParentWindow_Closed(object sender, EventArgs e)
        {
            ParentWindow = null;
        }

        /// <summary>
        /// 信息面板
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message) => GrowInfo(new NoticeModel(message, TimeSpan.FromSeconds(5)));

        /// <summary>
        /// 警告面板
        /// </summary>
        /// <param name="message"></param>
        public static void Waring(string message) => GrowInfo(new NoticeModel(message, TimeSpan.FromSeconds(5), GrowType.Waring));

        /// <summary>
        /// 错误面板
        /// </summary>
        /// <param name="message"></param>
        public static void Erro(string message) => GrowInfo(new NoticeModel(message, TimeSpan.FromSeconds(5), GrowType.Erro));

        /// <summary>
        /// 信息面板(不关闭)
        /// </summary>
        /// <param name="message"></param>
        public static void InfoNotAutoClosed(string message) => GrowInfo(new NoticeModel(message, TimeSpan.Zero));

        internal static void Clear()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                ParentWindow?.Close();
            }));
        }
    }
}
