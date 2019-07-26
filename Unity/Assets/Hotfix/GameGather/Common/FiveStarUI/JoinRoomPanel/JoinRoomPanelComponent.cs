using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class ShowInputNumberType
    {
        public const int RoomNumber = 1;//加入房间
        public const int FriendCicleId = 2;//加入亲友圈
    }
    [UIComponent(UIType.JoinRoomPanel)]
    public class JoinRoomPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private GameObject mInputBtnItemGo;
        private GameObject mFriendCicleInputGo;
        private GameObject mRoomNumberInputGo;
        private Text mRoomNumberText;
        private Text mFriendCicleInputText;
        private Text mTitleText;
        public GameObject mShuanChuGo;
        public GameObject mChongChuGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mInputBtnItemGo = rc.Get<GameObject>("InputBtnItemGo");
            mFriendCicleInputGo = rc.Get<GameObject>("FriendCicleInputGo");
            mRoomNumberInputGo = rc.Get<GameObject>("RoomNumberInputGo");
            mRoomNumberText = rc.Get<GameObject>("RoomNumberText").GetComponent<Text>();
            mFriendCicleInputText= rc.Get<GameObject>("FriendCicleInputText").GetComponent<Text>();
            mTitleText = rc.Get<GameObject>("TitleText").GetComponent<Text>();
            mShuanChuGo = rc.Get<GameObject>("ShuanChuGo");
            mChongChuGo = rc.Get<GameObject>("ChongChuGo");
            InitPanel();
        }
        #endregion
        public List<int> _NumberLists=new List<int>(){1,2,3,4,5,6,7,8,9,-2,0,-1};
        public void InitPanel()
        {
            InitInputList();
        }

        public void InitInputList()
        {
            Transform inputBtnItemParent = mInputBtnItemGo.transform.parent;
            inputBtnItemParent.CreatorChildCount(_NumberLists.Count);
            for (int i = 0; i < _NumberLists.Count; i++)
            {
                inputBtnItemParent.GetChild(i).gameObject.AddItem<InputBtnItem>().InitBtn(_NumberLists[i]);
            }
        }

        public int _CurrInputType;
        Text _CurrInputText;
        int _CurrInputLength;
        public Action<int> _InoutFinshAction;
        public void ShowInputPanel(int type,Action<int> inputFinshAction)
        {
            Show();
            _InoutFinshAction = inputFinshAction;
            _CurrInputType = type;
            mFriendCicleInputText.gameObject.SetActive(type == ShowInputNumberType.FriendCicleId);
            mFriendCicleInputGo.SetActive(type == ShowInputNumberType.FriendCicleId);
            mRoomNumberText.gameObject.SetActive(type == ShowInputNumberType.RoomNumber);
            mRoomNumberInputGo.SetActive(type == ShowInputNumberType.RoomNumber);
            if (type == ShowInputNumberType.RoomNumber)
            {
                mTitleText.text = "请输入房间号";
                _CurrInputText = mRoomNumberText;
                _CurrInputLength = RoomNumberLength;
            }
            else if (type == ShowInputNumberType.FriendCicleId)
            {
                mTitleText.text = "请输入亲友圈ID";
                _CurrInputText = mFriendCicleInputText;
                _CurrInputLength = FriendCicleLength;
            }
            _CurrInputText.text = "";
        }

        public const int RoomNumberLength = 5;
        public const int FriendCicleLength = 6;
        public void InputNum(int num)
        {
            if (num == -1)
            {
                if (_CurrInputText.text.Length > 0)
                {
                    _CurrInputText.text = _CurrInputText.text.Substring(0, _CurrInputText.text.Length - 1);
                }
            }
            else if (num == -2)
            {
                _CurrInputText.text = "";
            }
            else
            {
                _CurrInputText.text += num.ToString();
                if (_CurrInputText.text.Length >= _CurrInputLength)
                {
                    InoutFinshActionCall();
                }
            }
        }

        public  void InoutFinshActionCall()
        {
            try
            {
                _InoutFinshAction?.Invoke(int.Parse(_CurrInputText.text));
                _CurrInputText.text = "";
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
}
