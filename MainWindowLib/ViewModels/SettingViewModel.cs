﻿using CommonLibrary;
using CommonModels;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.TaskLib;
using GeneralTool.CoreLibrary.WPFHelper;
using MainWindowLib.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskLibarary;
using TaskLibarary.TestLib;

namespace MainWindowLib.ViewModels
{
    /// <summary>
    /// 设置
    /// </summary>
    public class SettingViewModel : BaseNotifyModel
    {
        /// <summary>
        /// 日志组件
        /// </summary>
        public ILog Log { get; set; }

        /// <summary>
        /// 任务组件
        /// </summary>
        public TaskManager TaskManager { get; set; }

        /// <summary>
        /// 任务对象1
        /// </summary>
        public TestTask TestTask { get; set; }

        /// <summary>
        /// 任务对象2
        /// </summary>
        public TestLib2 TestLib2 { get; set; }

        public Device Device { get; set; }

        /// <summary>
        /// 接口对象
        /// </summary>
        public InterfacesViewModel InterfacesView { get; set; }

        private bool controlVisible;
        /// <summary>
        /// 开始显示
        /// </summary>
        public bool ControlVisible { get => this.controlVisible; set => this.RegisterProperty(ref this.controlVisible, value); }

        private string serverIp = "127.0.0.1";
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServerIP
        {
            get => IniSettings.WindowNode.ServerIP.Value;
            set
            {
                this.serverIp = value;
                IniSettings.WindowNode.ServerIP.Value = this.serverIp;
                this.RegisterProperty(ref this.serverIp, value);
            }
        }

        private int serverPort = 8877;
        /// <summary>
        /// 服务器地址
        /// </summary>
        public int ServerPort
        {
            get => IniSettings.WindowNode.ServerPort.Value;
            set
            {
                this.serverPort = value;
                IniSettings.WindowNode.ServerPort.Value = this.serverPort;
                this.RegisterProperty(ref this.serverPort, value);
            }
        }

        private bool settingCanEnable = true;
        public bool SettingCanEnable { get => this.settingCanEnable; set => this.RegisterProperty(ref this.settingCanEnable, value); }

        /// <summary>
        /// 开启按钮事件
        /// </summary>
        public ICommand OpenCommand
        {
            get
            {
                return new SimpleCommand(() =>
                {
                    Init();
                });
            }
        }


        /// <summary>
        /// 刷新按钮事件
        /// </summary>
        public ICommand RefreshCommand
        {
            get
            {
                return new SimpleCommand(() =>
                 {
                     this.SettingCanEnable = true;
                     this.ControlVisible = false;
                 });
            }
        }

        public MiddleController Controller { get; set; }

        public async void Init()
        {
            this.Log.Debug("初始化");
            //等待对话框,但需要有await任务,否则不会出现
            var loadingStr = LangProvider.LangProviderInstance.GetLangValue("Logs.LoadingStartup");
            var tips = LangProvider.LangProviderInstance.GetLangValue("Logs.MessageTitle");
            var progressDialog = await ApplicationHelper.ShowProgressAsync(tips, loadingStr);
            try
            {
                await Task.Delay(100);

                var setLog = LangProvider.LangProviderInstance.GetLangValueFomart("Logs.SettingOpenLog", this.ServerIP, this.ServerPort);
                Log.Info(setLog);
                //var reBool = this.TaskManager.Open(this.ServerIP, this.ServerPort, this.TestTask, this.TestLib2, this.Device);
                var reBool = this.TaskManager.Open(this.ServerIP, this.ServerPort, this.TestTask);
                if (!reBool)
                {
                    Log.Error("开启接口失败:" + this.TaskManager.ErroMsg);
                    return;
                }

                //加载所有接口
                this.TaskManager.GetInterfaces();

                this.ControlVisible = true;
                this.SettingCanEnable = false;
                this.Log.Info("开启成功");
                Controller.ChangeMainTab(MainTab.Interface);
                //NoticeGlobal.Info("开启成功");//全局通知测试
            }
            catch (Exception ex)
            {
                Log.Error(ex + "");
                this.ControlVisible = false;
            }
            finally
            {
                await progressDialog.CloseAsync();
            }

        }
    }
}
