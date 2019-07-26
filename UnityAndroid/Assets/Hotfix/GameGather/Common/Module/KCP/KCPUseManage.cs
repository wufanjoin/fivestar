using ETModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using UnityEngine;


namespace ETHotfix
{
 

    [ObjectSystem]
    public class KCPMgrComponentAwakeSystem : AwakeSystem<KCPUseManage>
    {
        public override void Awake(KCPUseManage self)
        {
            self.AddComponent<KCPStateManage>();
            self.AddComponent<KCPLocalizationDispose>();
            self.Awake();
        }

       
    }

    public class KCPUseManage : Entity
    {
        public static KCPUseManage Ins { private set; get; }
        //自己的StateEvent组件
        private KCPStateManage _mStateManage;
        public void Awake()
        {
            Ins = this;
            _mStateManage= this.GetComponent<KCPStateManage>();
        }

        //用上次登陆方式和信息重新登陆
        public void AgainLoginAndConnect(bool isReconnection = false)
        {
            LoginAndConnect(_UpLoginType, _UpLoginDataStr, isReconnection);
        }
        private int _UpLoginType;
        private string _UpLoginDataStr;
        //登陆并连接
        public async void LoginAndConnect(int loginType, string dataStr,bool isReconnection=false)
        {
            _UpLoginType = loginType;
            _UpLoginDataStr = dataStr;
            if (_mStateManage.pKCPNetWorkState == KCPNetWorkState.BebeingConnect|| _mStateManage.pKCPNetWorkState == KCPNetWorkState.Connect)
            {
                Log.Warning("正在连接 请不要重复连接 或已经成功连接");
            }
            try
            {
                
               
                _mStateManage.StartConnect();
                //根据是否重连 注册 连接成功 是连接失败的事件
                mSocketCantConnectCall = _mStateManage.ConnectFailure;//连接失败
                Action<G2C_GateLogin> connectSuccesAction = _mStateManage.ConnectSuccess;//连接成功
                //如果是重连 更改一下回调
                if (isReconnection)
                {
                    mSocketCantConnectCall = _mStateManage.AgainConnectFailure;
                    connectSuccesAction = _mStateManage.AgainConnectSuccess;
                }
                Log.Debug("验证服地址:"+ GameVersionsConfigMgr.Ins.ServerAddress);
                // 创建一个ETModel层的Session
                ETModel.Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(GameVersionsConfigMgr.Ins.ServerAddress);
                // 创建一个ETHotfix层的Session, ETHotfix的Session会通过ETModel层的Session发送消息
                Session realmSession = ComponentFactory.Create<Session, ETModel.Session>(session);
                realmSession.session.GetComponent<SessionCallbackComponent>().DisposeCallback += RealmSessionDisposeCallback;
                //登陆验证服务器
                R2C_CommonLogin r2CLogin =(R2C_CommonLogin)await realmSession.Call(new C2R_CommonLogin()
                {
                    LoginType = loginType,
                    PlatformType = HardwareInfos.pCurrentPlatform,
                    DataStr = dataStr,
                });
                realmSession.Dispose();
                if (!string.IsNullOrEmpty(r2CLogin.Message))
                {
                    if (PlayerPrefs.HasKey(GlobalConstant.LoginVoucher))
                    {
                        PlayerPrefs.DeleteKey(GlobalConstant.LoginVoucher);//登陆失败的话 如果有凭证 就删除凭证
                    }
                    UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow(r2CLogin.Message,null, PopOptionType.Single);//显示提示
                    UIComponent.GetUiView<LoadingIconPanelComponent>().Hide();//隐藏圈圈
                    _mStateManage.pKCPNetWorkState = KCPNetWorkState.Disconnectl;//状态改为断开连接
                    Game.Scene.GetComponent<ToyGameComponent>().StartGame(ToyGameId.Login);//进入登陆界面
                    return;
                }
                PlayerPrefs.SetString(GlobalConstant.LoginVoucher, r2CLogin.LoginVoucher);//记录登陆凭证
                //登陆网关服务器
                G2C_GateLogin g2CLoginGate =await ConnectGate(r2CLogin.Address, r2CLogin.Key, loginType);
                if (!string.IsNullOrEmpty(g2CLoginGate.Message))
                {
                    UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow(g2CLoginGate.Message, null, PopOptionType.Single);
                    UIComponent.GetUiView<LoadingIconPanelComponent>().Hide();//隐藏圈圈
                    _mStateManage.pKCPNetWorkState = KCPNetWorkState.Disconnectl;//状态改为断开连接
                    return;
                }
                //发起连接成功事件
                connectSuccesAction(g2CLoginGate);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }

        //验证服连接回调
        public void RealmSessionDisposeCallback(ETModel.Session s)
        {
            if (s == null)
            {
                return;
            }
            switch (s.Error)
            {
                case ErrorCode.ERR_Success:
                    Log.Debug("验证服Session正常销毁");
                    return;
                case ErrorCode.ERR_KcpCantConnect:
                case ErrorCode.ERR_SocketCantSend://验证服 消息发送错误 也算连接失败
                    mSocketCantConnectCall?.Invoke();
                    Log.Error("验证服ERR_KcpCantConnect");//连接失败
                    break;
                case ErrorCode.ERR_SocketError://连接断开
                    mSocketCantConnectCall?.Invoke();//验证服 连接断开 只发起连接失败的事件
                    Log.Error("验证服ERR_SocketDisconnected");
                    break;
                default:
                    mSocketCantConnectCall?.Invoke();//连接断开
                    break;
            }
            Log.Debug("验证服Session销毁 ErrorCode:" + s.Error.ToString());
        }


        //重新连接
        public  void Reconnection()
        {
            if (_mStateManage.pKCPNetWorkState == KCPNetWorkState.BebeingConnect|| _mStateManage.pKCPNetWorkState == KCPNetWorkState.Connect)
            {
                Log.Warning("正在连接 请不要重复连接 或已经成功连接");
                return;
            }
            try
            {
                _mStateManage.StartReconnection();
                 AgainLoginAndConnect(true);//开始重连
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }

        //连接网关
        public async ETTask<G2C_GateLogin> ConnectGate(string gateAddress,long key,int loginType)
        {

            try
            {
                // 创建一个ETModel层的Session,并且保存到ETModel.SessionComponent中
                ETModel.Session gateSession = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(gateAddress); //连接网关
                ETModel.Game.Scene.GetComponent<ETModel.SessionComponent>().Session = gateSession;
                // 创建一个ETHotfix层的Session, 并且保存到ETHotfix.SessionComponent中
                Game.Scene.GetComponent<SessionComponent>().Session = ComponentFactory.Create<Session, ETModel.Session>(gateSession);
                gateSession.GetComponent<SessionCallbackComponent>().DisposeCallback += GateSessionDisposeCallback;
                G2C_GateLogin g2CLoginGate = (G2C_GateLogin)await SessionComponent.Instance.Session.Call(new C2G_GateLogin() { Key = key});
                Log.Debug(SessionComponent.Instance.Session.Id.ToString());
                return g2CLoginGate;
        }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
}

        private Action mSocketCantConnectCall = null;
        public void GateSessionDisposeCallback(ETModel.Session session)
        {
            if (session == null)
            {
                return;
            }
            switch (session.Error)
            {
                case ErrorCode.ERR_Success:
                    Log.Debug("网关Session正常销毁");
                    return;
                case ErrorCode.ERR_KcpCantConnect://连接失败
                    mSocketCantConnectCall?.Invoke();
                    break;
                case ErrorCode.ERR_SocketError://连接断开
                case ErrorCode.ERR_SocketCantSend://发消息 无法发现soket断开
                case (int)SocketError.NetworkDown:
                case (int)SocketError.NotConnected:
                    _mStateManage.ConnectLost();//连接断开
                    break;
                //case ErrorCode.ERR_PeerDisconnect://被服务器主动断开 
                //case ErrorCode.ERR_SocketDisconnected://这是服务 没了
                //    UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow("服务器断开连接", (bol) =>
                //    {
                //        Game.Scene.GetComponent<ToyGameComponent>().StartGame(ToyGameId.Login);
                //    }, PopOptionType.Single);
                //    break;
                default:
                    _mStateManage.ConnectLost();//连接断开
                    break;
            }
            Log.Debug("网关Session销毁 ErrorCode:" + session.Error.ToString());
        }
        //断开连接
        public void InitiativeDisconnect()
        {
            if (KCPStateManage.Ins.pKCPNetWorkState!= KCPNetWorkState.Connect&& KCPStateManage.Ins.pKCPNetWorkState != KCPNetWorkState.BebeingConnect)
            {
                return;
            }
            if (SessionComponent.Instance.Session != null)
            {
                SessionComponent.Instance.Session.Dispose();
            }
            _mStateManage.DisconnectInitiative();
        }
    }
}
