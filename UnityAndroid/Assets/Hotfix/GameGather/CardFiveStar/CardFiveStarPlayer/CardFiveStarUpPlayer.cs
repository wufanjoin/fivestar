using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public class CardFiveStarUpPlayer : CardFiveStarPlayer
    {
        public override int ChuCardJianTouYAxleOffset//出牌箭头Y轴高度
        {
            get { return 40; }
        }
        public override int HandType
        {
            get { return CardFiveStarCardType.Up_ZhiLi_BeiMian; }
        }
        public override int ChuCardType
        {
            get { return CardFiveStarCardType.Up_DaoDi_ZhengMain; }
        }
        public override int LaingCardType
        {
            get { return CardFiveStarCardType.Up_DaoDi_ZhengMain; }
        }
        public override int PengCardType
        {
            get { return CardFiveStarCardType.Up_DaoDi_ZhengMain; }
        }
        public override int AnGangCardType
        {
            get { return CardFiveStarCardType.Up_DaoDi_BeiMian; }
        }
        //初始化碰杠集合 上和右边的玩家要特殊处理下 颠倒一下位置
        //protected override void InitPengGangs()
        //{
        //    base.InitPengGangs();
        //    ReversePengGangsPoint();
        //}


        private Vector2 GetScoreTextPoint = new Vector2(-120, -5);
        //显示玩家头像
        public override void ShowPlayerHead(User user)
        {
            base.ShowPlayerHead(user);
            _PlayerHead._GetScoreText.transform.localPosition= GetScoreTextPoint;
        }
    }
}
