using CommonLibrary;
using CommonModels;
using System;

namespace CommonLibrary
{
    /// <summary>
    /// 中间件,用于各类之间的联合调用  TODO:请使用此类来进行不相关两类的相互调用
    /// </summary>
    public class MiddleController
    {
        /// <summary>
        /// 通知更改主页显示事件
        /// </summary>
        public event Action<MainTab> RaiseChangeMainTabEvent;

        /// <summary>
        /// 更改当前显示主页
        /// </summary>
        /// <param name="tab">要更改的主页</param>
        public void ChangeMainTab(MainTab tab)
        {
            this.RaiseChangeMainTabEvent?.Invoke(tab);
        }

        /// <summary>
        /// 通知需要启用全局通知事件
        /// </summary>
        public event Action<string, GrowType> RaiseNoticMessageEvent;

        /// <summary>
        /// 全局通知
        /// </summary>
        /// <param name="message">通知内容</param>
        /// <param name="growType">通知类型</param>
        public void GlobalNotice(string message, GrowType growType)
        {
            this.RaiseNoticMessageEvent?.Invoke(message, growType);
        }

    }
}
