using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETHotfix
{
    public class CardFiveStarDownDaoDiZhengMainCard : CardFiveStarCard
    {
        public GameObject _KouIconGo;//正面亮出来的牌 才有

        public override void Init(GameObject go)
        {
            base.Init(go);
            _KouIconGo = gameObject.transform.Find("Kou_icon").gameObject;
        }

        public override void SetCardUI(int size)
        {
            base.SetCardUI(size);
            _KouIconGo.SetActive(false);
        }

        public void ShowKouIcon()
        {
            _KouIconGo.SetActive(true);
        }
    }
}
