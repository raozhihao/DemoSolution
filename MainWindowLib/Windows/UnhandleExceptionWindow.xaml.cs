using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainWindowLib.Windows
{
    /// <summary>
    /// UnhandleExceptionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UnhandleExceptionWindow : Window
    {
        public UnhandleResult ResultType { get; set; } = UnhandleResult.Close;
        public UnhandleExceptionWindow()
        {
            InitializeComponent();
          
            this.ButtonClose.Click += ButtonClose_Click;
            this.ButtonCancel.Click += ButtonCancel_Click;
        }


        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.ResultType = UnhandleResult.Cancel;
            this.Close();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.ResultType = UnhandleResult.Close;
            this.Close();
        }

        public void SetMessage(string message)
        {
            this.ExcptionMessage.Text = message;
        }


        public static UnhandleResult MessageBox(string message)
        {
            var window = new UnhandleExceptionWindow();
            var appWindow = Application.Current.MainWindow;
            if (appWindow != window && appWindow.IsLoaded)
                window.Owner = appWindow;
            else
                window.ButtonCancel.Visibility = Visibility.Collapsed;

            window.SetMessage(message);
            window.ShowDialog();
            return window.ResultType;
        }
    }
}
