using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class KCPDisconnectDisposeAwakeSystem : AwakeSystem<KCPLocalizationDispose>
    {
        public override void Awake(KCPLocalizationDispose self)
        {
            self.Awake();
        }
    }
    public class KCPLocalizationDispose : Component
    {
        public void Awake()
        {
            KCPStateManage.Ins.pConnectLostCall += ConnectLostEvent;
            KCPStateManage.Ins.pAgainConnectFailureCall += AgainConnectFailureEvent;

            KCPStateManage.Ins.pStartConnectCall += StartConnectEvent;
            KCPStateManage.Ins.pStartReconnectionCall += StartReconnectionEvent;
            KCPStateManage.Ins.pConnectFailureCall += ConnectFailureEvent;
            KCPStateManage.Ins.pConnectSuccessCall += ConnectSuccessEvent;
            KCPStateManage.Ins.pAgainConnectSuccessCall += AgainConnectSuccessEvent;
        }

        //开始连接
        private void StartConnectEvent()
        {
            UIComponent.GetUiView<LoadingIconPanelComponent>().Show();
        }
        //开始重连
        private void StartReconnectionEvent()
        {
            UIComponent.GetUiView<LoadingIconPanelComponent>().Show();
        }
        //连接失败
        private  void ConnectFailureEvent()
        {
            UIComponent.GetUiView<LoadingIconPanelComponent>().Hide();
            UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow("网络连接失败是否重试", ConnectFailureConift);
        }
        //连接失败进行重连
        public void ConnectFailureConift(bool bol)
        {
            if (bol)
            {
                Game.Scene.GetComponent<KCPUseManage>().AgainLoginAndConnect();
            }
            else
            {
                Game.Scene.GetComponent<ToyGameComponent>().StartGame(ToyGameId.Login);
            }
        }
        //连接成功
        private void ConnectSuccessEvent(G2C_GateLogin g2CGateLogin)
        {
            UIComponent.GetUiView<LoadingIconPanelComponent>().Hide();
            User user = g2CGateLogin.User;
            Game.Scene.GetComponent<UserComponent>().SetSelfUser(user); ;
            Game.Scene.GetComponent<ToyGameComponent>().StartGame(ToyGameId.Lobby);
            SessionComponent.Instance.Session.AddComponent<HeartbeatComponent>();//添加心跳组件 
        }
        //重连成功
        private void AgainConnectSuccessEvent(G2C_GateLogin g2CGateLogin)
        {
            UIComponent.GetUiView<LoadingIconPanelComponent>().Hide();
            SessionComponent.Instance.Session.AddComponent<HeartbeatComponent>();//添加心跳组件 
        }
        //连接断开
        private void ConnectLostEvent()
        {
            KCPUseManage.Ins.Reconnection();
        }
        //重连失败后弹窗
        private void AgainConnectFailureEvent()
        {
            UIComponent.GetUiView<LoadingIconPanelComponent>().Hide();

            UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow("重新连接失败是否重试", (bol) =>
            {
                if (bol)
                {
                    KCPUseManage.Ins.Reconnection();
                }
                else
                {
                    Game.Scene.GetComponent<ToyGameComponent>().StartGame(ToyGameId.Login);
                }
            });
        }
    }
}
