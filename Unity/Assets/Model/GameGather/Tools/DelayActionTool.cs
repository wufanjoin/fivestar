using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public class DelayAction
    {
        public int _ActionId;
        public int _ResidueTime;
        public Action _CallAction;

        public async void StartDelayCall(int actionId,int delayTime,Action call)
        {
            _ActionId = actionId;
            _ResidueTime = delayTime;
            _CallAction = call;
            while (_ResidueTime>0)
            {
                _ResidueTime -= 100;
                await Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
            }
            call?.Invoke();
            DelayActionTool.FinishDelayAction(this);
        }

        public void RefreshTime(int time)
        {
            _ResidueTime = time;
        }
    }
    public static class DelayActionTool
    {
        public static Dictionary<int, DelayAction> _BeingExecuteAcionDic = new Dictionary<int, DelayAction>();
        private static List<DelayAction> _delayActionPool=new List<DelayAction>();

       //延迟执行一个事件 如果ID相同 就会再次推迟执行
        public static void ExecuteDelayAction(int actionId,int time,Action action)
        {
            if (_BeingExecuteAcionDic.ContainsKey(actionId))
            {
                _BeingExecuteAcionDic[actionId].RefreshTime(time);
                return;
            }
            DelayAction delayAction;
            if (_delayActionPool.Count > 0)
            {
                 delayAction = _delayActionPool[0];
                _delayActionPool.RemoveAt(0);
            }
            else
            {
                 delayAction=new DelayAction();
            }
            delayAction.StartDelayCall(actionId, time, action);
            _BeingExecuteAcionDic.Add(delayAction._ActionId, delayAction);
        }

        //延迟执行完成
        public static void FinishDelayAction(DelayAction delayAction)
        {
            if (_BeingExecuteAcionDic.ContainsKey(delayAction._ActionId))
            {
                _BeingExecuteAcionDic.Remove(delayAction._ActionId);
            }
            _delayActionPool.Add(delayAction);
        }
    }
}
