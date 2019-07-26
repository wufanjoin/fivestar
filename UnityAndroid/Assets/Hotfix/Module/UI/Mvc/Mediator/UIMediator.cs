using System.Reflection;
using ETModel;

namespace ETHotfix
{
    public class UIMediator<T,T2>:UIBaseMediator where T:UIView where T2:UIMediator<T,T2>,new()
    {
        public static T2 Ins { private set; get;}
        //View的类型
        private string uiType;
        public string pViewType
        {
            get
            {
                if (string.IsNullOrEmpty(uiType))
                {
                    
                    object[] attrs = this.GetType().GetCustomAttributes(typeof(UIMediatorAttribute), true);
                    uiType = (attrs[0] as UIMediatorAttribute).Type;
                }
                return uiType;
            }
        }

        protected string pViewState
        {
            get
            {
                if (mUIView == null)
                {
                    return ViewState.Hide;
                }
                return mUIView.pViewState;
            }
        }

        public T mUIView
        {
            get;
            private set;
        }

        private  UIComponent uiComponent;

        protected UIComponent mUIComponent
        {
            get
            {
                if (uiComponent == null)
                {
                    uiComponent = Game.Scene.GetComponent<UIComponent>();//不能在Awake里面使用此属性 因为还没有添加到Scene上
                }

                return uiComponent;
            }
        }
        
        public override void Awake()
        {
            base.Awake();
            Ins = this as T2;
        }
        public override void OnViewCreate(UIView view)
        {
            base.OnViewCreate(view);
            mUIView = view as T;
        }
        public virtual void OnDestroy()
        {
            
        }

        protected void ShowUIView()
        {
             mUIComponent.Show(pViewType);
        }

        protected void HideUIView()
        {
            mUIComponent.Hide(pViewType);
        }
    }
}