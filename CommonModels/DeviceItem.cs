﻿using GeneralTool.General.WPFHelper;
using System.Windows.Input;

namespace CommonModels
{
    public class DeviceItem : BaseNotifyModel
    {
        private int deviceId;
        public int DeviceID { get => this.deviceId; set => this.RegisterProperty(ref this.deviceId, value); }

        private string name;
        public string Name { get => this.name; set => this.RegisterProperty(ref this.name, value); }
        public string ToopTip
        {
            get
            {
                return $"{this.DeviceID}\r\n{this.Name}";
            }
        }
        public ICommand DeviceCommand { get; set; }

        public ICommand UpdateHeaderCommand { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
