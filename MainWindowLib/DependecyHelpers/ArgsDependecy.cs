using GeneralTool.General.Models;
using System.Linq;
using System.Text;
using System.Windows;

namespace MainWindowLib.DependecyHelpers
{
    public class ArgsDependecy : DependencyObject
    {
        public static readonly DependencyProperty ArgsProperty = DependencyProperty.RegisterAttached("Args", typeof(object), typeof(ArgsDependecy), new PropertyMetadata(ArgsChanged));

        public static void SetArgs(DependencyObject d, object value) => d.SetValue(ArgsProperty, value);

        public static object GetArgs(DependencyObject d) => d.GetValue(ArgsProperty);
        private static void ArgsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null && e.NewValue is DoTaskParameterItem value)
            {
                SetSocketArgs(value);
            }
        }

        private static void Value_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetSocketArgs(sender);
        }

        private static void SetSocketArgs(object sender)
        {
            var value = sender as DoTaskParameterItem;
            value.PropertyChanged -= Value_PropertyChanged;
            var builder = new StringBuilder();
            var url = CommonLibrary.IniSettings.WindowNode.ServerIP.Value + ":" + CommonLibrary.IniSettings.WindowNode.ServerPort.Value + "/";
            builder.Append(@"http://" + url + value.Url);

            var list = value.Paramters;
            if (value.HttpMethod == GeneralTool.General.NetHelper.HttpMethod.GET && list.Count > 0)
                builder.Append("?");

            if (list.Count > 0 && value.HttpMethod == GeneralTool.General.NetHelper.HttpMethod.GET)
            {
                var listStr = list.Select(p =>
                {
                    return string.Format("{0}={1}", p.ParameterName, p.Value);
                });
                builder.Append(string.Join("&", listStr));
            }

            value.SocketArgs = builder.ToString();
            value.PropertyChanged += Value_PropertyChanged;
        }
    }
}
