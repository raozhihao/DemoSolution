using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MainWindowLib.Models
{
    /// <summary>
    /// 应用程序帮助类
    /// </summary>
    public static class ApplicationHelper
    {
        /// <summary>
        /// 是否有非模态窗口存在
        /// </summary>
        public static bool HaveCutomShow { get; set; }
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
        /// 显示自定义用户窗体
        /// </summary>
        /// <param name="userControl">用户窗体</param>
        /// <param name="viewModel">用户窗体所对应的DataContext</param>
        /// <param name="title">标题</param>
        /// <param name="dialogHeightAuto"></param>
        /// <param name="dialogSettings">设置</param>
        /// <param name="dialogType">显示模式</param>
        /// <returns></returns>
        public static async Task ShowCustomDialogAsync(UserControl userControl, BaseCustomDialogViewModel viewModel = null, string title = null, bool dialogHeightAuto = true, MetroDialogSettings dialogSettings = null, CustomDialogType dialogType = CustomDialogType.Dialog)
        {
            void dialogManagerOnDialogOpened(object o, DialogStateChangedEventArgs args)
            {
                DialogManager.DialogOpened -= dialogManagerOnDialogOpened;
            }

            DialogManager.DialogOpened += dialogManagerOnDialogOpened;

            void dialogManagerOnDialogClosed(object o, DialogStateChangedEventArgs args)
            {
                DialogManager.DialogClosed -= dialogManagerOnDialogClosed;
            }

            DialogManager.DialogClosed += dialogManagerOnDialogClosed;

            if (dialogSettings == null)
                dialogSettings = new MetroDialogSettings() { MaximumBodyHeight = ApplicationWindow.ActualHeight };

            dialogSettings.OwnerCanCloseWithDialog = true;
            var dialog = new CustomDialog(dialogSettings) { Content = userControl, Title = title };
            if (viewModel != null)
            {
                userControl.DataContext = viewModel;
                viewModel.BaseMetroDialog = dialog;
                viewModel.CustomDialogType = dialogType;
            }

            if (!dialogHeightAuto)
                dialog.Height = ApplicationWindow.ActualHeight;

            if (dialogType == CustomDialogType.Dialog)
            {
                await ApplicationWindow.ShowMetroDialogAsync(dialog);
                await dialog.WaitUntilUnloadedAsync();
            }
            else
            {
                //非模态窗体,则使用用户控件的宽
                dialog.ShowDialogExternally();
                HaveCutomShow = true;
            }

        }
        /// <summary>
        /// 获取当前应用程序 Metro 主窗体
        /// </summary>
        public static MetroWindow ApplicationWindow { get => Application.Current.MainWindow as MetroWindow; }


        /// <summary>
        /// 向UI线程发送同步方法
        /// </summary>
        public static void UIInvokeMethod(Action action)
        {
            //SynchronizationContext.Send(sendOrPost, state);
            Application.Current.Invoke(action);
        }

        /// <summary>
        /// 向UI线程发送异步方法
        /// </summary>
        public static void UIBeginInvokeMethod(Action action)
        {
            //SynchronizationContext.Post(sendOrPost, state);
            Application.Current.BeginInvoke(action);
        }
    }
}
