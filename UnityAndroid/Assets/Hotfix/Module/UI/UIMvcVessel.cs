using System;
using System.Collections.Generic;
using NPOI.SS.Formula.Functions;

namespace ETHotfix
{
    //不要使用枚举热更有问题 使用Const string代替 每个UI界面都有一个工厂 组件 Mediator（中间者）
    public static class UIMvcVesselType
    {
        public const string Factory = "Factory";
        public const string Componet = "Componet";
    }
    //管理所有UI的工厂 组件 Mediator
    public class UIMvcVessel
    {
        private readonly Dictionary<string, IUIFactory> mUIFactoryDic = new Dictionary<string, IUIFactory>();
        private readonly Dictionary<string, UIView> mUIComponetDic = new Dictionary<string, UIView>();

        //清除所有容器
        public void Clear()
        {
            mUIFactoryDic.Clear();
            mUIComponetDic.Clear();
        }
        //存储一个UIType的工厂 组件 Mediator
        public void AddUIMvcVessel(string vesselType,string uiType,Type calssType)
        {
            switch (vesselType)
            {
                case UIMvcVesselType.Factory:
                    object of = Activator.CreateInstance(calssType);
                    IUIFactory factory = of as IUIFactory;
                    if (factory == null)
                    {
                        Log.Error($"{of.GetType().FullName} 没有继承 IUIFactory");
                        return;
                    }
                    AddUIType(mUIFactoryDic, uiType, factory);
                    break;
                case UIMvcVesselType.Componet:
                    object co = Activator.CreateInstance(calssType);
                    UIView componet = co as UIView;
                    if (componet == null)
                    {
                        Log.Error($"{co.GetType().FullName} 没有继承 IUIFactory");
                        return;
                    }
                    componet.GameInit();
                    AddUIType(mUIComponetDic, uiType, componet);
                    break;
            }
        }
        //存储一个UIType的工厂 组件 Mediator的泛型方法
        private void AddUIType<T>(Dictionary<string, T> uiTypeDic,string uiType,T obj)
        {
            if (uiTypeDic.ContainsKey(uiType))
            {
                Log.Error($"已经存在同类UI Type: {uiType}");
                throw new Exception($"已经存在同类UI Type: {uiType}");
            }
            uiTypeDic.Add(uiType,obj);
        }
     
        //获取一个UIType的工厂 UIView 
        public object GetUIMvcVessel(string vesselType,string uiType)
        {
            switch (vesselType)
            {
                case UIMvcVesselType.Factory:
                    return GetUIType(mUIFactoryDic, uiType,false);
                case UIMvcVesselType.Componet:
                    return GetUIType(mUIComponetDic, uiType);
            }
            throw new Exception($"无法识别vesselType: {vesselType}");
            //return null;
        }
        //获取一个UIType的工厂 组件 Mediator的泛型方法
        private T GetUIType<T>(Dictionary<string, T> uiTypeDic,string uiType,bool isReportError=true)
        {
            T obj;
            if (!uiTypeDic.TryGetValue(uiType,out obj))
            {
                if (!isReportError)
                {
                    return obj;
                }
                throw new Exception($"没有对应的uiType: {uiType}");
            }
            return obj;
        }
    }
}