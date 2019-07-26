using System.Reflection;
using UnityEngine;
using ETModel;

namespace ETHotfix
{
    public class ViewState
    {
        public const string Node = "Node";
        public const string CreateIn = "CreateIn";
        public const string Hide = "Hide";
        public const string Show = "Show";
    }
    public class CanvasType
    {
        public const string NormalCanvas = "NormalCanvas";
        public const string ChildCanvas = "ChildCanvas";
        public const string PopUpCanvas = "PopUpCanvas";
        public const string HintCanvas = "HintCanvas";
        public const string ErrorCanvas = "ErrorCanvas";
    }
    public abstract class UIView:Component
    {
        //View的当前的状态
        public string pViewState = ViewState.Node;

        public UIComponent mUIComponent
        {
            get
            {
               return Game.Scene.GetComponent<UIComponent>();
            }
        }
        //所在Canvas的名字
        public virtual string pCavasName
        {
            get
            {
                return CanvasType.NormalCanvas;
            }
        }

        //View的类型
        private string uiType;
        public string pViewType
        {
            get
            {
                if (string.IsNullOrEmpty(uiType))
                {
                    object[] attrs = this.GetType().GetCustomAttributes(typeof(UIComponentAttribute), true);
                    uiType = (attrs[0] as UIComponentAttribute).Type;
                }
                return uiType;
            }
        }
        public GameObject gameObject;

        public virtual void GameInit()
        {
            
        }
        public virtual void OnCrete(GameObject go)
        {
            gameObject = go;
            Awake();
            Show();
        }
        public virtual void Awake()
        {
            
        }
        public virtual void OnHideBefore()
        {
            
        }
        public virtual void OnHide()
        {
            
        }
        public virtual void OnShowBefore()
        {
           
        }
        //显示的时候把自己设为层的最后一个
        public virtual async void OnShow()
        {
            gameObject.transform.SetAsLastSibling();
        }
   
        
        public virtual void OnDestroy()
        {
            
        }

        public  void Show()
        {
            if (ViewState.Show == pViewState)
            {
                return;
            }
            if (ViewState.Node == pViewState)
            {
                mUIComponent.Create(pViewType);
                return;
            }
            OnShowBefore();
            gameObject.SetActive(true);
            pViewState = ViewState.Show;
            OnShow();
        }
          
        public void Hide()
        {
            if (ViewState.Hide == pViewState)
            {
                return;
            }
            if (ViewState.Node == pViewState)
            {
                return;
            }
            OnHideBefore();
            gameObject.SetActive(false);
            pViewState = ViewState.Hide;
            OnHide();
        }
        public void Destroy()
        {
            OnDestroy();
            UnityEngine.Object.Destroy(gameObject);
        }

        public T GetResoure<T>(string resName) where T: UnityEngine.Object
        {
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            return resourcesComponent.GetResoure(pViewType, resName) as T;
        }
    }
}