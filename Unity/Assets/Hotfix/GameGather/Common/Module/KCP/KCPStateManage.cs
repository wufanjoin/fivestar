using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETHotfix;
using ETModel;

namespace ETHotfix
{
    public static class KCPNetWorkState
    {
        public const string BebeingConnect = "BebeingConnect";
        public const string Disconnectl = "Disconnectl";
        public const string Connect = "Connect";
    }
    [ObjectSystem]
    public class KCPConnectStateEventAwakeSystem : AwakeSystem<KCPStateManage>
    {
        public override void Awake(KCPStateManage self)
        {
            self.Awake();
        }
    }
    public  class KCPStateManage:Component
    {
        public string pKCPNetWorkState {  set; get; }

        public Action pStartConnectCall;//开始连接
        public Action<G2C_GateLogin> pConnectSuccessCall;//连接成功
        public Action pConnectFailureCall; //连接失败
        public Action pConnectLostCall;//连接断开
        public Action pStartReconnectionCall;//开始重连
        public Action<G2C_GateLogin> pAgainConnectSuccessCall;//重连成功
        public Action pAgainConnectFailureCall;//重连失败
        public Action pInitiativeDisconnectCall;//主动断开连接

        //单例模式
        public static KCPStateManage Ins { private set; get; }

        public void Awake()
        {
            Ins = this;
        }
        //开始连接
        public void StartConnect()
        {
            Log.Debug("开始连接");
            pKCPNetWorkState = KCPNetWorkState.BebeingConnect;
            pStartConnectCall?.Invoke();
        }
        //连接成功
        public void ConnectSuccess(G2C_GateLogin g2CGateLogin)
        {
            Log.Debug("连接成功");
            pKCPNetWorkState = KCPNetWorkState.Connect;
            pConnectSuccessCall?.Invoke(g2CGateLogin);
        }

        //连接失败
        public void ConnectFailure()
        {
            Log.Debug("连接失败");
            pKCPNetWorkState = KCPNetWorkState.Disconnectl;
            pConnectFailureCall?.Invoke();
        }

        //连接断开
        public async void ConnectLost()
        {
            Log.Debug("连接断开");
            pKCPNetWorkState = KCPNetWorkState.Disconnectl;
            await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);//连接断开后等待1秒 才发起重连 因为底层需要对Session进行一些处理
            pConnectLostCall?.Invoke();
        }

        //开始重连
        public void StartReconnection()
        {
            Log.Debug("开始重连");
            pKCPNetWorkState = KCPNetWorkState.BebeingConnect;
            pStartReconnectionCall?.Invoke();
        }
        //重连成功
        public void AgainConnectSuccess(G2C_GateLogin g2CGateLogin)
        {
            Log.Debug("重连成功");
            pKCPNetWorkState = KCPNetWorkState.Connect;
            pAgainConnectSuccessCall?.Invoke(g2CGateLogin);
        }
        //重连失败
        public void AgainConnectFailure()
        {

            Log.Debug("重连失败");
            pKCPNetWorkState = KCPNetWorkState.Disconnectl;
            pAgainConnectFailureCall?.Invoke();

        }
        //主动断开连接
        public void DisconnectInitiative()
        {
            Log.Debug("主动连接断开");
            pKCPNetWorkState = KCPNetWorkState.Disconnectl;
            pInitiativeDisconnectCall?.Invoke();
        }
    }
}
