using GeneralTool.CoreLibrary.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MainWindowLib.DependecyHelpers
{
    /// <summary>
    /// 参数信息依赖属性
    /// </summary>
    public class DoTaskParameterInfoDependecy : DependencyObject
    {
        public static readonly DependencyProperty DoTaskParameterInfoProperty = DependencyProperty.RegisterAttached("DoTaskParameterInfo", typeof(bool), typeof(ItemsDoTaskDependecy), new PropertyMetadata(ItemsChanged));



        public static void SetDoTaskParameterInfo(Grid control, bool value) => control.SetValue(DoTaskParameterInfoProperty, value);

        public static bool GetDoTaskParameterInfo(Grid control) => (bool)control.GetValue(DoTaskParameterInfoProperty);


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
            var model = e.NewValue as ObservableCollection<ParameterItem>;

            int index = 0;
            foreach (var item in model)
            {
                //生成行
                grid.RowDefinitions.Add(new RowDefinition());
                //写入数据
                //数据类型
                var txtType = new TextBlock() { Text = item.ParameterType.Name };
                grid.Children.Add(txtType);
                Grid.SetRow(txtType, index);
                Grid.SetColumn(txtType, 0);

                //数据名称
                var txtName = new TextBox() { Text = item.ParameterName };
                grid.Children.Add(txtName);
                Grid.SetRow(txtName, index);
                Grid.SetColumn(txtName, 1);

                //数据注释
                var waterMark = new TextBox() { Text = item.WaterMark };
                grid.Children.Add(waterMark);
                Grid.SetRow(waterMark, index);
                Grid.SetColumn(waterMark, 2);
                index++;
            }
        }


    }
}
