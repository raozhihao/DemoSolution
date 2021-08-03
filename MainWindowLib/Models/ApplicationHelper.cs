using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MainWindowLib.Models
{
    /// <summary>
    /// 应用程序帮助类
    /// </summary>
    public static class ApplicationHelper
    {
        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="dialogStyle">格式</param>
        /// <returns></returns>
        public static async Task<MessageDialogResult> ShowAsync(string title, string content, MessageDialogStyle dialogStyle = MessageDialogStyle.Affirmative)
        {
            return await ApplicationWindow.ShowMessageAsync(title, content, dialogStyle);
        }

        /// <summary>
        /// 显示等待窗体
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public static async Task<ProgressDialogController> ShowProgressAsync(string title, string content)
        {
            return await ApplicationWindow.ShowProgressAsync(title, content);
        }


        /// <summary>
        /// 获取当前应用程序 Metro 主窗体
        /// </summary>
        public static MetroWindow ApplicationWindow { get => Application.Current.MainWindow as MetroWindow; }

        /// <summary>
        /// 获取当前应用程序UI同步上下文
        /// </summary>
        public static SynchronizationContext SynchronizationContext { get; set; }

        /// <summary>
        /// 向UI线程发送同步方法
        /// </summary>
        /// <param name="sendOrPost"></param>
        /// <param name="state"></param>
        public static void UIInvokeMethod(SendOrPostCallback sendOrPost, object state = null)
        {
            SynchronizationContext.Send(sendOrPost, state);
        }

        /// <summary>
        /// 向UI线程发送异步方法
        /// </summary>
        /// <param name="sendOrPost"></param>
        /// <param name="state"></param>
        public static void UIBeginInvokeMethod(SendOrPostCallback sendOrPost, object state = null)
        {
            SynchronizationContext.Post(sendOrPost, state);
        }
    }
}
