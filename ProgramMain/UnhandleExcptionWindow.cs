using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProgramMain
{
    class UnhandleExcptionWindow
    {
        private static MetroWindow window;
        public static void MessageBox(string message)
        {
            var text = GetText("程序出现异常");

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });

            Grid.SetRow(text, 0);
            grid.Children.Add(text);

            var box = GetTextBox(message);
            Grid.SetRow(box, 1);
            grid.Children.Add(box);

            var button = GetButton("确定");
            button.Click += Button_Click;
            Grid.SetRow(button, 2);
            grid.Children.Add(button);

            window = new MetroWindow()
            {
                Width = 800,
                Topmost = true,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.Content = grid;
            window.ShowDialog();
        }

        private static void Button_Click(object sender, RoutedEventArgs e)
        {
            window?.Close();
        }

        private static Button GetButton(string content)
        {
            return new Button()
            {
                Content = content,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5),
                Padding = new Thickness(15, 5, 15, 5),
                FontSize = 20,
                Cursor = Cursors.Hand,
            };
        }

        private static TextBox GetTextBox(string message)
        {
            return new TextBox()
            {
                Text = message,
                Margin = new Thickness(5),
                TextWrapping = TextWrapping.Wrap,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                IsReadOnly = true,
            };
        }

        private static TextBlock GetText(string message)
        {
            return new TextBlock()
            {
                Text = message,
                Margin = new Thickness(5),
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center,
            };
        }
    }
}
