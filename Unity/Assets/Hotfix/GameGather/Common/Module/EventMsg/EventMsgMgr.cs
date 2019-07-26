
using System.Collections.Generic;

namespace ETHotfix
{
    public static class EventMsgMgr
    {
        public delegate void EventFunc(params object[] objs);

        private static Dictionary<string, EventFunc> _messageHanlde = new Dictionary<string, EventFunc>();


        //注册消息
        public static void RegisterEvent(string eventID, EventFunc func)
        {
            if (_messageHanlde.ContainsKey(eventID))
            {
                _messageHanlde[eventID] -= func;
                _messageHanlde[eventID] += func;
            }
            else
            {
                _messageHanlde.Add(eventID, func);
            }
        }

        //注销消息
        public static void RemoveEvent(string eventID, EventFunc func)
        {
            if (_messageHanlde.ContainsKey(eventID))
            {
                _messageHanlde[eventID] -= func;
                if (null == _messageHanlde[eventID]) _messageHanlde.Remove(eventID);
            }
        }

        //注销所有消息
        public static void AllRemoveEvent()
        {
            _messageHanlde.Clear();
        }

        //发送消息
        public static bool SendEvent(string eventID, params object[] objs)
        {
            EventFunc func;
            if (_messageHanlde.TryGetValue(eventID, out func))
            {
                func(objs);
                return true;
            }
            return false;
        }
    }
}
