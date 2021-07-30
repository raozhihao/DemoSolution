using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralTool.General.WPFHelper;
using CommonModels;
using System.Windows.Input;

namespace MainWindowLib.ViewModels
{
   public  class DeviceViewModel:BaseNotifyModel
    {
        public ObservableCollection<DeviceItem> DeviceItems { get; set; }= new ObservableCollection<DeviceItem>();

        public DeviceViewModel()
        {
            for (int i = 0; i < 100; i++)
            {
                var device = new DeviceItem() { DeviceID = i + 1, Name = "Device" + i };
                device.DeviceCommand = this.DeviceCommand;
                this.DeviceItems.Add(device);
            }
        }

        public ICommand DeviceCommand
        {
            get
            {
                return new SimpleCommand<DeviceItem>((sender) =>
                {

                });
            }
        }
    }
}
