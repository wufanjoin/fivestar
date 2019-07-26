using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{

    [UIComponent(UIType.CanOpertionPanel)]
    public class CanOpertionPanelComponent : ChildUIView
    {
        #region 脚本工具生成的代码
        private GameObject _PengGangGo;
        private Button _LiangBtn;
        private Button _HuBtn;
        private Button _QiBtn;
        private GameObject _LiangPaiSelctGo;
        private Button _CancelLiangBtn;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            _PengGangGo = rc.Get<GameObject>("PengGangGo");
            _LiangBtn = rc.Get<GameObject>("LiangBtn").GetComponent<Button>();
            _HuBtn = rc.Get<GameObject>("HuBtn").GetComponent<Button>();
            _QiBtn = rc.Get<GameObject>("QiBtn").GetComponent<Button>();
            _LiangPaiSelctGo = rc.Get<GameObject>("LiangPaiSelctGo");
            _CancelLiangBtn = rc.Get<GameObject>("CancelLiangBtn").GetComponent<Button>();
            _PengGangGo.SetActive(false);
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            _LiangBtn.Add(LiangBtnEvent);
            _HuBtn.Add(HuBtnEvent);
            _QiBtn.Add(QiBtnEvent);
            _CancelLiangBtn.Add(CancelLiangBtnEvent,false);
        }

        public void LiangBtnEvent()
        {
            _LiangPaiSelctGo.SetActive(true);
            _LiangBtn.gameObject.SetActive(false);
            EventMsgMgr.SendEvent(CardFiveStarEventID.PlayerSelectLiangCardState, true);
        }
        public void CancelLiangBtnEvent()
        {
            _LiangPaiSelctGo.SetActive(false);
            _LiangBtn.gameObject.SetActive(true);
            EventMsgMgr.SendEvent(CardFiveStarEventID.PlayerSelectLiangCardState, false);
        }
        public void HuBtnEvent()
        {
            SendOperationMessage(FiveStarOperateType.FangChongHu);
        }
        public void QiBtnEvent()
        {
            SendOperationMessage(FiveStarOperateType.None);
        }

        public static void SendOperationMessage(int operationType, int card = 0)
        {
            Actor_FiveStar_OperateResult operateResult = new Actor_FiveStar_OperateResult();
            FiveStarOperateInfo fiveStarOperate = new FiveStarOperateInfo();
            fiveStarOperate.OperateType = operationType;
            fiveStarOperate.Card = card;
            operateResult.OperateInfo = fiveStarOperate;
            SessionComponent.Instance.Send(operateResult);
        }


        //隐藏所有按钮对象
        public void HideAllBtn()
        {
            foreach (var pengGang in _pengGangBtns)
            {
                pengGang.Hide();
            }
            _QiBtn.gameObject.SetActive(false);
            _LiangBtn.gameObject.SetActive(false);
            _HuBtn.gameObject.SetActive(false);
            _LiangPaiSelctGo.gameObject.SetActive(false);
        }

        //创建对应的碰杠需要对象
        private List<PengGangBtn> _pengGangBtns = new List<PengGangBtn>();
        public void CreatePengGangBtn(int count)
        {
            if (_pengGangBtns.Count >= count)
            {
                return;
            }
            for (int i = _pengGangBtns.Count; i < count; i++)
            {
                _pengGangBtns.Add(new PengGangBtn(GameObject.Instantiate(_PengGangGo, _PengGangGo.transform.parent)));
            }
        }

        //显示操作列表
        public void ShowCanOpration(int oprationType)
        {
            ShowCanOpration(new RepeatedField<int>() { oprationType });
        }

        //显示操作列表
        public void ShowCanOpration(RepeatedField<int> oprationList, RepeatedField<int> gangLits = null)
        {
            Show();
            HideAllBtn();//先隐藏所有Btn
            int pengGangCount = 0;
            if (!oprationList.Contains(FiveStarOperateType.Liang))//只要不是亮牌操作 就有弃
            {
                _QiBtn.gameObject.SetActive(true);
            }
            if (oprationList.Contains(FiveStarOperateType.Peng))
            {
                pengGangCount++;
            }
            if (oprationList.Contains(FiveStarOperateType.MingGang))
            {
                if (gangLits == null)
                {
                    Log.Error("服务器发过来可以杠 但没有杠牌列表");
                    return;
                }
                pengGangCount += gangLits.Count;
            }
            //创建对应的碰杠需要对象
            CreatePengGangBtn(pengGangCount);
            //当前使用到碰杠列表下标索引
            int cuurPengGangBtnIndex = 0;
            for (int i = 0; i < oprationList.Count; i++)
            {
                if (oprationList[i] == FiveStarOperateType.Peng)
                {
                    _pengGangBtns[cuurPengGangBtnIndex++].SetUI(FiveStarOperateType.Peng, CardFiveStarRoom.Ins.iEndChuCardSize);
                }
                else if (oprationList[i] == FiveStarOperateType.MingGang)
                {
                    for (int j = 0; j < gangLits.Count; j++)
                    {
                        _pengGangBtns[cuurPengGangBtnIndex++].SetUI(FiveStarOperateType.MingGang, gangLits[j]);
                    }
                }
                else if (oprationList[i] == FiveStarOperateType.FangChongHu)
                {
                    _HuBtn.gameObject.SetActive(true);
                }
                else if (oprationList[i] == FiveStarOperateType.Liang)
                {
                    _LiangBtn.gameObject.SetActive(true);
                }
            }
        }
    }
}
