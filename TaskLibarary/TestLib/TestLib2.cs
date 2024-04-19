using GeneralTool.CoreLibrary.Attributes;
using GeneralTool.CoreLibrary.TaskLib;
using System;

namespace TaskLibarary.TestLib
{
    [Route(nameof(TestLib2) + "/", "测试2"), InjectType()]
    public class TestLib2 : BaseTaskInvoke
    {
        [Route(nameof(TestHello), "测试SayHello")]
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
    }
}
