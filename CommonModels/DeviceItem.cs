using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GeneralTool.General.WPFHelper;

namespace CommonModels
{
    public class DeviceItem
    {
        public int DeviceID { get; set; }
        public string Name { get; set; }
        public string ToopTip
        {
            get
            {
                return $"{this.DeviceID}\r\n{this.Name}";
            }
        }
        public ICommand DeviceCommand { get; set; }
    }
}
