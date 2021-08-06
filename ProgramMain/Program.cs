﻿using CommonLibrary;
using ControlzEx.Theming;
using GeneralTool.General.Interfaces;
using GeneralTool.General.Logs;
using GeneralTool.General.TaskLib;
using GeneralTool.General.WPFHelper.Extensions;
using MainWindowLib;
using MainWindowLib.ViewModels;
using System;
using System.Windows;
using System.Windows.Media;
using TaskLibarary.TestLib;

namespace ProgramMain
{
    class Program
    {
        // <summary>
        // 启动层级日志
        // </summary>
        public static ILog Log = new FileInfoLog("启动层级日志");

        /// <summary>
        /// 启动容器
        /// </summary>
        public static void Start()
        {
            InjectionService.Service.InjectionSingleInstance<MainWindow>();
            var contentLog = new FileInfoLog("内容日志");//5M大小每个
            InjectionService.Service.InjectionSingleInstance<ILog>(contentLog);
            NoticeGlobal.Log = contentLog;

            InjectionService.Service.InjectionSingleInstance<MiddleController>();
            InjectionService.Service.InjectionSingleInstance<TaskManager>();
            InjectionService.Service.InjectionSingleInstance<TestTask>();
            InjectionService.Service.InjectionSingleInstance<TestLib2>();

            InjectionService.Service.InjectionSingleInstance<MainViewModel>();
            InjectionService.Service.InjectionSingleInstance<ImageViewModel>();
            InjectionService.Service.InjectionSingleInstance<InterfacesViewModel>();
            InjectionService.Service.InjectionSingleInstance<SettingViewModel>();
            InjectionService.Service.InjectionSingleInstance<ThemeViewModel>();
            InjectionService.Service.InjectionSingleInstance<DeviceViewModel>();
            InjectionService.Service.InjectionSingleInstance<CustomViewModel>();

            InjectionService.Service.Start();
        }

        [STAThread]
        static void Main(string[] args)
        {
            //处理非UI线程异常  
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                var app = new App();
                app.InitializeComponent();
                InitTheme();
                using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, AppDomain.CurrentDomain.FriendlyName, out bool createNew))
                {
                    if (createNew)
                    {
                        var splash = new SplashScreenWindow();
                        splash.Show(Resource.code.ToBitmapImage());
                        Start();
                        app.MainWindow = InjectionService.Service.Resolve<MainWindow>();
                        app.MainWindow.Show();
                        splash.Close();
                        app.Run();
                    }
                    else
                    {
                        var message = "应用程序已经在运行中...";
                        UnhandleExcptionWindow.MessageBox(message);
                        Log.Error(message);
                        System.Threading.Thread.Sleep(1000);
                        System.Environment.Exit(1);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("程序运行错误:" + ex);
                UnhandleExcptionWindow.MessageBox(ex.ToString());
            }
        }


        /// <summary>
        /// 初始化主题颜色等
        /// </summary>
        private static void InitTheme()
        {
            // Application.Current.Dispatcher.Thread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            var themeColorName = IniSettings.ThemeNode.ThemeColorName.Value;
            var selectTheme = IniSettings.ThemeNode.ThemeName.Value;
            var color = (Color)ColorConverter.ConvertFromString(themeColorName);

            var newTheme = new Theme(name: "CustomTheme",
                                  displayName: "CustomTheme",
                                  baseColorScheme: selectTheme,
                                  colorScheme: "CustomAccent",
                                  primaryAccentColor: color,
                                  showcaseBrush: new SolidColorBrush(color),
                                  isRuntimeGenerated: true,
                                  isHighContrast: true);

            ThemeManager.Current.ChangeTheme(Application.Current, newTheme);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.ExceptionObject + "");
            UnhandleExcptionWindow.MessageBox(e.ExceptionObject + "");
        }
    }
}
