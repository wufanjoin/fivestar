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
    public class CardFiveStarCard : Entity
    {
        public int CardType= CardFiveStarCardType.None;//牌的类型
        public int CardSize = 0;//牌的大小
        public GameObject gameObject;//牌对应的游戏对象
        private Image _iconImage;//正面亮出来的牌 才有

        public virtual void Init(GameObject go)
        {
            gameObject = go;
            if (gameObject.transform.Find("Card_icon")!=null)
            {
                _iconImage = gameObject.transform.Find("Card_icon").GetComponent<Image>();
            }
        }

        public virtual void SetCardUI(int size)
        {
            isDestroy = false;//每次从 对象池拿出都会 调用这个方法
            CardSize = size;
            if (_iconImage != null)
            {
                _iconImage.sprite=ResourcesComponent.Ins.GetResoure(UIType.CardFiveStarRoomPanel, "big_" + CardSize) as Sprite;
                _iconImage.SetNativeSize();
            }
            SetActive(true);
        }

        //设置父节点
        public void SetParent(Transform parent)
        {
            gameObject.transform.SetParent(parent);
        }

        //本地位置归0
        public void LocalPositionZero()
        {
            gameObject.transform.localPosition = Vector3.zero;
        }
        //隐藏或者显示这个牌
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
        //隐藏或者显示这个牌的Image组件 
        public void SetImageActive(bool active)
        {
            Image[] images=gameObject.transform.GetComponentsInChildren<Image>();
            for (int i = 0; i < images.Length; i++)
            {
                images[i].enabled = active;
            }
        }
        //设置大小
        public void SetScale(float scale)
        {
            gameObject.transform.localScale = VectorHelper.GetSameVector2(scale);
        }

        public bool isDestroy { private set; get; }
        //销毁这个牌
        public virtual void Destroy()
        {
            if (isDestroy)
            {
                return;
            }
            CardSize = 0;//牌的大小置为0
            CardFiveStarCardPool.Ins.DestroyJoyLandlordsCard(this);
            gameObject.SetActive(false);
            isDestroy = true;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
