using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class OperationSpecialMgr : Single<OperationSpecialMgr>
    {
        private List<Image> _SpecialPool = new List<Image>();//简单对象池
        private GameObject OperationSpecialImagePrefab;//特效Image预制体

        //得到一个可以用的Image对象
        private Image GetSpcialImage(Transform parenTransform)
        {
            if (_SpecialPool.Count > 0)
            {
                Image image = _SpecialPool[0];
                _SpecialPool.RemoveAt(0);
                image.transform.SetParent(parenTransform);
                image.transform.localPosition = Vector3.zero;
                return image;
            }

            if (OperationSpecialImagePrefab == null)
            {
                OperationSpecialImagePrefab = ResourcesComponent.Ins.GetResoure(UIType.CardFiveStarRoomPanel, "OperationSpecialImage") as GameObject;
                InitSpecialSprite();
            }
            GameObject spcialGo = GameObject.Instantiate(OperationSpecialImagePrefab, parenTransform);
            spcialGo.transform.localPosition = Vector3.zero;
            return spcialGo.GetComponent<Image>();
        }

        //初始化特效图片
        public const string PengSpecialRes = "peng_";
        public const string GengSpecialRes = "gang_";
        public const string HuSpecialRes = "hu_";
        public const string LiangSpecialRes = "liang_";
        private Dictionary<int, List<Sprite>> OperationTypeInSpecial;
        public void InitSpecialSprite()
        {
            OperationTypeInSpecial = new Dictionary<int, List<Sprite>>();
        
            List<Sprite> pengSprites=new List<Sprite>();
            for (int i = 1; i < 4; i++)
            {
                pengSprites.Add(ResourcesComponent.Ins.GetResoure(UIType.CardFiveStarRoomPanel, PengSpecialRes + i) as Sprite); 
            }

            List<Sprite> gangSprites = new List<Sprite>();
            for (int i = 1; i < 4; i++)
            {
                gangSprites.Add(ResourcesComponent.Ins.GetResoure(UIType.CardFiveStarRoomPanel, GengSpecialRes + i) as Sprite);
            }

            List<Sprite> huSprites = new List<Sprite>();
            for (int i = 1; i < 4; i++)
            {
                huSprites.Add(ResourcesComponent.Ins.GetResoure(UIType.CardFiveStarRoomPanel, HuSpecialRes + i) as Sprite);
            }

            List<Sprite> liangSprites = new List<Sprite>();
            for (int i = 1; i < 4; i++)
            {
                liangSprites.Add(ResourcesComponent.Ins.GetResoure(UIType.CardFiveStarRoomPanel, LiangSpecialRes + i) as Sprite);
            }
            OperationTypeInSpecial.Add(FiveStarOperateType.Peng, pengSprites);
            OperationTypeInSpecial.Add(FiveStarOperateType.MingGang, gangSprites);
            OperationTypeInSpecial.Add(FiveStarOperateType.AnGang, gangSprites);
            OperationTypeInSpecial.Add(FiveStarOperateType.CaGang, gangSprites);
            OperationTypeInSpecial.Add(FiveStarOperateType.FangChongHu, huSprites);
            OperationTypeInSpecial.Add(FiveStarOperateType.ZiMo, huSprites);
            OperationTypeInSpecial.Add(FiveStarOperateType.Liang, liangSprites);
        }

        public const int SpriteIntervalmillisecond = 150;//每张图片切换间隔的毫秒
        public const int VanishIntervalmillisecond = 100;//渐隐 间隔多少减0.1f
        public async void ShowSpecial(int operationType, Transform parenTransform)
        {
            if (operationType == FiveStarOperateType.None)
            {
                return;//玩家选择过 就无视
            }
            Image spcialImage = GetSpcialImage(parenTransform);

            List<Sprite> specialSpriteState;
            if (!OperationTypeInSpecial.TryGetValue(operationType,out specialSpriteState))
            {
                Log.Error("显示特效 操作类型没有"+ operationType);
                return;
            }
            
            spcialImage.gameObject.SetActive(true);
            spcialImage.color = VectorHelper.GetSameColor(1);
            for (int i = 0; i < specialSpriteState.Count; i++)
            {
                spcialImage.sprite = specialSpriteState[i];
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(SpriteIntervalmillisecond);
            }
            for (float i = 1; i >=0; i-=0.1f)
            {
                spcialImage.color = VectorHelper.GetLucencyWhiteColor(i);
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(VanishIntervalmillisecond);
            }
            spcialImage.gameObject.SetActive(false);
            _SpecialPool.Add(spcialImage);//显示完毕放回对象池
        }
    }
}
