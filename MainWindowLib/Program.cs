using CommonLibrary;
using ControlzEx.Theming;
using GeneralTool.CoreLibrary;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Ioc;
using GeneralTool.CoreLibrary.Logs;
using GeneralTool.CoreLibrary.TaskLib;
using GeneralTool.CoreLibrary.WPFHelper.Extensions;
using MainWindowLib.ViewModels;
using MainWindowLib.Windows;
using System;
using System.Windows;
using System.Windows.Media;
using TaskLibarary;
using TaskLibarary.TestLib;

namespace MainWindowLib
{
    class Program
    {
        /// <summary>
        /// 项目属性启动对象,请选择此Main
        /// </summary>
        [STAThread]
        static void Main()
        {
            OnStart(); 
        }


        // <summary>
        // 启动层级日志
        // </summary>
        public static ILog Log = new FileInfoLog("启动层级日志");

        /// <summary>
        /// 注册容器
        /// </summary>
        static void Inject()
        {
            var httpLog = new FileInfoLog("http");
            var httpServer = new HttpServerStation(httpLog);
            SimpleIocSerivce.SimpleIocSerivceInstance.Inject(new TaskManager(null, null, httpServer));

            //SimpleIocSerivce.SimpleIocSerivceInstance.Inject<TaskManager>();

            var contentLog = new FileInfoLog("内容日志");//3M大小每个
            SimpleIocSerivce.SimpleIocSerivceInstance.Inject<ILog>(contentLog);
            NoticeGlobal.Log = contentLog;

            //注册TaskLib程序集
            SimpleIocSerivce.SimpleIocSerivceInstance.Inject(typeof(TestTask).Assembly, true);

            //注册其它程序集
            SimpleIocSerivce.SimpleIocSerivceInstance.Inject(false,
                typeof(MiddleController),
                typeof(SettingViewModel),
                typeof(AutoCreateViewModel),
                typeof(Device),
                typeof(ImageViewModel),
                typeof(InterfacesViewModel),
                typeof(ThemeViewModel),
                typeof(DeviceViewModel),
                typeof(CustomViewModel),
                typeof(MainViewModel),
                typeof(MainWindow)
                );

            SimpleIocSerivce.SimpleIocSerivceInstance.Start();
        }


      
        static bool OnStart()
        {
            Log.Info("程序开始准备初始化");
            //处理非UI线程异常  
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            SplashScreenWindow splash = null;
            try
            {
                var app = new App();

                app.DispatcherUnhandledException += App_DispatcherUnhandledException;
                app.InitializeComponent();
                InitTheme();

                using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, AppDomain.CurrentDomain.FriendlyName, out bool createNew))
                {
                    if (createNew)
                    {
                        splash = new SplashScreenWindow();
                        splash.Show(Properties.Resources.code.ToBitmapImage());

                        splash.SetMessage("初始化容器中....");
                        Inject();
                        splash.SetMessage("容器初始化完成,启动窗体中....");

                        app.MainWindow = SimpleIocSerivce.SimpleIocSerivceInstance.Resolve<MainWindow>();

                        splash.SetMessage("窗体初始化完成,准备启动....");

                        app.MainWindow.Show();
                        splash.Close();
                        Log.Info("程序开始正常启动");

                        try
                        {
                            app.Run();
                        }
                        catch
                        {
                            splash?.Close();
                        }

                        return true;
                    }
                    else
                    {
                        var message = "应用程序已经在运行中...";
                        MessageBox.Show(message);
                        Log.Error(message);
                        System.Threading.Thread.Sleep(1000);
                        Environment.Exit(1);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                splash?.Close();
                Log.Error("程序运行错误:" + ex);
                UnhandleExceptionWindow.MessageBox(ex.ToString());
                return false;
            }
        }

        private static void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error("程序运行错误:" + e.Exception);
            var result = UnhandleExceptionWindow.MessageBox(e.Exception + "");
            if (result == UnhandleResult.Cancel)
            {
                //用户不想关闭程序,则继续
                e.Handled = true;
            }
        }


        /// <summary>
        /// 初始化主题颜色等
        /// </summary>
        static void InitTheme()
        {
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

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.ExceptionObject);
            UnhandleExceptionWindow.MessageBox(e.ExceptionObject + "");
        }

    }
}
