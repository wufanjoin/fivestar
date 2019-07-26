using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public class CardFiveStarHand : CardFiveStarCard
    {
        public int iHandIndex;
        protected const float _MoveTime = 0.6f;
        protected const float _HandInterval = 1.9f;
        protected static float _width = 0;

        public GameObject _HuPaiHintJianArrowsGo;
        public GameObject _PaoIconGo;
        public GameObject _KouIconGo;
        public GameObject _NotStateGo;
        private PointerEvent _PointerEvent;
        public override void Init(GameObject go)
        {
            base.Init(go);
            _HuPaiHintJianArrowsGo = gameObject.FindChild("HuPaiHintJianArrowsGo").gameObject;
            _PaoIconGo = gameObject.FindChild("PaoIconGo").gameObject;
            _KouIconGo = gameObject.FindChild("KouIconGo").gameObject;
            _NotStateGo = gameObject.FindChild("NotStateGo").gameObject;

            if (_width == 0)
            {
                RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
                _width = rectTransform.rect.width - _HandInterval;
            }

            _PointerEvent = gameObject.AddComponent<PointerEvent>();
            //注册事件委托
            _PointerEvent.OnPointerDownAction -= OnPointerDown;
            _PointerEvent.OnPointerUpAction -= OnPointerUp;
            _PointerEvent.OnPointerEnterAction -= OnPointerEnter;
            _PointerEvent.OnPointerDownAction += OnPointerDown;
            _PointerEvent.OnPointerUpAction += OnPointerUp;
            _PointerEvent.OnPointerEnterAction += OnPointerEnter;
            _PointerEvent.OnPointerExitAction -= OnPointerExit;
            _PointerEvent.OnPointerExitAction += OnPointerExit;



            
            EventMsgMgr.RegisterEvent(CardFiveStarEventID.CanChuCardHaveHuHint, DetetionChuCanHu);
            EventMsgMgr.RegisterEvent(CardFiveStarEventID.UserChuCard, UserChuCard);
            EventMsgMgr.RegisterEvent(CardFiveStarEventID.PlayerLiangDao, PlayerLiangDaoEvnet);
            EventMsgMgr.RegisterEvent(CardFiveStarEventID.PlayerSelectLiangCardState, PlayerSelectLiangCardStateEvent);
        }

        //初始化UI
        private void InitUI()
        {
            _HuPaiHintJianArrowsGo.SetActive(false);
            _PaoIconGo.SetActive(false);
            _KouIconGo.SetActive(false);
            _NotStateGo.SetActive(false);
            if (CardFiveStarRoom.Ins!=null&& CardFiveStarRoom.Ins._LiangDaoCanHuCards.Contains(CardSize))
            {
                _PaoIconGo.SetActive(true);
            }
        }
        //每次 重新 从对象池取取来 就会设置card
        public override void SetCardUI(int size)
        {
            base.SetCardUI(size);
            InitUI();
        }

        //玩家选择 是否亮
        public void PlayerSelectLiangCardStateEvent(params object[] objs)
        {
            bool isSelectLiang = (bool) objs[0];
            if (isSelectLiang)
            {
                if (_IsCanChuTingHu)
                {
                    SetCanOperationState(true);
                }
                else
                {
                    SetCanOperationState(false);
                }
            }
            else
            {
                SetCanOperationState(true);
            }
        }
        //设置可操作状态
        public void SetCanOperationState(bool state)
        {
            _PointerEvent.enabled = state;
            _NotStateGo.SetActive(!state);
        }

        private List<int> _ChuTingHuList;//出牌听胡列表
        private bool _IsCanChuTingHu = false;//出这牌是否能听胡
        //检测出这牌是否 能听
        public void DetetionChuCanHu(params object[] objs)
        {
            Dictionary<int, List<int>> huHintDic= objs[0] as Dictionary<int, List<int>>;
            if (huHintDic.ContainsKey(CardSize))
            {
                _IsCanChuTingHu = true;
                _ChuTingHuList = huHintDic[CardSize];
                _HuPaiHintJianArrowsGo.SetActive(true);
            }
            else
            {
                _IsCanChuTingHu = false;
            }
        }
        //有玩家亮倒了
        public void PlayerLiangDaoEvnet(params object[] objs)
        {
            if (CardFiveStarRoom.Ins._LiangDaoCanHuCards.Contains(CardSize))
            {
                _PaoIconGo.SetActive(true);
            }
        }
        //用户出牌 就是自己出牌
        public void UserChuCard(params object[] objs)
        {
            //清除 胡牌提示的一些状态
            _IsCanChuTingHu = false;
            _ChuTingHuList = null;
            _HuPaiHintJianArrowsGo.SetActive(false);
            //设为可操作状态
            SetCanOperationState(true);
        }

        //鼠标移出
        public void OnPointerExit()
        {
          

        }
        //按下
        public void OnPointerDown()
        {
            //暂时只有按下
            CardFiveStarHandComponent.Ins.PointerDownHand(this);
           // Log.Debug("按下");
        }
        //弹起
        public void OnPointerUp()
        {
           // Log.Debug("弹起");
            CardFiveStarHandComponent.Ins.EndDetecionMousePlayCard();
        }
        //鼠标移进
        public void OnPointerEnter()
        {
           
        }

        public Tweener _MoveTweener;
        public void RightMove()
        {
            _MoveTweener = gameObject.transform.DOLocalMoveX(gameObject.transform.localPosition.x + _width, _MoveTime);
        }
        public void LeftMove()
        {
            _MoveTweener=gameObject.transform.DOLocalMoveX(gameObject.transform.localPosition.x - _width, _MoveTime);
        }

        //当前是否选中
        private bool _isPitch = false;
        //选中的偏移量
        private static Vector3 CardPithOffse = new Vector3(0, 25, 0);
        //设置选中状态
        public void SetPitchStatu(bool pitch)
        {
            if (pitch && !_isPitch)
            {
                ShowHuHintPanel();
                gameObject.transform.localPosition += CardPithOffse;
            }
            else if (_isPitch)
            {
                gameObject.transform.localPosition -= CardPithOffse;
            }
            _isPitch = pitch;
        }

        //根据这张 打出牌能否你牌 当前能否胡牌 显示胡牌提示
        public void ShowHuHintPanel()
        {
            if (!CardFiveStarHandComponent.Ins._IsCanChuCard)
            {
                return; //当前不能出牌 点击牌 和胡牌提示无关
            }
            if (_IsCanChuTingHu)
            {
                UIComponent.GetUiView<FiveStarMingPaiHintPanelComponent>().ShowSelfChuCardHint(_ChuTingHuList,CardSize);
            }
            else
            {
                UIComponent.GetUiView<FiveStarMingPaiHintPanelComponent>().HideHuCardHint(0);
            }
          
        }

       
        //销毁这个牌
        public override void Destroy()
        {
            _MoveTweener?.Pause();//打牌移动 动画暂停
            SetPitchStatu(false);
            _HuPaiHintJianArrowsGo.SetActive(false);
            _PaoIconGo.SetActive(false);
            _KouIconGo.SetActive(false);
            _NotStateGo.SetActive(false);
            base.Destroy();
        }

        //
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
