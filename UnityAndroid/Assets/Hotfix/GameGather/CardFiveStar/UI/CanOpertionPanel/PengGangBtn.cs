using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETHotfix;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class PengGangBtn
    {
        public GameObject gameObject;
        public int _CardSize = 0;
        public int _OperatuionType = 0;
        public CardFiveStarCard _CardFiveStarCard;
        public Button _PengGangBtn;
        public Image _PengGangImage;

        public PengGangBtn(GameObject go)
        {
            gameObject = go;
            gameObject.transform.SetAsFirstSibling();
            _PengGangBtn = go.FindChild("PengGangBtn").GetComponent<Button>();
            _PengGangImage = go.FindChild("PengGangBtn").GetComponent<Image>();
            _CardFiveStarCard = CardFiveStarCardPool.Ins.Create(CardFiveStarCardType.Down_ZhiLi_ZhengMain,
                go.FindChild("CardPointGo"), 0.7f);
            _CardFiveStarCard.LocalPositionZero();
            _PengGangBtn.Add(Cilck);
        }

        //玩家点击操作
        public void Cilck()
        {
            CanOpertionPanelComponent.SendOperationMessage(_OperatuionType, _CardSize);
        }
        //设置UI
        public void SetUI(int operatuionType, int cardSize)
        {
            gameObject.SetActive(true);
            _CardSize = cardSize;
            _OperatuionType = operatuionType;
            _CardFiveStarCard.SetCardUI(_CardSize);
            if (operatuionType == FiveStarOperateType.Peng)
            {
                _PengGangImage.sprite = ResourcesComponent.Ins.GetResoure(UIType.CardFiveStarRoomPanel, "peng") as Sprite;
            }
            else if (operatuionType == FiveStarOperateType.MingGang)
            {
                _PengGangImage.sprite = ResourcesComponent.Ins.GetResoure(UIType.CardFiveStarRoomPanel, "gang") as Sprite;
            }
        }
        //隐藏
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
