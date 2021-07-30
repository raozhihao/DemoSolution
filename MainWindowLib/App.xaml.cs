using Microsoft.Win32;
using System;
using System.Net.NetworkInformation;
using System.Windows;

namespace MainWindowLib
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (this.MainWindow == null)
            {
                this.Shutdown(-1);
                return;
            }

            base.OnStartup(e);
        }
    }
}
