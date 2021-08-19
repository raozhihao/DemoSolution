using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MainWindowLib.Windows
{
    public class SplashScreenWindow
    {
        public SplashScreenWindow()
        {

        }

        public Window Window { get; private set; }

        private TextBlock messageBox;
        public void Show(BitmapImage image, double width = 800, bool topMost = true)
        {
            var grid = new Grid() { Background = Brushes.Black, Width = width };
            this.messageBox = new TextBlock()
            {
                Text = "正在启动应用程序,请稍候...",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Background = Brushes.Black,
                Foreground = Brushes.White,
                Margin = new Thickness(20)
            };

            Panel.SetZIndex(this.messageBox, 999);

            grid.Children.Add(this.messageBox);

            var Img = new Image() { Source = image, Width = width, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };

            grid.Children.Add(Img);

            this.Window = new MetroWindow()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Topmost = topMost,
                UseNoneWindowStyle = true,
                Width = width,
                Content = grid,
                AllowsTransparency = true,
                BorderThickness = new Thickness(0),
                SizeToContent = SizeToContent.Height,
                Background = Brushes.Transparent,
                Opacity = 100,
            };
            this.Window.Show();
            System.Threading.Thread.Sleep(50);
        }

        public void SetMessage(string message) => this.messageBox.Text = message;

        public void Close() => this.Window?.Close();
    }
}
