using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETHotfix;
using Google.Protobuf.Collections;


namespace ETModel
{
    [ObjectSystem]
    public class TurntableComponentAwakeSystem : AwakeSystem<TurntableComponent>
    {
        public override void Awake(TurntableComponent self)
        {
            self.Awake();
        }
    }
    public class TurntableComponent : Component
    {

        public DBProxyComponent dbProxyComponent;
        public static TurntableComponent Ins;
        public List<TurntableGoods> mTurntableGoodses;
        public RepeatedField<TurntableGoods> mTurntableGoodsesRepeatedField=new RepeatedField<TurntableGoods>();
        public int MaxWinPrizeId=0;//当前最大中奖记录Id
        public async void Awake()
        {
            Ins = this;
            dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
            mTurntableGoodses = await dbProxyComponent.Query<TurntableGoods>(signInAward => true);
            if (mTurntableGoodses.Count<=0)
            {
                InitDefaultTurntableGoodsConfigs();
            }
            mTurntableGoodsesRepeatedField.Add(mTurntableGoodses.ToArray());
            List< WinPrizeRecord > winPrizeRecords =await dbProxyComponent.SortQuery<WinPrizeRecord>(prize => true, prize => prize.WinPrizeId == -1, -1);
            if (winPrizeRecords.Count > 0)
            {
                MaxWinPrizeId = winPrizeRecords[0].WinPrizeId;
            }
            else
            {
                MaxWinPrizeId = 1000;
            }
        }
        //获取中奖记录Id
        public int GetWinPrizeId()
        {
            return ++MaxWinPrizeId;
        }
        public async void InitDefaultTurntableGoodsConfigs()
        {
            for (int i = 0; i < 8; i++)
            {
                TurntableGoods turntableGoods = ComponentFactory.Create<TurntableGoods>();
                turntableGoods.TurntableGoodsId = i+1;
                switch (i)
                {
                    case 0:
                        turntableGoods.GoodsId = GoodsId.Besans;
                        turntableGoods.Amount = 20000;
                        turntableGoods.Probability = 30;
                        break;
                    case 1:
                        turntableGoods.GoodsId = GoodsId.Jewel;
                        turntableGoods.Amount = 20;
                        turntableGoods.Probability = 8;
                        break;
                    case 2:
                        turntableGoods.GoodsId = GoodsId.HongBao;
                        turntableGoods.Amount = 2;
                        turntableGoods.Probability = 0;
                        break;
                    case 3:
                        turntableGoods.GoodsId = GoodsId.None;
                        turntableGoods.Amount = 0;
                        turntableGoods.Probability = 30;
                        break;
                    case 4:
                        turntableGoods.GoodsId = GoodsId.Jewel;
                        turntableGoods.Amount = 5;
                        turntableGoods.Probability = 30;
                        break;
                    case 5:
                        turntableGoods.GoodsId = GoodsId.HongBao;
                        turntableGoods.Amount = 5;
                        turntableGoods.Probability = 0;
                        break;
                    case 6:
                        turntableGoods.GoodsId = GoodsId.Jewel;
                        turntableGoods.Amount = 50;
                        turntableGoods.Probability = 2;
                        break;
                    case 7:
                        turntableGoods.GoodsId = GoodsId.HongBao;
                        turntableGoods.Amount = 10;
                        turntableGoods.Probability = 0;
                        break;
                }
                mTurntableGoodses.Add(turntableGoods);
                await dbProxyComponent.Save(turntableGoods);
            }
            
        }
    }
}
