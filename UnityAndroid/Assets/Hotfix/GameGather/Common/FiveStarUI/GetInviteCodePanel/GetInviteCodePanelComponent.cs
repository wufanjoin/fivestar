using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.GetInviteCodePanel)]
    public class GetInviteCodePanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private Button mConfirmBtn;
        private InputField mInvitationCodeInputField;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mConfirmBtn = rc.Get<GameObject>("ConfirmBtn").GetComponent<Button>();
            mInvitationCodeInputField = rc.Get<GameObject>("InvitationCodeInputField").GetComponent<InputField>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mCloseBtn.Add(Hide);
            mConfirmBtn.Add(ConfirmBtnEvent);
        }

        public async void ConfirmBtnEvent()
        {
            try
            {
                int inviteCode = int.Parse(mInvitationCodeInputField.text);
                L2C_GetGreenGift  l2CGetGreenGift=(L2C_GetGreenGift)await SessionComponent.Instance.Call(new C2L_GetGreenGift(){Code = inviteCode });
                if (!string.IsNullOrEmpty(l2CGetGreenGift.Message))
                {
                    UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(l2CGetGreenGift.Message);
                }
                else
                {
                    GeneralizePanelComponent.GreenGiftStatu = 1;//状态改为已领取
                    Hide();
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public void GetGreenGiftPop(bool rersult)
        {
            Hide();
        }
    }
}
