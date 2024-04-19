using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GeneralTool.CoreLibrary.WPFHelper;

namespace MainWindowLib.Models
{
    public class TestModels:BaseNotifyModel
    {
        public AsyncObservableCollection<TestModel> TestModelCollection { get; set; }

        public TestModels()
        {
            var keyVal = new Dictionary<string, object>();
            keyVal.Add("动作", "Add");
            keyVal.Add("开始时间", "Add");
            keyVal.Add("动作类型", "Add");
            keyVal.Add("耗时", "Add");
            keyVal.Add("进度", "Add");
            keyVal.Add("备注", "Add");
            //可选
            keyVal.Add("本地路径", "Add");
            keyVal.Add("远程路径", "Add");
            keyVal.Add("处理数量", "Add");
            keyVal.Add("删除数据", "Add");
            keyVal.Add("目录数量", "Add");
            keyVal.Add("保留空间", "Add");
        }
    }
}
