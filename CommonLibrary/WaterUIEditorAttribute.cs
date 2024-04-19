using System;
using System.Windows;
using System.Windows.Controls;

using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Models;

using MahApps.Metro.Controls;

namespace CommonLibrary
{
    public class WaterUIEditorAttribute : Attribute
    {
        public WaterUIEditorAttribute(Type uiControl, params object[] pars)
        {
            this.UIControl = uiControl;
            this.Settings = pars;
        }

        public Type UIControl { get; }
        public object[] Settings { get; }
    }


    public abstract class BaseUIControl
    {
        public abstract DependencyProperty DependencyProperty { get; }
        public abstract Control Control { get; }

        public abstract object ConvertByString(ParameterItem item);
        public abstract void SetProperty(Control control, params object[] objects);
    }



    public class StringControl : BaseUIControl
    {
        public override DependencyProperty DependencyProperty { get; } = TextBox.TextProperty;
        public override Control Control { get; } = new TextBox();

        public override object ConvertByString(ParameterItem item)
        {
            return item.Value + "";
        }

        public override void SetProperty(Control control, params object[] objects)
        {
            if (objects == null || objects.Length == 0) return;
            if (control is TextBox b)
            {
                b.MaxLength = (int)(objects[0]);
            }
        }
    }

    public class NumberControl : BaseUIControl
    {
        public override DependencyProperty DependencyProperty { get; } = NumericUpDown.ValueProperty;
        public override Control Control { get; } = new NumericUpDown();

        public override object ConvertByString(ParameterItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Value + ""))
            {
                return 0;
            }
            return Convert.ToDouble(item.Value);
        }

        public override void SetProperty(Control control, params object[] objects)
        {
            if (control is NumericUpDown n)
            {
                n.Minimum = Convert.ToInt32(objects[0]);
                n.Maximum = Convert.ToInt32(objects[1]);
                n.NumericInputMode = (NumericInput)(objects[2]);
            }

        }
    }

    public class EnumControl : BaseUIControl
    {
        public override DependencyProperty DependencyProperty { get; } = ComboBox.SelectedItemProperty;
        public override Control Control { get; } = new ComboBox();

        public override object ConvertByString(ParameterItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Value + ""))
            {
                return Enum.ToObject(item.ParameterType, 0);
            }
            return Enum.Parse(item.ParameterType, item.Value + "");
        }

        public override void SetProperty(Control control, params object[] objects)
        {
            if (this.Control is ComboBox box && box.DataContext is ParameterItem e)
            {
                var arr = Enum.GetValues(e.ParameterType);
                box.ItemsSource = arr;
                box.SelectedIndex = Array.IndexOf(arr, e.Value);
            }

        }
    }

    public class BoolControl : BaseUIControl
    {
        public override DependencyProperty DependencyProperty { get; } = CheckBox.IsCheckedProperty;
        public override Control Control { get; } = new CheckBox();

        public override object ConvertByString(ParameterItem item)
        {
            var value = item.Value + "";
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            return Convert.ToBoolean(value);
        }

        public override void SetProperty(Control control, params object[] objects)
        {
            this.Control.Margin = new Thickness(5);

            if (this.Control is CheckBox box && box.DataContext is ParameterItem e)
            {
                box.VerticalAlignment = VerticalAlignment.Center;
                box.VerticalContentAlignment = VerticalAlignment.Center;
            }
        }
    }
}
