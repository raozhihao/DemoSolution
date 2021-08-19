using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MainWindowLib.DependecyHelpers
{
    public class MetroHamburgerMenuDependecys : DependencyObject
    {
        /// <summary>
        /// 为UserControl附加DataContext,但需要先指定 MainProperty 才可生效
        /// </summary>
        public static readonly DependencyProperty ViewDataContextProperty = DependencyProperty.RegisterAttached("ViewDataContext", typeof(Type), typeof(MetroHamburgerMenuDependecys), new PropertyMetadata(null, ViewDataContextChanged));

        static Dictionary<UserControl, Type> userModels = new Dictionary<UserControl, Type>();

        private static void ViewDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UserControl u)
            {
                var type = (Type)e.NewValue;
                userModels.Add(u, type);
            }
        }

        public static void SetViewDataContext(UserControl dependency, object value) => dependency.SetValue(ViewDataContextProperty, value);

        public static object GetViewDataContext(UserControl dependency) => dependency.GetValue(ViewDataContextProperty);


        /// <summary>
        /// 为UserControl的DataContext指定window宿主
        /// </summary>
        public static readonly DependencyProperty MainProperty = DependencyProperty.RegisterAttached("Main", typeof(Window), typeof(MetroHamburgerMenuDependecys), new PropertyMetadata(WindowChanged));
        private static void WindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window w)
            {
                w.Loaded += W_Loaded;
            }
        }

        private static void W_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in userModels)
            {
                //var obj = CommonLibrary.InjectionService.Service.Resolve(item.Value);
                var obj = GeneralTool.General.Ioc.SimpleIocSerivce.SimpleIocSerivceInstance.Resolve(item.Value);
                item.Key.DataContext = obj;
            }
        }

        public static void SetMain(DependencyObject dependecy, Window window) => dependecy.SetValue(MainProperty, window);

        public static Window GetMain(DependencyObject dependency) => (Window)dependency.GetValue(MainProperty);
    }
}
