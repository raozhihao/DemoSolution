using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainWindowLib.DependecyHelpers
{
    public class SplitButtonDependecy:DependencyObject
    {
        public readonly static DependencyProperty ClickOpenProperty = DependencyProperty.RegisterAttached("ClickOpen", typeof(bool), typeof(SplitButtonDependecy), new PropertyMetadata(ClickOpenChanged));

        private static void ClickOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                var button = d as SplitButton;
                button.AddHandler(UIElement.MouseDownEvent,new RoutedEventHandler(Button_MouseDown),true);
            }
        }

        private static void Button_MouseDown(object sender, EventArgs e)
        {
            var button = sender as SplitButton;
            button.IsDropDownOpen = true;
            button.SetValue(SplitButton.IsDropDownOpenProperty, true);
        }

        public static void SetClickOpen(SplitButton splitButton, bool value) => splitButton.SetValue(ClickOpenProperty, value);
        public static bool GetClickOpen(SplitButton splitButton) => (bool)splitButton.GetValue(ClickOpenProperty);
    }
}
