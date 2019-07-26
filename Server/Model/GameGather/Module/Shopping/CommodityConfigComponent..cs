using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ETHotfix;
using ETModel;

namespace ETModel
{
    [ObjectSystem]
    public class ShoppingCommodityAwakeSystem : AwakeSystem<ShoppingCommodityComponent>
    {
        public override void Awake(ShoppingCommodityComponent self)
        {
            self.Awake();
        }
    }

    public class ShoppingCommodityComponent : Component
    {
        public static ShoppingCommodityComponent Ins { private set; get; }

        private DBProxyComponent dbProxyComponent;


        private List<Commodity> BeansList = new List<Commodity>();

        private List<Commodity> JewelList = new List<Commodity>();

        private Dictionary<long,Commodity> CommdityDic=new Dictionary<long, Commodity>();

        public async void Awake()
        {
            Ins = this;
            dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
            List<Commodity> commodities =await dbProxyComponent.Query<Commodity>(commodity => true);
            if (commodities.Count>0)
            {
                SetCommodityData(commodities);
            }
            else
            {
                SaveInitDefaultCommodityInfo();
            }
           
        }

        public void SetCommodityData(List<Commodity> commodities)
        {
            foreach (var commodity in commodities)
            {
                switch (commodity.CommodityType)
                {
                    case GoodsId.Besans:
                        BeansList.Add(commodity);
                        break;
                    case GoodsId.Jewel:
                        JewelList.Add(commodity);
                        break;
                }
                CommdityDic[commodity.CommodityId] = commodity;
            }
        }
        public List<Commodity> GetBeansList()
        {
            return BeansList;
        }
        public List<Commodity> GetJewelList()
        {
            return JewelList;
        }

        /// <summary>
        /// 根据商品id获取商品
        /// </summary>
        /// <param name="commodityId"></param>
        /// <returns></returns>
        public Commodity GetCommdity(long commodityId)
        {
            Commodity commodity;
            if (CommdityDic.TryGetValue(commodityId, out commodity))
            {
                
            }
            return commodity;
        }
        public async void SaveInitDefaultCommodityInfo()
        {
            Dictionary<int,int> beans=new Dictionary<int, int>();
            beans.Add(30,36000);
            beans.Add(60, 75600);
            beans.Add(100, 132000);
            beans.Add(200, 276000);
            beans.Add(500, 720000);
            beans.Add(1000, 1500000);
            beans.Add(2000, 3120000);
            int i = 0;
          
            foreach (var bean in beans)
            {
                i++;
                Commodity commodity = ComponentFactory.Create<Commodity>();
                commodity.Id = 1000 + i;
                commodity.CommodityId = 1000+i;
                commodity.CommodityType = GoodsId.Besans;
                commodity.Amount = bean.Value;
                commodity.Price = bean.Key;
                commodity.MonetaryType = GoodsId.Jewel;
                commodity.Lv = i;
                BeansList.Add(commodity);
                CommdityDic[commodity.CommodityId] = commodity;
                await dbProxyComponent.Save(commodity);
            }

            Dictionary<int, int> Jewes = new Dictionary<int, int>();
            Jewes.Add(3, 30);
            Jewes.Add(6, 72);
            Jewes.Add(18, 220);
            Jewes.Add(30, 360);
            Jewes.Add(68, 800);
            Jewes.Add(168, 2000);
            Jewes.Add(328, 4000);
            i = 0;
           
            foreach (var bean in Jewes)
            {
                i++;
                Commodity commodity = ComponentFactory.Create<Commodity>();
                commodity.Id = 2000 + i;
                commodity.CommodityId = 2000 + i;
                commodity.CommodityType = GoodsId.Jewel;
                commodity.Amount = bean.Value;
                commodity.Price = bean.Key;
                commodity.MonetaryType = GoodsId.CNY;
                commodity.Lv = i;
                JewelList.Add(commodity);
                CommdityDic[commodity.CommodityId] = commodity;
                await dbProxyComponent.Save(commodity);
            }

        }
    }
}
