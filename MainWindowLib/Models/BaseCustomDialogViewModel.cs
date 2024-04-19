using GeneralTool.CoreLibrary.WPFHelper;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MainWindowLib.Models
{
    /// <summary>
    /// 基础自定义用户控件DataContext
    /// </summary>
    public abstract class BaseCustomDialogViewModel : BaseNotifyModel
    {
        /// <summary>
        /// 设置或获取基础对话框
        /// </summary>
        public BaseMetroDialog BaseMetroDialog { get; set; }

        /// <summary>
        /// 对话框显示类型
        /// </summary>
        public CustomDialogType CustomDialogType { get; set; }

        /// <summary>
        /// 关闭Command
        /// </summary>
        public ICommand CloseCommand { get => new SimpleCommand(async () => { await this.CloseAsync(); }); }

        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool DialogResult { get; protected set; }

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync()
        {
            if (this.BaseMetroDialog == null)
                return;
            if (this.CustomDialogType == CustomDialogType.Show)
                await BaseMetroDialog.RequestCloseAsync();
            else
                await ApplicationHelper.ApplicationWindow.HideMetroDialogAsync(this.BaseMetroDialog);
            ApplicationHelper.HaveCutomShow = false;
        }
    }

    public enum CustomDialogType
    {
        Dialog,
        Show
    }
}
