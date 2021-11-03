using CommonLibrary;
using CommonModels;
using GeneralTool.General.Attributes;
using GeneralTool.General.TaskLib;
using System;
using System.Drawing;
using System.IO;

namespace TaskLibarary.TestLib
{
    [Route(nameof(TestTask) + "/", "测试Test",LangKey = "TestTaskLabel"), InjectType()]
    public class TestTask : BaseTaskInvoke
    {
        public MiddleController Controller { get; set; }


        [Route(nameof(TestHello), "测试SayHello",LangKey ="SayHelloLable")]
        public string TestHello([WaterMark("名字")] string name, [WaterMark("年龄")] double age = 18)
        {
            return $"Hello {name} , your age is {age}";
        }

        [Route(nameof(ErrorLog), "错误日志测试")]
        public void ErrorLog()
        {
            try
            {
                this.Log.Error("Test Error");
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                //异常信息交给 this.erroMsg 后外部通过socket调用能够获取此信息
                this.erroMsg = ex.Message;
                this.Log.Error(this.erroMsg);
                throw;
            }
        }

        [Route(nameof(WaringLog), "警告日志测试")]
        public void WaringLog()
        {
            this.Log.Waring("警告测试");
        }

        [Route(nameof(GlobalNotice), "全局通知测试")]
        public void GlobalNotice([WaterMark("消息")] string message, [WaterMark("消息")] GrowType growType)
        {
            this.Controller.GlobalNotice(message, growType);
        }

        [Route(nameof(GetdImage), "图片接收测试")]
        public bool GetdImage([WaterMark("图片base64字符串")] string base64Str)
        {
            try
            {
                var img = ConvertBase64ToImage(base64Str);
                img.Save("1.png");
                img.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                this.erroMsg = "图片错误,无法转换:" + ex.Message;
                this.Log.Error(this.erroMsg);
            }
            return false;
        }

        [Route(nameof(TypeTest), "图片接收测试")]
        public bool TypeTest([WaterMark("Int 类型参数")] int int1 = 12, [WaterMark("double 类型参数")] double double1 = 12.2, [WaterMark("Enum 类型参数")] GrowType enumType = GrowType.Waring, [WaterMark("String 类型参数")] string str = "abcd", [WaterMark("bool 类型参数")] bool boo = false)
        {
            return false;
        }

        public Image ConvertBase64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                return Image.FromStream(ms, true);
            }
        }

        [Route(nameof(TestLog), "日志测试")]
        public void TestLog([WaterMark("打印次数")] int count = 1000)
        {
            for (int i = 0; i < count; i++)
            {
                new Action<int>(index =>
                {
                    this.Log.Info("打印日志:" + index);
                    System.Threading.Thread.Sleep(350);

                }).BeginInvoke(i, null, null);
            }
        }

    }
}
