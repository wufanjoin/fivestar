using System;
using System.Collections.Generic;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class TurntableActivityView : BaseView
    {
        #region 脚本工具生成的代码
        private GameObject mTurntableTurnGo;
        private Button mDrawLotteryBtn;
        private Text mConsumeJewelHinttText;
        private Text mTaskHintText;
        private Text mFreeNumText;
        private GameObject mGoodsPointParentGo;
        private GameObject mGoodsItemGo;
        private Button mWinPrizeRecordBtn;
        private GameObject mWinPrizeRecordViewGo;


        public override void Init(GameObject go)
        {
            base.Init(go);
            mTurntableTurnGo = rc.Get<GameObject>("TurntableTurnGo");
            mDrawLotteryBtn = rc.Get<GameObject>("DrawLotteryBtn").GetComponent<Button>();
            mConsumeJewelHinttText = rc.Get<GameObject>("ConsumeJewelHinttText").GetComponent<Text>();
            mTaskHintText = rc.Get<GameObject>("TaskHintText").GetComponent<Text>();
            mFreeNumText = rc.Get<GameObject>("FreeNumText").GetComponent<Text>();
            mGoodsPointParentGo = rc.Get<GameObject>("GoodsPointParentGo");
            mGoodsItemGo = rc.Get<GameObject>("GoodsItemGo");
            mWinPrizeRecordBtn = rc.Get<GameObject>("WinPrizeRecordBtn").GetComponent<Button>();
            mWinPrizeRecordViewGo = rc.Get<GameObject>("WinPrizeRecordViewGo");
            InitPanel();
        }
        #endregion

        private WinPrizeRecordView _winPrizeRecordView;
        public void InitPanel()
        {
            _winPrizeRecordView = mWinPrizeRecordViewGo.AddItem<WinPrizeRecordView>();
            mWinPrizeRecordBtn.Add(WinPrizeRecordBtnEvent);
            mDrawLotteryBtn.Add(DrawLotteryBtnEvent);
            InitTurntableGoodsList();
            RefreshFreeCount();
        }
        public Vector3 ChouZhongVector3=Vector3.zero;
        public Vector3 ChouJiangRotaAngele=new Vector3(0,0,-8);

        public bool IsDrawLotteyIn = false;//是否正在抽奖中

        public void DrawLotteryBtnEvent()
        {
            if (IsDrawLotteyIn)
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("请等待当前抽奖结束");
            }
            else  if (_FreeCount > 0)
            {
                StartDrawLottery();
            }
            else
            {
                if (UserComponent.Ins.pSelfUser.Jewel < 10)
                {
                    UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("钻石不足无法进行");
                    return;
                }
                UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow("是否消耗10钻石进行抽奖", StartDrawLottery);
            }
        }
        //开始抽奖
        public async void StartDrawLottery(bool confirm=true)
        {
            if (IsDrawLotteyIn|| !confirm)
            {
                return;
            }
            IsDrawLotteyIn = true;
            L2C_TurntableDrawLottery l2CGetTurntableGoodss = (L2C_TurntableDrawLottery)await SessionComponent.Instance.Call(new C2L_TurntableDrawLottery());
            if (l2CGetTurntableGoodss.Error == 0)
            {
                if (_FreeCount > 0)
                {
                    SetFreeCount(_FreeCount - 1);
                }
                TurntableGoods zhongJiangGoods=null;
                foreach (var item in _TurntableGoodsItems)
                {
                    if (item.mData.TurntableGoodsId == l2CGetTurntableGoodss.TurntableGoodsId)
                    {
                        ChouZhongVector3.z = item._ChouZongRotaZ;
                        zhongJiangGoods = item.mData;
                        break;
                    }
                }
                int rotaTime = 50;
                while (rotaTime-->0)
                {
                    await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(50);
                    mTurntableTurnGo.transform.Rotate(ChouJiangRotaAngele);
                }
                while (Math.Abs(mTurntableTurnGo.transform.localEulerAngles.z - ChouZhongVector3.z)>15)
                {
                    await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(50);
                    mTurntableTurnGo.transform.Rotate(ChouJiangRotaAngele);
                }
                if (zhongJiangGoods != null&& zhongJiangGoods.GoodsId!=GoodsId.None)
                {
                    UIComponent.GetUiView<GetGoodsHintPanelComponent>().ShowGetGoods(zhongJiangGoods.GoodsId, zhongJiangGoods.Amount);
                }
                else
                {
                    UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("谢谢参与");
                }
            }
            IsDrawLotteyIn = false;
        }
        public List<TurntableGoodsItemGo> _TurntableGoodsItems=new List<TurntableGoodsItemGo>();
        //初始化转盘物品列表
        public async void InitTurntableGoodsList()
        {
            _TurntableGoodsItems.Clear();
            L2C_GetTurntableGoodss  l2CGetTurntableGoodss= (L2C_GetTurntableGoodss)await SessionComponent.Instance.Call(new C2L_GetTurntableGoodss());
            for (int i = 0; i < mGoodsPointParentGo.transform.childCount&&i< l2CGetTurntableGoodss.Goodss.Count; i++)
            {
               GameObject  goodsGo=GameObject.Instantiate(mGoodsItemGo, mGoodsPointParentGo.transform.GetChild(i));
                goodsGo.transform.localPosition=Vector3.zero;
                TurntableGoodsItemGo item= goodsGo.AddItemIfHaveInit<TurntableGoodsItemGo, TurntableGoods>(l2CGetTurntableGoodss.Goodss[i]);
                _TurntableGoodsItems.Add(item);
            }
        }

        //获取免费抽奖次数
        public async void RefreshFreeCount()
        {
            L2C_GetFreeDrawLotteryCount l2CGetFreeDrawLotteryCount = (L2C_GetFreeDrawLotteryCount)await SessionComponent.Instance.Call(new C2L_GetFreeDrawLotteryCount());
            SetFreeCount(l2CGetFreeDrawLotteryCount.Count);
        }
        //免费抽奖次数
        public int _FreeCount = 0;
        public void SetFreeCount(int freeCount)
        {
            mFreeNumText.text = $"当前免费抽奖次数剩余<color=#FCFF44FF> {freeCount} </color>次";
            _FreeCount = freeCount;
        }
        //显示中奖记录面板
        public void WinPrizeRecordBtnEvent()
        {
            _winPrizeRecordView.ShowRecord();
        }
    }
}
