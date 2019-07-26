using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
	[ObjectSystem]
	public class UiComponentAwakeSystem : AwakeSystem<UIComponent>
	{
		public override void Awake(UIComponent self)
		{
		    UIAdaption();//屏幕UI自适应
            self.Awake();
		}

        //屏幕自适应
	    public const int IdealWide = 1280;
	    public const int IdealHigh = 720;
        public void UIAdaption()
	    {
	      GameObject uiRoot=GameObject.Find("Global/UI/");
	        float idealscreenProportion = IdealWide * 1.00f / IdealHigh;
            float screenProportion=Screen.width * 1.00f / Screen.height;
	        int match = 0;
	        if (screenProportion > idealscreenProportion)
	        {
	            match = 1;

	        }
            for (int i = 0; i < uiRoot.transform.childCount; i++)
	        {
	            if (uiRoot.transform.GetChild(i).GetComponent<CanvasScaler>() == null)
	            {
	                continue;
	            }
	            uiRoot.transform.GetChild(i).GetComponent<CanvasScaler>().matchWidthOrHeight = match;

	        }
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
	public class UIComponent: Component
	{
		private GameObject Root;
		private readonly Dictionary<string, IUIFactory> UiTypes = new Dictionary<string, IUIFactory>();
		private readonly Dictionary<string, UI> uis = new Dictionary<string, UI>();

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

			this.uis.Clear();
			this.UiTypes.Clear();
		}

		public void Awake()
		{
			this.Root = GameObject.Find("Global/UI/");
			this.Load();
		}


		public void Load()
		{
			this.UiTypes.Clear();
            
			List<Type> types = Game.EventSystem.GetTypes(typeof(UIFactoryAttribute));
			
			foreach (Type type in types)
			{
				object[] attrs = type.GetCustomAttributes(typeof (UIFactoryAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}
				UIFactoryAttribute attribute = attrs[0] as UIFactoryAttribute;
				if (UiTypes.ContainsKey(attribute.Type))
				{
                    Log.Debug($"已经存在同类UI Factory: {attribute.Type}");
					throw new Exception($"已经存在同类UI Factory: {attribute.Type}");
				}
				object o = Activator.CreateInstance(type);
				IUIFactory factory = o as IUIFactory;
				if (factory == null)
				{
					Log.Error($"{o.GetType().FullName} 没有继承 IUIFactory");
					continue;
				}
				this.UiTypes.Add(attribute.Type, factory);
			}
		}

		public UI Create(string type)
		{
			try
			{
				UI ui = UiTypes[type].Create(this.GetParent<Scene>(), type, Root);
				uis.Add(type, ui);

				// 设置canvas
				string cavasName = ui.GameObject.GetComponent<CanvasConfig>().CanvasName;
				ui.GameObject.transform.SetParent(this.Root.Get<GameObject>(cavasName).transform, false);
				return ui;
			}
			catch (Exception e)
			{
				throw new Exception($"{type} UI 错误: {e}");
			}
		}

		public void Add(string type, UI ui)
		{
			this.uis.Add(type, ui);
		}

		public void Remove(string type)
		{
			UI ui;
			if (!uis.TryGetValue(type, out ui))
			{
				return;
			}
            uis.Remove(type);
			ui.Dispose();
		}

		public void RemoveAll()
		{
			foreach (string type in this.uis.Keys.ToArray())
			{
				UI ui;
				if (!this.uis.TryGetValue(type, out ui))
				{
					continue;
                }
                this.uis.Remove(type);
				ui.Dispose();
			}
		}

		public UI Get(string type)
		{
			UI ui;
			this.uis.TryGetValue(type, out ui);
			return ui;
		}

		public List<string> GetUITypeList()
		{
			return new List<string>(this.uis.Keys);
		}
	}
}