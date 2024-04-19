using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.TaskLib;
using GeneralTool.CoreLibrary.WPFHelper;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MainWindowLib.ViewModels
{
    public class InterfacesViewModel : BaseNotifyModel
    {
        /// <summary>
        /// 接口列表信息
        /// </summary>
        public ObservableCollection<TaskModel> TaskModels
        {
            get
            {
                return this.TaskManager?.TaskModels;
            }
        }
        private string resultLog;
        /// <summary>
        /// 调用后返回值
        /// </summary>
        public string ResultLog { get => this.resultLog; set => this.RegisterProperty(ref this.resultLog, value); }

        /// <summary>
        /// 日志组件
        /// </summary>
        public ILog Log { get; set; }

        /// <summary>
        /// 任务组件
        /// </summary>
        public TaskManager TaskManager { get; set; }

        /// <summary>
        /// 执行接口
        /// </summary>
        public ICommand DoTaskCommand
        {
            get
            {
                return new SimpleCommand<DoTaskParameterItem>(async (taskinfo) =>
              {

                  if (taskinfo == null)
                  {
                      this.Log.Info("没有选择一个接口");
                      this.ResultLog = "执行出错:没有选择一个接口";
                      return;
                  }

                  try
                  {
                      Log.Debug($"开始执行接口:{taskinfo.Url}");
                      var parameterStr = GetParameter(taskinfo);
                     
                      Log.Debug($"执行参数信息:{parameterStr}");
                      var result = await Task.Run(() =>
                      {
                          var r = this.TaskManager.DoInterface( taskinfo);
                          return r;
                      });

                      this.ResultLog = $"执行接口: [{taskinfo.Method.Name}] {Environment.NewLine}执行结果: [{this.TaskManager.JsonCovert.SerializeObject(result)}]";
                      this.Log.Info(this.resultLog);
                  }
                  catch (Exception ex)
                  {
                      this.ResultLog = $"执行接口: [{taskinfo.Method.Name}] 出错,错误信息:{ex.GetInnerExceptionMessage()} ";
                      this.Log.Error(this.ResultLog);
                  }

              });
            }
        }

        /// <summary>
        /// 基础的检查参数值是否与参数类型相匹配
        /// </summary>
        /// <param name="taskinfo"></param>
        /// <returns></returns>
        private string GetParameter(DoTaskParameterItem taskinfo)
        {
            var list = new List<string>(taskinfo.Paramters.Count);
            foreach (var item in taskinfo.Paramters)
            {
                list.Add($"{item.ParameterName}:{item.Value}");
            }
          return string.Join(" , ", list);
        }

        private CommonLibrary.MiddleController controller;
        public InterfacesViewModel(CommonLibrary.MiddleController controller)
        {
            this.controller = controller;
        }
    }
}
