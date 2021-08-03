using CommonModels;
using GeneralTool.General.Interfaces;
using GeneralTool.General.WPFHelper;
using MahApps.Metro.Controls.Dialogs;
using MainWindowLib.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MainWindowLib.ViewModels
{
    public class DeviceViewModel : BaseNotifyModel
    {
        public ILog Log { get; set; }
        public ObservableCollection<DeviceItem> DeviceItems { get; set; } = new ObservableCollection<DeviceItem>();

        public DeviceViewModel()
        {
            for (int i = 0; i < 100; i++)
            {
                var device = new DeviceItem() { DeviceID = i + 1, Name = "Device" + i };
                device.DeviceCommand = this.DeviceCommand;
                device.UpdateHeaderCommand = this.UpdateHeaderCommand;
                this.DeviceItems.Add(device);
            }
        }

        public ICommand DeviceCommand
        {
            get
            {
                return new SimpleCommand<DeviceItem>((sender) =>
                {
                    this.Log.Info(sender + "");
                });
            }
        }

        public ICommand UpdateHeaderCommand
        {
            get
            {
                return new SimpleCommand<DeviceItem>(async (sender) =>
                {
                    var result = await ApplicationHelper.ApplicationWindow.ShowInputAsync("New Header", "Input new header name");
                    sender.Name = result;
                });
            }
        }
    }
}
