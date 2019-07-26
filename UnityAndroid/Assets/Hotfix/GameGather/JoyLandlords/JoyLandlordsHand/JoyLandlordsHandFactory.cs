using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
  public static class JoyLandlordsHandFactory
    {
        public static JoyLandlordsHand Create(int cardNum,int handIndex=0)
        {
            
            JoyLandlordsHand joyLandlordsHand=ComponentFactory.Create<JoyLandlordsHand>();
            joyLandlordsHand.mCardNum = cardNum;
            joyLandlordsHand.mHandIndex = handIndex;
            JoyLandlordsCard joyLandlordsCard=JoyLandlordsCardPool.Ins.Create(JoyLandlordsCardPrefabType.Large, cardNum,
                UIComponent.GetUiView<JoyLandlordsRoomPanelComponent>().mSelfHandCardGroupGo.transform);
            joyLandlordsHand.Init(joyLandlordsCard);
            return joyLandlordsHand;
        }
    }
}
