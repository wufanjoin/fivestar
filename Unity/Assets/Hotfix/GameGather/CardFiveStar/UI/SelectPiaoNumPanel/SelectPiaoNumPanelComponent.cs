using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.SelectPiaoNumPanel)]
    public class SelectPiaoNumPanelComponent : ChildUIView
    {
        #region 脚本工具生成的代码
        private Button mNoPiaoBtn;
        private Button mPiaoOneBtn;
        private GameObject mAwaitHintGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mNoPiaoBtn = rc.Get<GameObject>("NoPiaoBtn").GetComponent<Button>();
            mPiaoOneBtn = rc.Get<GameObject>("PiaoOneBtn").GetComponent<Button>();
            mAwaitHintGo = rc.Get<GameObject>("AwaitHintGo");
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mPiaoOneBtn.gameObject.SetActive(false);
            mNoPiaoBtn.Add(NoPiaoBtnEvent);
        }

        public void NoPiaoBtnEvent()
        {
            SessionComponent.Instance.Send(new Actor_FiveStar_DaPiaoResult() { SelectPiaoNum = 0 });
        }
        private void ShowCanSelctPiaoNum(int maxPiaoNum)
        {
            for (int i = _SelectPiaoNumBtns.Count; i < maxPiaoNum; i++)
            {
                GameObject goBtn=GameObject.Instantiate(mPiaoOneBtn.gameObject, mPiaoOneBtn.transform.parent);
                SelectBtnClickEvent selectBtnClick=new SelectBtnClickEvent(i+1);
                Button  button=goBtn.GetComponent<Button>();
                button.SetText("漂"+ NumberConvertorHanziTool.GetHanzi(i+1));
                button.Add(selectBtnClick.Click);
                _SelectPiaoNumBtns.Add(goBtn);
            }
            for (int i = 0; i < maxPiaoNum; i++)
            {
                _SelectPiaoNumBtns[i].SetActive(true);
            }
            for (int i = maxPiaoNum; i < _SelectPiaoNumBtns.Count; i++)
            {
                _SelectPiaoNumBtns[i].SetActive(false);
            }
        }

        public void HideAllBtn()
        {
            mNoPiaoBtn.gameObject.SetActive(false);
            foreach (var btngo in _SelectPiaoNumBtns)
            {
                btngo.SetActive(false);
            }
        }
        List<GameObject> _SelectPiaoNumBtns=new List<GameObject>();
        public void ShowCanSelectPiaoNum(int maxPiaoNum)
        {
            Show();
            mAwaitHintGo.SetActive(false);
            mNoPiaoBtn.gameObject.SetActive(true);
            ShowCanSelctPiaoNum(maxPiaoNum);
        }

        public void ShowAwaitHint()
        {
            Show();
            HideAllBtn();
            mAwaitHintGo.SetActive(true);
        }
    }

    public class SelectBtnClickEvent
    {
        private int _PiaoNum;
        public SelectBtnClickEvent(int piaoNum)
        {
            _PiaoNum = piaoNum;
        }

        public void Click()
        {
            SessionComponent.Instance.Send(new Actor_FiveStar_DaPiaoResult(){SelectPiaoNum = _PiaoNum });
        }
    }
}
