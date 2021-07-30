using GeneralTool.General.ExceptionHelper;
using GeneralTool.General.Interfaces;
using GeneralTool.General.Models;
using GeneralTool.General.TaskLib;
using GeneralTool.General.WPFHelper;
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
                      bool re = CheckParameter(taskinfo, out var parameterStr);
                      if (!re)
                          return;
                      Log.Debug($"执行参数信息:{parameterStr}");
                      var result = await Task.Run(() =>
                      {
                          return this.TaskManager.DoInterface(taskinfo.Url, taskinfo);
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
        private bool CheckParameter(DoTaskParameterItem taskinfo, out string parameterStr)
        {
            parameterStr = "";
            var list = new List<string>(taskinfo.Paramters.Count);
            foreach (var item in taskinfo.Paramters)
            {
                try
                {
                    list.Add($"{item.ParameterName}:{item.Value}");
                    //Convert.ChangeType(item.Value, item.ParameterType);
                }
                catch
                {
                    //this.ResultLog = $"参数:{item.ParameterName} 值填写不正确,应该为类型:{item.ParameterType}";
                    //this.Log.Error(ResultLog);
                    //return false;
                }
            }
            parameterStr = string.Join(" , ", list);
            return true;
        }

        private CommonLibrary.MiddleController controller;
        public InterfacesViewModel(CommonLibrary.MiddleController controller)
        {
            this.controller = controller;
        }
    }
}
