using CommonLibrary;
using MahApps.Metro.Controls;
using MainWindowLib.ViewModels;
using Microsoft.Win32;
using System;
using System.Windows;

namespace MainWindowLib
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// ViewModel 必须要有,否则里面的那些Log不能自动初始化
        /// </summary>
        public MainViewModel MainViewModel { get; set; }

        public MiddleController Controller { get; set; }

        // public ILog Log { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = MainViewModel;
            this.MainViewModel.Init();
        }

    }
}
