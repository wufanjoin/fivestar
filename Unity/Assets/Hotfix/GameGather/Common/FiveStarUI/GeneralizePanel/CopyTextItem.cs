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
  public  class CopyTextItem:InitBaseItem
  {
      public Text mCopyText;
      public Button mCopyBtn;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mCopyText=gameObject.FindChild("CopyText").GetComponent<Text>();
            mCopyBtn = gameObject.FindChild("CopyBtn").GetComponent<Button>();
        }

      private string _copyContent;
      public void InitItem(string desc,string copyStr="")
      {
          _copyContent = copyStr;

          if (string.IsNullOrEmpty(_copyContent))
          {
              _copyContent = desc;
          }
          mCopyText.text = desc;
          mCopyBtn.Add(CopyBtnEvent);
      }

      public void CopyBtnEvent()
      {
          SdkMgr.Ins.CopyText(_copyContent);
      }
    }
}
