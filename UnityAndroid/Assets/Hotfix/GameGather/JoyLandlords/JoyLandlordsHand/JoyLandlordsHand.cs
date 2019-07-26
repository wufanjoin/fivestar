using ETModel;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETHotfix
{
    public class JoyLandlordsHand:Entity
    {
        public int mHandIndex;
        public int mCardNum;
        public JoyLandlordsCard mJoyLandlordsCard;
        public PointerEvent mPointerEvent;
        public void Init(JoyLandlordsCard joyLandlordsCard)
        {
            mJoyLandlordsCard = joyLandlordsCard;
            mPointerEvent= mJoyLandlordsCard.gameObject.AddComponent<PointerEvent>();
            //注册事件委托
            mPointerEvent.OnPointerDownAction -= OnPointerDown;
            mPointerEvent.OnPointerUpAction -= OnPointerUp;
            mPointerEvent.OnPointerEnterAction -= OnPointerEnter;
            mPointerEvent.OnPointerDownAction += OnPointerDown;
            mPointerEvent.OnPointerUpAction += OnPointerUp;
            mPointerEvent.OnPointerEnterAction += OnPointerEnter;
            mPointerEvent.OnPointerExitAction -= OnPointerExit;
            mPointerEvent.OnPointerExitAction += OnPointerExit;
        }

        public void SetCardDataUI(int cardNum,int handIndex)
        {
            if (mCardNum != cardNum)
            {
                mCardNum = cardNum;
                mJoyLandlordsCard.SetCardDataUI(mCardNum);
            }
            mHandIndex = handIndex;
        }
        public void OnPointerExit()
        {
            JoyLandlordsHandComponent.Ins.WithdrawTheCard(mHandIndex);

        }
        public void OnPointerDown()
        {
            JoyLandlordsHandComponent.Ins.SelectFistPai(mHandIndex);
            
        }
        public void OnPointerUp()
        {
            JoyLandlordsHandComponent.Ins.ConfirmSelect();
        }
        public void OnPointerEnter()
        {
            JoyLandlordsHandComponent.Ins.DragPai(mHandIndex);
        }

        //设置预选状态并切显示蒙版
        private bool PrimaryStatu = false;
        public void SetPrimaryStatu(bool isPirmary)
        {
            PrimaryStatu = isPirmary;
            mJoyLandlordsCard.SetMengBanActive(isPirmary);
        }
        //选择完成
        public void SelectFinsh()
        {
            //如果在预选状态就 转换选中状态
            if (PrimaryStatu)
            {
                OverturnPitchStatu();
            }
            SetPrimaryStatu(false);//取消预选状态
        }
        //当前选中状态
        public bool PitchStatu
        {
            get { return _isPitch; }
        }
        //当前是否选中
        private bool _isPitch=false;
        //转换选中状态
        private void OverturnPitchStatu()
        {
            SetPitchStatu(!_isPitch);
        }
        //选中的偏移量
        private static Vector3 CardPithOffse= new Vector3(0, 25, 0);
        //设置选中状态
        public void SetPitchStatu(bool pitch)
        {
            if (pitch&&!_isPitch)
            {
                
                mJoyLandlordsCard.gameObject.transform.localPosition += CardPithOffse;
            }
            else if (_isPitch)
            {
                mJoyLandlordsCard.gameObject.transform.localPosition -= CardPithOffse;
            }
            _isPitch = pitch;
        }
        //取消选中和预选状态
        public void CancelStatu()
        {
            SetPrimaryStatu(false);
            SetPitchStatu(false);
        }
        public override void Dispose()
        {
            CancelStatu();
            UnityEngine.Object.Destroy(mPointerEvent);
            mJoyLandlordsCard.Destroy();
            base.Dispose();
        }
    }
}
