using GeneralTool.General.WPFHelper;
using MahApps.Metro.Controls.Dialogs;
using MainWindowLib.Models;
using MainWindowLib.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MainWindowLib.ViewModels
{
    public class CustomViewModel
    {
        public ICommand ShowMessageCommand
        {
            get => new SimpleCommand(async () =>
            {
                await ApplicationHelper.ApplicationWindow.ShowMessageAsync("标题", "消息");
            });
        }
        public ICommand ShowProgressCommand
        {
            get => new SimpleCommand(async () =>
            {
                var result = await ApplicationHelper.ShowProgressAsync("等待框", "请等待 3 秒");
                await Task.Run(() =>
                {
                    for (int i = 1; i < 4; i++)
                    {
                        result.SetProgress((i * 3.0) / 10);
                        result.SetMessage($"请等待 {(4 - i)} 秒");
                        System.Threading.Thread.Sleep(1000);
                    }
                });
                await result.CloseAsync();
            });
        }
        public ICommand ShowLoginCommand
        {
            get => new SimpleCommand<string>(async (showLogin) =>
            {
                LoginDialogData result = await ApplicationHelper.ApplicationWindow.ShowLoginAsync("title", "Message", new LoginDialogSettings { ColorScheme = MetroDialogColorScheme.Accented, NegativeButtonVisibility = System.Windows.Visibility.Visible, ShouldHideUsername = showLogin == "", });
                if (result == null)
                {
                    //User pressed cancel
                }
                else
                {
                    var message = "";
                    if (showLogin == "")
                        message = String.Format("Password: {0}", result.Password);
                    else
                        message = String.Format("UserName: {0},Password: {1}", result.Username, result.Password);
                    MessageDialogResult messageResult = await ApplicationHelper.ApplicationWindow.ShowMessageAsync("Authentication Information", message);
                }
            });
        }


        public ICommand ShowCustomCommand
        {
            get => new SimpleCommand<string>(async (str) =>
            {
                var settings = new MetroDialogSettings() { ColorScheme = str == "Theme" ? MetroDialogColorScheme.Theme : MetroDialogColorScheme.Accented };
                var viewModel = new DeviceUpdateDialogViewModel();

                var userControl = new CustomDialogs.DeviceUpdateView();
                var ctype = CustomDialogType.Dialog;
                if (str == "NotShowDialog")
                {
                    ctype = CustomDialogType.Show;
                }
                await ApplicationHelper.ShowCustomDialogAsync(userControl, viewModel, "自定义标题", str != "UseMaxHeight", settings, ctype);
                //获取数据
                // await ApplicationHelper.ApplicationWindow.ShowMessageAsync("自定义框传回的数据", viewModel.Result);
            });
        }

        public ThemeViewModel ThemeViewModel { get; set; }
        public ICommand ShowThemeCommand
        {
            get => new SimpleCommand(async () =>
              {
                  var control = new ThemeView();
                  await ApplicationHelper.ShowCustomDialogAsync(control, this.ThemeViewModel);
              });
        }
    }
}
