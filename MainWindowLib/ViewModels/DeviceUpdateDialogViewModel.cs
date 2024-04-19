using GeneralTool.CoreLibrary.WPFHelper;
using MainWindowLib.Models;
using System.Windows.Input;

namespace MainWindowLib.ViewModels
{
    public class DeviceUpdateDialogViewModel : BaseCustomDialogViewModel
    {
        private string name;
        public string Name { get => this.name; set => this.RegisterProperty(ref this.name, value); }

        private string tooltip;
        public string ToolTip { get => this.tooltip; set => this.RegisterProperty(ref this.tooltip, value); }

        public ICommand SubmitCommand { get => new SimpleCommand(SubmitMethod); }

        private async void SubmitMethod()
        {
            this.DialogResult = true;
            await this.CloseAsync();
        }
    }
}
