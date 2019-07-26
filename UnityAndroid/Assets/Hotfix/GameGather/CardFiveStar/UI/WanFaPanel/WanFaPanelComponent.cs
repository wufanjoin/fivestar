using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    /// <summary>
    /// 玩法弹窗Pop
    /// </summary>
    [UIComponent(UIType.WanFaPanel)]
    public class WanFaPanelComponent : PopUpUIView
    {

        #region 脚本工具生成的代码
        private GameObject mWanFaInfoItemGo;
        private Text mGameNameTitleText;//游戏标题可能会用到
        private Button mCloseBtn;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mWanFaInfoItemGo = rc.Get<GameObject>("WanFaInfoItemGo");
            mGameNameTitleText = rc.Get<GameObject>("GameNameTitleText").GetComponent<Text>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public override float MaskLucencyValue
        {
            get
            {
                return 0f;
            }
        }
        public void InitPanel()
        {
            mCloseBtn.Add(Hide);
            
        }
        public override void OnShow()
        {
            base.OnShow();
            SetWanFaPanel();
        }

        List<WanFaInfoItem> _wanFaInfoItems = new List<WanFaInfoItem>();
        public void SetWanFaPanel()
        {
            FiveStarRoomConfig config;// = CardFiveStarRoom.Ins._config;
            int roomType = RoomType.RoomCard;
            if (CardFiveStarRoom.Ins != null)
            {
                config = CardFiveStarRoom.Ins._config;
                roomType = CardFiveStarRoom.Ins._RoomType;
            }
            else
            {
                config = FiveStarVideoRoom.Ins._RoomConfig;
            }
            
            List<WanFaInfoData> WanFaInfoData = new List<WanFaInfoData>();

            if (roomType == RoomType.RoomCard)
            {
                string payMoeyDesc = CardFiveStarRoomConfig.GetDescIdAndValueIn(CardFiveStarRoomConfig.PayMoneyId, config.PayMoneyType);//房费描述
                WanFaInfoData.Add(new WanFaInfoData(true, "房费:" + payMoeyDesc));
            }
   

            string piaoNumDesc = CardFiveStarRoomConfig.GetDescIdAndValueIn(CardFiveStarRoomConfig.FloatNumId, config.MaxPiaoNum);//打漂描述
            if (config.MaxPiaoNum > 0)
            {
                WanFaInfoData.Add(new WanFaInfoData(true, piaoNumDesc));
            }
            else
            {
                WanFaInfoData.Add(new WanFaInfoData(false, "打漂"));
            }
            string maiMaDesc = CardFiveStarRoomConfig.GetDescIdAndValueIn(CardFiveStarRoomConfig.MaiMaId, config.MaiMaType);//买马描述
            if (config.MaiMaType > 0)
            {
                WanFaInfoData.Add(new WanFaInfoData(true, maiMaDesc));
            }
            else
            {
                WanFaInfoData.Add(new WanFaInfoData(false, "买马"));
            }
            string waiShiwuDesc = CardFiveStarRoomConfig.GetDescIdAndValueIn(CardFiveStarRoomConfig.WaiShiWuId, config.WaiShiWuType);//外十五描述
            WanFaInfoData.Add(new WanFaInfoData(config.WaiShiWuType > 0, "自带一漂"));
            string fengdingDesc = CardFiveStarRoomConfig.GetDescIdAndValueIn(CardFiveStarRoomConfig.FengDingFanShuId, config.FengDingFanShu);//封顶描述
            fengdingDesc = "封顶:" + fengdingDesc;
            WanFaInfoData.Add(new WanFaInfoData(true, fengdingDesc));
            //显示玩法信息
            Transform wanFaItemParent = mWanFaInfoItemGo.transform.parent;
            wanFaItemParent.CreatorChildAndAddItem<WanFaInfoItem, WanFaInfoData>(WanFaInfoData);
        }
        

    }
}
