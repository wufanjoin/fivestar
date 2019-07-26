using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class UiComponentAwakeSystem : AwakeSystem<UIComponent>
    {
        public override void Awake(UIComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UiComponentLoadSystem : LoadSystem<UIComponent>
    {
        public override void Load(UIComponent self)
        {
            self.Load();
        }
    }

    /// <summary>
    /// 管理所有UI
    /// </summary>
    public class UIComponent : Component
    {
        private GameObject Root;
        private readonly UIMvcVessel uiMvcVessel = new UIMvcVessel();
        private readonly Dictionary<string, UI> uis = new Dictionary<string, UI>();
        private readonly List<UIView> uiViews = new List<UIView>();

        public static UIComponent Ins
        {
            private set;
            get;
        }

        

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            foreach (string type in uis.Keys.ToArray())
            {
                UI ui;
                if (!uis.TryGetValue(type, out ui))
                {
                    continue;
                }
                uis.Remove(type);
                ui.Dispose();
            }

            this.uiMvcVessel.Clear();
            this.uis.Clear();
        }

        public void Awake()
        {
            this.Root = GameObject.Find("Global/UI/");
            //ClerAllPanel();
            this.Load();
            Ins = this;
        }
        public void ClerAllPanel()
        {
            ClerPanel(CanvasType.HintCanvas);
            ClerPanel(CanvasType.ErrorCanvas);
            ClerPanel(CanvasType.NormalCanvas);
            ClerPanel(CanvasType.PopUpCanvas);
        }
        public void ClerPanel(string panelType)
        {
            Transform canvsTran = Root.transform.Find(panelType);
            for (int i = 0; i < canvsTran.childCount; i++)
            {
                UnityEngine.Object.Destroy(canvsTran.GetChild(i).gameObject);
            }
        }

        public void HideCanvsTypeAllView(string canvasTyps)
        {
            for (int i = 0; i < uiViews.Count; i++)
            {
                if (uiViews[i].pCavasName == canvasTyps)
                {
                    uiViews[i].Hide();
                }
            }
        }
        public void Load()
        {
            uiMvcVessel.Clear();

            List<Type> types = Game.EventSystem.GetTypes();
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(UIFactoryAttribute), true);
                if (attrs.Length == 0)
                {
                    attrs = type.GetCustomAttributes(typeof(UIComponentAttribute), true);
                    if (attrs.Length == 0)
                    {
                        continue;
                    }
                }

                Type attrType = attrs[0].GetType();
                if (typeof(UIFactoryAttribute) == attrType)
                {
                    UIFactoryAttribute factoryAttribute = attrs[0] as UIFactoryAttribute;
                    uiMvcVessel.AddUIMvcVessel(UIMvcVesselType.Factory, factoryAttribute.Type, type);
                }
                else if (typeof(UIComponentAttribute) == attrType)
                {
                    UIComponentAttribute componentAttribute = attrs[0] as UIComponentAttribute;
                    uiMvcVessel.AddUIMvcVessel(UIMvcVesselType.Componet, componentAttribute.Type, type);
                }
            }
        }

        public static T GetUiView<T>() where T:UIView
        {
            object[] attrs = typeof(T).GetCustomAttributes(typeof(UIComponentAttribute), true);
            string uiType = (attrs[0] as UIComponentAttribute).Type;
            T uiCommpoentView = Ins.uiMvcVessel.GetUIMvcVessel(UIMvcVesselType.Componet, uiType) as T;
            return uiCommpoentView;
        }

        public static UIView GetUiView(string uiType)
        {
            UIView uiCommpoentView = Ins.uiMvcVessel.GetUIMvcVessel(UIMvcVesselType.Componet, uiType) as UIView;
            return uiCommpoentView;
        }
        public UI Create(string type)
        {
            try
            {
                UI ui;
                IUIFactory uiFactory = uiMvcVessel.GetUIMvcVessel(UIMvcVesselType.Factory, type) as IUIFactory;
                if (uiFactory != null)
                {
                    ui = uiFactory.Create(this.GetParent<Scene>(), type, Root);
                }
                else
                {
                    UIView uiCommpoentView = uiMvcVessel.GetUIMvcVessel(UIMvcVesselType.Componet, type) as UIView;
                    ui = DefaultUIFactory.Create(this.GetParent<Scene>(), type, Root, uiCommpoentView);
                }
                UIView uiView = ui.GetComponent<UIView>();
                uiView.pViewState = ViewState.CreateIn;//状态改为正在创建中
                Type t = uiView.GetType();
                ui.GameObject.transform.SetParent(this.Root.Get<GameObject>(uiView.pCavasName).transform, false);
                uiView.OnCrete(ui.GameObject);
                uis.Add(type, ui);
                uiViews.Add(uiView);
                return ui;
            }
            catch (Exception e)
            {
                throw new Exception($"{type} UI 错误: {e}");
            }
        }

        public void Show(string type)
        {
            UI ui;
            if (uis.TryGetValue(type, out ui))
            {
                UIView uiView = ui.GetComponent<UIView>();
                uiView.Show();
            }
            else
            {
                Create(type);
            }
        }
        public void Hide(string type)
        {
            UI ui;
            if (uis.TryGetValue(type, out ui))
            {
                UIView uiView = ui.GetComponent<UIView>();
                uiView.Hide();
            }
            else
            {
                Log.Warning($"要隐藏的UIType还没有创建type：{type}");
            }
        }

    }
}