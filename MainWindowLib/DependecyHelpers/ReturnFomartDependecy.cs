using System.Windows;

namespace MainWindowLib.DependecyHelpers
{
    public class ReturnFomartDependecy : DependencyObject
    {
        public static readonly DependencyProperty ReturnFomartProperty = DependencyProperty.RegisterAttached("ReturnFomart", typeof(object), typeof(ReturnFomartDependecy), new PropertyMetadata(ReturnFomartChanged));
        private static void ReturnFomartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {

            }
        }

        public static void SetReturnFomart(DependencyObject d, object value) => d.SetValue(ReturnFomartProperty, value);
        public static object GetReturnFomart(DependencyObject d) => d.GetValue(ReturnFomartProperty);
    }
}
