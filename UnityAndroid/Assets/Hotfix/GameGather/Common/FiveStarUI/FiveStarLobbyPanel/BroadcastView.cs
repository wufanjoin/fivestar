using System;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class BroadcastView : BaseView
    {
        #region 脚本工具生成的代码
        private Text mBroadcastText;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mBroadcastText = rc.Get<GameObject>("BroadcastText").GetComponent<Text>();
            InitPanel();
        }
        #endregion

        private BroadcastPlay _BroadcastPlay;
        public async void InitPanel()
        {
            L2C_GetAnnouncement l2CGetAnnouncement=(L2C_GetAnnouncement) await SessionComponent.Instance.Call(new C2L_GetAnnouncement());
            _BroadcastPlay=new BroadcastPlay();
            _BroadcastPlay.RegisterBroadcast(mBroadcastText, l2CGetAnnouncement.Message);
        }

        public override async void Show()
        {
            base.Show();
            if (_BroadcastPlay != null)
            {
                _BroadcastPlay.PlayBroadcast();
            }
        }
    }

    public class BroadcastPlay
    {
        public Text _BroadcastText;

        public float _ParentWidth;
        public float _SelfWidth;

        public float _MoveEndPoint;
        public Vector2 _MoveStartPoint;
        public Vector3 Speed=new Vector3(2,0,0);
        public async void RegisterBroadcast(Text broadcastText,string content)
        {
            _BroadcastText = broadcastText;

            broadcastText.text = content;

            await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);

            _ParentWidth = broadcastText.transform.parent.GetComponent<RectTransform>().rect.width;
            _SelfWidth = broadcastText.transform.GetComponent<RectTransform>().rect.width;

            broadcastText.transform.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);

            _MoveEndPoint = -_ParentWidth / 2 -_SelfWidth;
            _MoveStartPoint=new Vector2(_ParentWidth / 2, 0);

            PlayBroadcast();
        }

        private bool isPlayBroadcastIn = false;//是否在播放广播中
        public async void PlayBroadcast()
        {
            if (isPlayBroadcastIn)
            {
                return;
            }
            isPlayBroadcastIn = true;
            while (_BroadcastText.gameObject.activeInHierarchy)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(50);
                _BroadcastText.transform.localPosition -= Speed;
                if (_BroadcastText.transform.localPosition.x <= _MoveEndPoint)
                {
                    _BroadcastText.transform.localPosition = _MoveStartPoint;
                }
            }
            isPlayBroadcastIn = false;
        }
    }
}
