using GeneralTool.General.IniHelpers;

namespace CommonLibrary
{
    ///TODO:所有需要保存到本地或实时读取的配置项,均写在此处

    /// <summary>
    /// Ini 设置
    /// </summary>
    public static class IniSettings
    {
        public static ThemeNode ThemeNode { get; private set; }
        public static WindowNode WindowNode { get; set; }

        static IniSettings()
        {
            ThemeNode = new ThemeNode(nameof(ThemeNode));
            WindowNode = new WindowNode(nameof(WindowNode));
        }
    }

    /// <summary>
    /// 主题节点
    /// </summary>
    public class ThemeNode : Category
    {
        public ThemeNode(string sectionName) : base(sectionName)
        {
            this.ThemeName = new Node<string>(sectionName, nameof(ThemeName), "Light");
            this.ThemeColorName = new Node<string>(sectionName, nameof(ThemeColorName), "#FF7E7EBF");
        }

        /// <summary>
        /// 主题名称
        /// </summary>
        public Node<string> ThemeName { get; set; }
        /// <summary>
        /// 主题颜色
        /// </summary>
        public Node<string> ThemeColorName { get; set; }
    }

    /// <summary>
    /// 窗体全局节点
    /// </summary>
    public class WindowNode : Category
    {
        public WindowNode(string sectionName) : base(sectionName)
        {
            this.ShowLogCount = new Node<int>(sectionName, nameof(this.ShowLogCount), 300);
            this.LogFontSize = new Node<double>(sectionName, nameof(this.LogFontSize), 12);
            this.ServerIP = new Node<string>(sectionName, nameof(this.ServerIP), "127.0.0.1");
            this.ServerPort = new Node<int>(sectionName, nameof(this.ServerPort), 8877);
            this.LangSelectedIndex = new Node<int>(sectionName, nameof(this.LangSelectedIndex), 0);
        }

        /// <summary>
        /// 日志显示数量
        /// </summary>
        public Node<int> ShowLogCount { get; set; }

        /// <summary>
        /// 日志字体大小
        /// </summary>
        public Node<double> LogFontSize { get; set; }

        /// <summary>
        /// 服务器IP
        /// </summary>
        public Node<string> ServerIP { get; set; }

        /// <summary>
        /// 服务器端口
        /// </summary>
        public Node<int> ServerPort { get; set; }

        /// <summary>
        /// 语言选择下标
        /// </summary>
        public Node<int> LangSelectedIndex { get; set; }
    }
}
