using CommonLibrary;
using CommonModels;
using GeneralTool.General.Interfaces;
using GeneralTool.General.Models;
using GeneralTool.General.WPFHelper;
using MahApps.Metro.Controls;
using MainWindowLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace MainWindowLib.ViewModels
{
    public class MainViewModel : BaseNotifyModel
    {
        #region 属性/字段

        private bool themeIsOpen = false;
        /// <summary>
        /// 主题是否展示
        /// </summary>
        public bool ThemeIsOpen { get => this.themeIsOpen; set => this.RegisterProperty(ref this.themeIsOpen, value); }

        private int mainSelectedIndex = 0;
        public int MainSelectedIndex { get => this.mainSelectedIndex; set => this.RegisterProperty(ref this.mainSelectedIndex, value); }
        /// <summary>
        /// Log组件
        /// </summary>
        public ILog Log { get; set; }

        private int logNums = 10;
        /// <summary>
        /// 日志数量
        /// </summary>
        public int LogNums
        {
            get
            {

                this.logNums = IniSettings.WindowNode.ShowLogCount.Value;
                return this.logNums;
            }
            set
            {
                if (value < 10)
                    value = 10;
                IniSettings.WindowNode.ShowLogCount.Value = value;
                this.RegisterProperty(ref this.logNums, value);
            }
        }

        private double logFontSize = 12;
        /// <summary>
        /// 日志字体尺寸
        /// </summary>
        public double LogFontSize
        {
            get
            {
                this.logFontSize = IniSettings.WindowNode.LogFontSize.Value;
                return this.logFontSize;
            }
            set
            {
                IniSettings.WindowNode.LogFontSize.Value = value;
                this.RegisterProperty(ref this.logFontSize, value);
            }
        }

        private int langSelectedIndex = 0;
        public int LangSelectedIndex
        {
            get
            {
                this.langSelectedIndex = IniSettings.WindowNode.LangSelectedIndex.Value;
                return this.langSelectedIndex;
            }
            set
            {
                IniSettings.WindowNode.LangSelectedIndex.Value = value;
                this.RegisterProperty(ref this.langSelectedIndex, value);
            }
        }

        private MiddleController Controller;

        /// <summary>
        /// 日志
        /// </summary>
        public ObservableCollection<LogMessageInfo> Lines { get; set; } = new ObservableCollection<LogMessageInfo>();
        public ObservableCollection<string> LangList { get; set; } = new ObservableCollection<string>();

        #endregion

        #region Command


        public ICommand ClearLogCommand
        {
            get
            {
                return new SimpleCommand<Paragraph>((p) =>
                 {
                     this.Log.Debug("清除日志");
                     p.Inlines.Clear();
                     NoticeGlobal.Clear();
                 });
            }
        }

        public ICommand HamburgerItemCommand
        {
            get => new SimpleCommand<HamburgerMenu>(h =>
              {
                  h.Content = h.SelectedItem;
              });
        }


        #endregion


        #region 事件

        private HamburgerMenu hamburger;

        /// <summary>
        /// 菜单切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HamburgerItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            if (sender is HamburgerMenu hamburger)
            {
                hamburger.Content = e.InvokedItem;
                this.hamburger = hamburger;
            }
        }

        /// <summary>
        /// 主题更换事件
        /// </summary>
        /// <param name="e"></param>
        public void ThemeMouseDown(MouseButtonEventArgs e)
        {
            this.ThemeIsOpen = !this.ThemeIsOpen;
            this.Log.Debug($"主题打开:{this.ThemeIsOpen}");
        }

        /// <summary>
        /// 日志缓存清除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ClearLogsMouseDown(object sender, MouseButtonEventArgs e)
        {
            var text = sender as UIElement;
            text.IsEnabled = false;
            var result = await ApplictionHelper.ShowProgressAsync("提示", "正在清除中,请稍候");
            await Task.Delay(1000);
            await Task.Run(() =>
            {
                //清除本地LOG缓存 ,不是当天的全给清了
                //找到本地日志文件夹
                var logDir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"));

                //循环其中的文件夹
                foreach (DirectoryInfo directory in logDir.EnumerateDirectories())
                {
                    var files = directory.GetFiles("*.log");
                    foreach (var item in files)
                    {
                        try
                        {
                            if ((DateTime.Now - item.CreationTime >= TimeSpan.FromHours(10)))
                                item.Delete();
                        }
                        catch
                        {

                        }
                    }
                }

            });

            await result.CloseAsync();
            text.IsEnabled = true;
            this.Log.Log("清理日志及缓存成功");
        }

        public void LangSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var key = e.AddedItems[0] + "";

            LangProvider.LangProviderInstance.ChangeLang(key);
            var CurrentUICultureStr = LangProvider.LangProviderInstance.GetLangValue("CurrentUICulture");

            var cultureInfo = new CultureInfo(CurrentUICultureStr);

            Application.Current.MainWindow.Dispatcher.Thread.CurrentUICulture = cultureInfo;
        }


        #endregion

        #region ViewModels

        /// <summary>
        /// 主题
        /// </summary>
        public ThemeViewModel ThemeViewModel { get; set; }

        /// <summary>
        /// 设置
        /// </summary>
        public SettingViewModel SettingViewModel { get; set; }

        /// <summary>
        /// 接口
        /// </summary>
        public InterfacesViewModel InterfacesViewModel { get; set; }

        /// <summary>
        /// 图像
        /// </summary>
        public ImageViewModel ImageViewModel { get; set; }

        public DeviceViewModel DeviceViewModel { get; set; }
        #endregion


        public MainViewModel(MiddleController controller)
        {
            this.Controller = controller;
            this.Controller.RaiseChangeMainTabEvent += this.Controller_RaiseChangeMainTabEvent;
            this.Controller.RaiseNoticMessageEvent += Controller_RaiseNoticMessageEvent;
            this.LangList.Add("中文");
            this.LangList.Add("English");

            //设置当前的语言包
            var dic = new Dictionary<string, ResourceDictionary>();
            dic.Add("English", new ResourceDictionary() { Source = new Uri($"pack://application:,,,/MainWindowLib;component/Asserts/English.xaml") });
            dic.Add("中文", new ResourceDictionary() { Source = new Uri($"pack://application:,,,/MainWindowLib;component/Asserts/Chinese.xaml") });
            LangProvider.LangProviderInstance.AddLangResources(dic);
           // LangProvider.LangProviderInstance.DefaultChineseLangKey = "中文";
        }

        private void Controller_RaiseNoticMessageEvent(string message, GrowType growType)
        {
            NoticeGlobal.GrowInfo(new NoticeModel(message, TimeSpan.FromSeconds(5), growType));
        }

        private void Log_LogEvent(object sender, LogMessageInfo e)
        {
            this.Lines.Add(e);
        }

        public void Init()
        {
            ApplictionHelper.SynchronizationContext = SynchronizationContext.Current;
            this.Log.LogEvent += this.Log_LogEvent;
        }

        private void Controller_RaiseChangeMainTabEvent(MainTab tab)
        {
            this.MainSelectedIndex = (int)tab;
            this.hamburger?.RaiseItemCommand();
        }
    }
}
