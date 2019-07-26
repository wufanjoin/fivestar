using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class QueryPlaybackView : BaseView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private Button mConfirmBtn;
        private InputField mPlaybackCodeInputField;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mConfirmBtn = rc.Get<GameObject>("ConfirmBtn").GetComponent<Button>();
            mPlaybackCodeInputField = rc.Get<GameObject>("PlaybackCodeInputField").GetComponent<InputField>();
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
                int recordId=int.Parse(mPlaybackCodeInputField.text);
                MilitaryPanelComponent.ExaminePlayBack(recordId);
            }
            catch (Exception e)
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("回放码格式不对");
                Log.Error(e);
                throw;
            }
            
        }


    }
}
