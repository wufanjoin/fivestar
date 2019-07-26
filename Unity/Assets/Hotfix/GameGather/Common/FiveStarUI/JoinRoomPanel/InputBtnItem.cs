
using ETModel;
using UnityEngine.UI;
using UnityEngine;

namespace ETHotfix
{
    public class InputBtnItem : InitBaseItem
    {
        public Text mInpuText;
        public Button mInputBtn;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mInpuText = gameObject.FindChild("Text").GetComponent<Text>();
            mInputBtn = gameObject.GetComponent<Button>();
            mInputBtn.Add(InputBtnEvent);
        }

        public void InputBtnEvent()
        {
            UIComponent.GetUiView<JoinRoomPanelComponent>().InputNum(_InputNum);
        }
        private int _InputNum;
        public void InitBtn(int num)
        {
            if (num == -2)
            {
                UIComponent.GetUiView<JoinRoomPanelComponent>().mChongChuGo.transform.SetParent(gameObject.transform);
                UIComponent.GetUiView<JoinRoomPanelComponent>().mChongChuGo.transform.localPosition=Vector3.zero;
                
            }
            else if (num == -1)
            {
                UIComponent.GetUiView<JoinRoomPanelComponent>().mShuanChuGo.transform.SetParent(gameObject.transform);
                UIComponent.GetUiView<JoinRoomPanelComponent>().mShuanChuGo.transform.localPosition = Vector3.zero;
            }
            _InputNum = num;
            if (num >= 0)
            {
                mInpuText.text = num.ToString();
            }
            else
            {
                mInpuText.text = string.Empty;
            }
        }
    }
}
