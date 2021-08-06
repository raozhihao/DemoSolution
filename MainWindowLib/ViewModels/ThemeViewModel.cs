using CommonLibrary;
using ControlzEx.Theming;
using GeneralTool.General.Interfaces;
using GeneralTool.General.WPFHelper;
using MainWindowLib.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MainWindowLib.ViewModels
{
    public class ThemeViewModel : BaseCustomDialogViewModel
    {
        private string selectTheme = "Light";
        public string SelectTheme
        {
            get => this.selectTheme;
            set => this.RegisterProperty(ref this.selectTheme, value);
        }

        private string currentTheme;
        public string CurrentTheme { get => this.currentTheme; set => this.RegisterProperty(ref this.currentTheme, value); }


        private Color selectedColor;
        public Color SelectedColor { get => this.selectedColor; set => this.RegisterProperty(ref this.selectedColor, value); }

        public ObservableCollection<string> BaseThemes { get; set; } = new ObservableCollection<string>();

        public ILog Log { get; set; }


        public ICommand ChangeThemeCommand
        {
            get
            {
                return new SimpleCommand(async () =>
                {
                    var newTheme = new Theme(name: "CustomTheme",
                                   displayName: "CustomTheme",
                                   baseColorScheme: this.SelectTheme,
                                   colorScheme: "CustomAccent",
                                   primaryAccentColor: this.SelectedColor,
                                   showcaseBrush: new SolidColorBrush(this.SelectedColor),
                                   isRuntimeGenerated: true,
                                   isHighContrast: true);

                    ThemeManager.Current.ChangeTheme(Application.Current, newTheme);
                    IniSettings.ThemeNode.ThemeColorName.Value = this.SelectedColor.ToString();
                    IniSettings.ThemeNode.ThemeName.Value = this.SelectTheme;
                    this.CurrentTheme = $"{this.SelectTheme}.{this.SelectedColor}";
                    this.Log.Info($"主题已更改为:{ this.CurrentTheme}");
                    await this.CloseAsync();
                });
            }
        }

        public ThemeViewModel()
        {
            var colors = ThemeManager.Current.BaseColors;
            foreach (var item in colors)
            {
                this.BaseThemes.Add(item);
            }
            var themeColorName = IniSettings.ThemeNode.ThemeColorName.Value;
            this.SelectTheme = IniSettings.ThemeNode.ThemeName.Value;
            var theme = $"{this.SelectTheme}.{themeColorName}";
            this.CurrentTheme = theme;
            this.SelectedColor = (Color)ColorConverter.ConvertFromString(themeColorName);

        }

    }
}
