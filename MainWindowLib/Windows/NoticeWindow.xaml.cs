using CommonModels;
using MahApps.Metro.Controls;
using MainWindowLib.Models;
using MainWindowLib.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MainWindowLib.Windows
{
    /// <summary>
    /// GrowWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NoticeWindow : MetroWindow
    {
        public NoticeWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗口开启的最大值
        /// </summary>
        public int MaxCount { get; internal set; } = 5;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            Rect workArea = SystemParameters.WorkArea;
            base.Height = workArea.Height;
            base.Left = workArea.Right - base.Width;
            base.Top = 0.0;
            this.PanelContent.SizeChanged += PanelContent_SizeChanged;
            this.Owner = ApplicationHelper.ApplicationWindow;
        }

        private void PanelContent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Height == 0 && e.HeightChanged)
            {
                this.Close();
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="grow"></param>
        public void SetInfo(NoticeModel grow)
        {
            CloseElseWindow(MaxCount, this.PanelContent.Children.Count - 1);
            var view = new NoticeView();
            view.CloseEvent += View_CloseEvent;
            view.SetMessage(grow);

            this.PanelContent.Children.Insert(0, view);
            //this.PanelContent.Children.Add(view);
        }

        private void CloseElseWindow(int maxCount, int index)
        {
            try
            {
                var count = this.PanelContent.Children.Count;
                if (count >= maxCount && index < count)
                {
                    if (!(this.PanelContent.Children[index] is NoticeView info))
                        return;
                    if (info.CloseTimeSpan == TimeSpan.Zero)
                    {
                        CloseElseWindow(maxCount, index - 1);
                        return;
                    }
                    this.PanelContent.Children.RemoveAt(index);
                }
            }
            catch
            {

            }
        }

        private void View_CloseEvent(UserControl view)
        {
            view.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.PanelContent.Children.Remove(view);
            }));

        }
    }
}
