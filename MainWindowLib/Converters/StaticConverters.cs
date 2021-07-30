using GeneralTool.General.WPFHelper.Extensions;
using System.Windows.Data;

namespace MainWindowLib.Converters
{
    public static class StaticConverters
    {
        public static IValueConverter ObjectToStringTypeConverter => new CoverterEx().ObjectToTypeStringConverter;

        public static IValueConverter ParameterConverter
        {
            get
            {
                return ValueConverter.Create<object, object>(ConvertFunc, ConvertBackFunc);
            }
        }

        private static object ConvertFunc(ValueConverterArgs<object> arg)
        {
            return arg.Value;
        }

        private static object ConvertBackFunc(ValueConverterArgs<object> arg)
        {
            return arg.Value;
        }
    }
}
