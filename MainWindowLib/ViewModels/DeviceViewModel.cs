using CommonModels;
using GeneralTool.General.Interfaces;
using GeneralTool.General.WPFHelper;
using MahApps.Metro.Controls.Dialogs;
using MainWindowLib.Models;
using System.Collections.ObjectModel;
using System.Linq;
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
                device.ToolTip = $"{device.DeviceID} - {device.Name}";
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
                    //var result = await ApplicationHelper.ApplicationWindow.ShowInputAsync("New Header", "Input new header name");
                    //sender.Name = result;

                    //打开修改框
                    var model = new DeviceUpdateDialogViewModel
                    {
                        Name = sender.Name,
                        ToolTip = sender.ToolTip
                    };
                    await ApplicationHelper.ShowCustomDialogAsync(new CustomDialogs.DeviceUpdateView(), model, $"更改设备信息");
                    if (model.DialogResult)
                    {
                        var first = this.DeviceItems.FirstOrDefault(d => d.Name == model.Name);
                        if (first != null && first != sender)
                        {
                            await ApplicationHelper.ApplicationWindow.ShowMessageAsync("提示", $"设备名称 {model.Name} 已存在");
                            return;
                        }
                        sender.Name = model.Name;
                        sender.ToolTip = model.ToolTip;
                    }
                });
            }
        }
    }


}
