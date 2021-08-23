using GeneralTool.General;
using GeneralTool.General.Attributes;
using GeneralTool.General.NetHelper;
using GeneralTool.General.TaskLib;
using System;
using System.Linq;

namespace TaskLibarary
{
    [Route(nameof(Device) + "/", "设备"), InjectType()]
    public class Device : BaseTaskInvoke
    {
        [Route(nameof(DeviceState), "获取设备状态")]
        public dynamic GetDeviceState()
        {
            var code = new Random().Next(0, 3);
            var state = (DeviceState)code;
            return new { ret = state == DeviceState.offline, devicestatus = state, msg = "无异常" };
        }

        [Route(nameof(GetDeviceSN), "获取设备SN")]
        public object GetDeviceSN()
        {
            var code = Guid.NewGuid().ToString();
            return new { ret = true, sn = code };

        }

        [Route(nameof(SetList), "设置测试", HttpMethod.POST)]
        public object SetList([WaterMark("数量")] int count)
        {
            var list = Enumerable.Range(0, count).ToArray();


            return new { ret = true, list = list };
        }

        [Route(nameof(TestTuple), "设置测试", ReturnString = "(bool ret,string msg)")]
        public dynamic TestTuple()
        {
            var re = new { ret = false, msg = "Test" };

            var str = re.SerializeToJsonString();
            return re;
        }
    }

    public enum DeviceState
    {
        available,
        unavilable,
        offline
    }
}
