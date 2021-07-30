using CommonModels;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MainWindowLib.Views
{
    /// <summary>
    /// GrowInfoView.xaml 的交互逻辑
    /// </summary>
    public partial class NoticeView : UserControl
    {
        public NoticeView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 信息模块将要关闭事件
        /// </summary>
        public event Action<UserControl> CloseEvent;
        private DateTime enterTime;

        /// <summary>
        /// 关闭时间,如果为Zero,则表示永不关闭
        /// </summary>
        public TimeSpan CloseTimeSpan { get; private set; }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="grow"></param>
        internal void SetMessage(NoticeModel grow)
        {
            if (grow.ShowCurrentTime)
                grow.Message = DateTime.Now + Environment.NewLine + grow.Message;

            this.MessageInfo.Text = grow.Message;
            enterTime = DateTime.Now;
            this.CloseTimeSpan = grow.TimeOutClosed;
            switch (grow.GrowType)
            {
                case GrowType.Waring:
                    this.HeadText.Foreground = Brushes.Yellow;
                    this.HeadText.Text = "\ue633";
                    break;
                case GrowType.Erro:
                    this.HeadText.Foreground = Brushes.Red;
                    this.HeadText.Text = "\ue619";
                    break;
            }



            this.CloseButton.Visibility = grow.ShowClosed ? Visibility.Visible : Visibility.Collapsed;
            if (grow.TimeOutClosed == TimeSpan.Zero)
                return;
            var timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.IsMouseOver)
            {
                return;
            }
            if (DateTime.Now - enterTime > this.CloseTimeSpan)
                this.CloseEvent?.Invoke(this);
        }

        private void CloseButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.CloseEvent?.Invoke(this);
        }
    }
}
