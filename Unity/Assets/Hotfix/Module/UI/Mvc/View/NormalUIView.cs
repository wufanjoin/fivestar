using System.Collections.Generic;
using System.Linq;

namespace ETHotfix
{
    public class NormalUIView:UIView
    {
        private List<string> ChildViews;
        public override string pCavasName
        {
            get
            {
                return CanvasType.NormalCanvas;
            }
        }

        public override void Awake()
        {
            base.Awake();
            EventMsgMgr.RegisterEvent(CommEventID.OnOtherNormalViewShow, OnOtherNormalViewShow);
        }
        //初始化Normal层的子视图
        public void InitChildView(params string[] views)
        {
            ChildViews = views.ToList();
        }
        //当其他Normal层视图显示的时候 其他视图会自动隐藏
        public void OnOtherNormalViewShow(params object[] objs)
        {
            if (this.pViewState == ViewState.Show&&(string)objs[0]!= this.pViewType)
            {
                this.Hide();
            }
        }
        
        //隐藏的时候 同时隐藏所有子视图
        public override void OnHideBefore()
        {
            base.OnHideBefore();
            mUIComponent.HideCanvsTypeAllView(CanvasType.ChildCanvas);//隐藏所有孩子视图
        }
        //显示的时候发送自己显示的事件
        public override void OnShowBefore()
        {
            base.OnShowBefore();
            EventMsgMgr.SendEvent(CommEventID.OnOtherNormalViewShow, this.pViewType);
        }

        //显示的时候同时显示自己的子视图
        public override void OnShow()
        {
            base.OnShow();
            if (ChildViews != null)
            {
                foreach (var viewType in ChildViews)
                {
                    mUIComponent.Show(viewType);
                    ChildUIView childUiView = UIComponent.GetUiView(viewType) as ChildUIView;
                    childUiView.SetParentUIType(pViewType);
                }
            }
        }
    }
}