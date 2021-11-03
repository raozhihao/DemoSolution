using MainWindowLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralTool.General.WPFHelper;
using System.Windows.Input;

namespace MainWindowLib.ViewModels
{
    public class AutoCreateViewModel : BaseNotifyModel
    {
        private List<DicModel<string, object>> keyValues;
        public List<DicModel<string, object>> KeyValuePairs
        {
            get => this.keyValues;
            set => this.RegisterProperty(ref this.keyValues, value);
        }

        private string text;
        public string Text
        {
            get => this.text;
            set => this.RegisterProperty(ref this.text, value);
        }
        public AutoCreateViewModel()
        {
            var keyVal = new DicModel<string, object>();
            keyVal.Add("动作", "Add","BLOCKTYPE");
            keyVal.Add("开始时间", DateTime.Now);
            keyVal.Add("动作类型", "Files");
            keyVal.Add("耗时", "00:34:12");
            keyVal.Add("进度", "0.9");
            keyVal.Add("备注", "Error");
            //可选
            keyVal.Add("本地路径", @"C:\Code\GitCode\GeneralTools\GeneralTool.General\bin\Debug");
            keyVal.Add("远程路径", "/sdcard/testfile24");
            keyVal.Add("处理数量", "4566");
            keyVal.Add("目录数量", "50");


            this.KeyValuePairs = new List<DicModel<string, object>>();
            this.keyValues.Add(keyVal);


            keyVal = new DicModel<string, object>();
            keyVal.Add("动作", "Add");
            keyVal.Add("开始时间", "Add");
            keyVal.Add("动作类型", "Add");
            keyVal.Add("耗时", "Add");
            keyVal.Add("进度", "Add");
            keyVal.Add("备注", "Add");
            //可选
            keyVal.Add("本地路径", "Add");
            keyVal.Add("远程路径", "Add");
            keyVal.Add("目录数量", "Add");
            keyVal.Add("保留空间", "Add");
            this.keyValues.Add(keyVal);
            this.Text = "TExt";

            foreach (var item in this.KeyValuePairs)
            {
                
            }
            
        }

        public ICommand ChangeCommand
        {
            get => new SimpleCommand(Changed);
        }

        private void Changed()
        {
            this.keyValues[0]["进度"] = new Random().Next(1,100);
        }
    }

    public class DicModel<T,K> : BaseNotifyModel
    {
        public ObservableCollection<KeyValue<T,K>> KeyValues { get; set; } = new ObservableCollection<KeyValue<T, K>>();
      
        public void Add(T key, K val,string colName="")
        {
            this.KeyValues.Add(new KeyValue<T, K>() { Key= key, Value = val,ColumnName= string.IsNullOrWhiteSpace(colName)?key+"":colName });
        }

        public K this[T key]
        {
            get
            {
                return this.KeyValues.First(k => k.Key.Equals(key)).Value;
            }
            set
            {
                this.KeyValues.First(k => k.Key.Equals(key)).Value = value;
            }
        }
    }

    public class KeyValue<T,K> : BaseNotifyModel
    {
        private T key;
        public T Key
        {
            get => this.key;
            set => this.RegisterProperty(ref this.key, value);
        }

        private K value;
        public K Value
        {
            get => this.value;
            set => this.RegisterProperty(ref this.value, value);
        }

        private string columnName;
        public string ColumnName
        {
            get => this.columnName;
            set => this.RegisterProperty(ref this.columnName, value);
        }

    }
}
