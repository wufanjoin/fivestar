using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.FiveStarTotalResultPanel)]
    public class FiveStarTotalResultPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mReturnBtn;
        private GameObject mPlayerInfoItemGo;
        private Button mShareBtn;
        private Button mReturnLobbyBtn;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mReturnBtn = rc.Get<GameObject>("ReturnBtn").GetComponent<Button>();
            mPlayerInfoItemGo = rc.Get<GameObject>("PlayerInfoItemGo");
            mShareBtn = rc.Get<GameObject>("ShareBtn").GetComponent<Button>();
            mReturnLobbyBtn = rc.Get<GameObject>("ReturnLobbyBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        List<TotalPlayerInfoItemGoItem> _totalPlayerInfos=new List<TotalPlayerInfoItemGoItem>();
        public void InitPanel()
        {
            InitTotalPlayerInfo();
            mShareBtn.Add(ShareBtnEvent);
            mReturnBtn.Add(ReturnEvent);
            mReturnLobbyBtn.Add(ReturnEvent);
        }

        public void ShareBtnEvent()
        {
            SdkMgr.Ins.WeChatShareScreen();
        }

        public void ReturnEvent()
        {
            Game.Scene.GetComponent<ToyGameComponent>().EndGame();
            Hide();
        }
        //初始化总结算 玩家Player信息
        private void InitTotalPlayerInfo()
        {
            Transform _playerGroupParentTrm = mPlayerInfoItemGo.transform.parent;
            for (int i = 0; i < 3; i++)
            {
                GameObject.Instantiate(mPlayerInfoItemGo, _playerGroupParentTrm);
            }
            for (int i = 0; i < _playerGroupParentTrm.childCount; i++)
            {
                TotalPlayerInfoItemGoItem  totalPlayerInfoItem=_playerGroupParentTrm.GetChild(i)
                    .AddItemIfHaveInit<TotalPlayerInfoItemGoItem, FiveStarTotalPlayerResult>(
                        new FiveStarTotalPlayerResult());
                _totalPlayerInfos.Add(totalPlayerInfoItem);
            }
        }

        //显示总结算
        public void ShowTotalResultInfo(Actor_FiveStar_TotalResult totalResult)
        {
            Show();
            GetBigWinAndPaoShouIndex(totalResult.TotalPlayerResults);
            for (int i = 0; i < totalResult.TotalPlayerResults.Count; i++)
            {
                _totalPlayerInfos[i].SetUI(totalResult.TotalPlayerResults[i]);
            }
            for (int i = totalResult.TotalPlayerResults.Count; i < _totalPlayerInfos.Count; i++)
            {
                _totalPlayerInfos[i].Hide();
            }
        }

        //计算大赢家和 最佳炮手的索引
        public int bigWinSeatIndex;
        public int PaoShouSeatIndex;
        public void GetBigWinAndPaoShouIndex(RepeatedField<FiveStarTotalPlayerResult> playerResults)
        {
            int _scoreCount=0;
            int _fangChongNumCount=0;
            for (int i = 0; i < playerResults.Count; i++)
            {
                if (playerResults[i].TotalSocre > _scoreCount)
                {
                    _scoreCount = playerResults[i].TotalSocre;
                    bigWinSeatIndex = playerResults[i].SeatIndex;
                }
                if (playerResults[i].FangChongCount > _fangChongNumCount)
                {
                    _fangChongNumCount = playerResults[i].FangChongCount;
                    PaoShouSeatIndex = playerResults[i].SeatIndex;
                }
            }
        }
    }
}
