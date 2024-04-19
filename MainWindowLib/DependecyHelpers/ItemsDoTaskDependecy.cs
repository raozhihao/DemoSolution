using CommonLibrary;

using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.WPFHelper.Extensions;
using MahApps.Metro.Controls;
using MainWindowLib.Models;

using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MainWindowLib.DependecyHelpers
{
    /// <summary>
    /// 参数信息动态生成
    /// </summary>
    public class ItemsDoTaskDependecy : DependencyObject
    {
        public static readonly DependencyProperty ItemsAutoCreateUiProperty = DependencyProperty.RegisterAttached("ItemsAutoCreateUi", typeof(bool), typeof(ItemsDoTaskDependecy), new PropertyMetadata(ItemsChanged));


        public static void SetItemsAutoCreateUi(Grid control, bool value) => control.SetValue(ItemsAutoCreateUiProperty, value);

        public static bool GetItemsAutoCreateUi(Grid control) => (bool)control.GetValue(ItemsAutoCreateUiProperty);


        private static void ItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Grid;
            if (e.NewValue != null && (bool)e.NewValue)
                control.DataContextChanged += Control_DataContextChanged;
            else
                control.DataContextChanged -= Control_DataContextChanged;
        }

        private static void Control_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //自动生成控件
            var grid = sender as Grid;
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            var model = e.NewValue as DoTaskParameterItem;
            var parameters = model.Method.GetParameters();
            int index = 0;
            for (int i = 0; i < parameters.Length; i++)
            {
                var item = model.Paramters[i];
                //生成行
                grid.RowDefinitions.Add(new RowDefinition());
                //写入数据
                var txtName = new TextBlock() { Text = item.ParameterName };
                grid.Children.Add(txtName);
                Grid.SetRow(txtName, index);
                Grid.SetColumn(txtName, 0);

                var sourceParameter = parameters[i];
                var uiAttr = sourceParameter.GetCustomAttribute<WaterUIEditorAttribute>();
                if (uiAttr != null)
                {
                    var control = uiAttr.UIControl;
                    var baseControl = Activator.CreateInstance(control) as BaseUIControl;
                    var property = baseControl.DependencyProperty;

                    item.Value = baseControl.ConvertByString(item);
                    SetControlBinding(grid, item, index, baseControl.Control, property);
                    baseControl.SetProperty(baseControl.Control, uiAttr.Settings);
                }
                else
                {
                    //只支持 string 类型,数值类型,枚举类型,boolean
                    //枚举
                    if (item.ParameterType.IsEnum)
                    {
                        SetEnumUI(grid, item, index);
                    }
                    else if (item.ParameterType.IsValueType)
                    {
                        //值类型
                        if (item.ParameterType == typeof(Boolean))
                        {
                            SetBooleanUI(grid, item, index);
                        }
                        else
                            SetValueType(grid, item, index);
                    }
                    else
                    {
                        //其它全是string类型
                        SetStringUI(grid, item, index);
                    }

                }

                index++;
            }
        }


        private static void SetControlBinding(Grid grid, ParameterItem item, int index, Control valueControl, DependencyProperty property)
        {
            grid.Children.Add(valueControl);
            Grid.SetRow(valueControl, index);
            Grid.SetColumn(valueControl, 1);
            //binding
            valueControl.DataContext = item;

            SetBinding(valueControl, item, property);
            //double.TryParse(item.Value + "", out var result);
            //valueControl.Value = result;
        }


        private static void SetValueType(Grid grid, ParameterItem item, int index)
        {
            var valueControl = new NumericUpDown();
            grid.Children.Add(valueControl);
            Grid.SetRow(valueControl, index);
            Grid.SetColumn(valueControl, 1);
            //binding
            valueControl.DataContext = item;
            SetBinding(valueControl, item, NumericUpDown.ValueProperty);
            double.TryParse(item.Value + "", out var result);
            valueControl.Value = result;


            if (item.ParameterType == typeof(int))
            {
                valueControl.NumericInputMode = NumericInput.Numbers;
            }
            else if (item.ParameterType == typeof(byte))
            {
                valueControl.Maximum = byte.MaxValue;
                valueControl.Minimum = byte.MinValue;
                valueControl.NumericInputMode = NumericInput.Numbers;
            }
        }

        private static void SetBooleanUI(Grid grid, ParameterItem item, int index)
        {
            var valueControl = new ComboBox();
            valueControl.SelectedItem = item.Value;
            grid.Children.Add(valueControl);
            Grid.SetRow(valueControl, index);
            Grid.SetColumn(valueControl, 1);
            //binding
            valueControl.DataContext = item;
            var arr = new bool[] { true, false };
            valueControl.ItemsSource = arr;

            SetBinding(valueControl, item, ComboBox.SelectedItemProperty);

            if (!string.IsNullOrWhiteSpace(item.Value + ""))
            {
                valueControl.SelectedIndex = Array.IndexOf(arr, Convert.ToBoolean(item.Value));
            }
            else
                valueControl.SelectedIndex = -1;
        }

        private static void SetEnumUI(Grid grid, ParameterItem item, int index)
        {
            var valueControl = new ComboBox();
            valueControl.SelectedItem = item.Value;
            grid.Children.Add(valueControl);
            Grid.SetRow(valueControl, index);
            Grid.SetColumn(valueControl, 1);
            //binding
            valueControl.DataContext = item;
            var arr = Enum.GetValues(item.ParameterType);
            valueControl.ItemsSource = arr;

            SetBinding(valueControl, item, ComboBox.SelectedItemProperty);

            if (!string.IsNullOrWhiteSpace(item.Value + ""))
            {
                valueControl.SelectedIndex = Array.IndexOf(arr, Enum.Parse(item.ParameterType, item.Value + ""));
            }
            else
                valueControl.SelectedIndex = -1;

        }


        private static void SetStringUI(Grid grid, ParameterItem item, int index)
        {
            var valueControl = new TextBox() { Text = item.Value + "" };
            grid.Children.Add(valueControl);
            Grid.SetRow(valueControl, index);
            Grid.SetColumn(valueControl, 1);
            //binding
            valueControl.DataContext = item;

            SetBinding(valueControl, item, TextBox.TextProperty);
        }

        private static void SetBinding(FrameworkElement valueControl, ParameterItem item, DependencyProperty Property)
        {
            var binding = new Binding();
            binding.Source = valueControl.DataContext;
            binding.Path = new PropertyPath(nameof(item.Value));
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            var converter = ValueConverter.CreateObjectConverter(item.ParameterType, typeof(string), ConvertFunc, BackFunc);
            binding.Converter = converter;
            valueControl.SetBinding(Property, binding);
            valueControl.ToolTip = item.WaterMark;
        }

        private static object ConvertFunc(ValueConverterObjectArgs arg)
        {
            if (string.IsNullOrWhiteSpace(arg.Value + "") && arg.InputType.IsValueType && arg.InputType != typeof(string))
            {
                return Activator.CreateInstance(arg.InputType);
            }
            return arg.Value + "";
        }

        private static object BackFunc(ValueConverterObjectArgs arg)
        {
            if (arg.Value == null)
            {
                return arg.Value + "";
            }
            if (arg.Value.GetType() == arg.InputType)
            {
                return arg.Value;
            }
            else if (arg.InputType == typeof(string))
            {
                return arg.Value;
            }
            else if (arg.InputType.IsEnum)
            {
                if (string.IsNullOrWhiteSpace(arg.Value + ""))
                    return Activator.CreateInstance(arg.InputType);

                return Enum.Parse(arg.InputType, arg.Value + "");
            }
            else if (arg.InputType.IsValueType)
            {
                return arg.Value;
            }
            return Activator.CreateInstance(arg.InputType);
        }
    }
}
