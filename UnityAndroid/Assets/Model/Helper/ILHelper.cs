using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Intepreter;
using UnityEngine;

namespace ETModel
{
	public static class ILHelper
	{
		public static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
		{
			// 注册重定向函数

			// 注册委托
			appdomain.DelegateManager.RegisterMethodDelegate<List<object>>();
			appdomain.DelegateManager.RegisterMethodDelegate<AChannel, System.Net.Sockets.SocketError>();
			appdomain.DelegateManager.RegisterMethodDelegate<byte[], int, int>();
			appdomain.DelegateManager.RegisterMethodDelegate<IResponse>();
		    appdomain.DelegateManager.RegisterMethodDelegate<bool>();
            appdomain.DelegateManager.RegisterMethodDelegate<Session, object>();
			appdomain.DelegateManager.RegisterMethodDelegate<Session, byte, ushort, MemoryStream>();
			appdomain.DelegateManager.RegisterMethodDelegate<Session>();
			appdomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();
			appdomain.DelegateManager.RegisterFunctionDelegate<Google.Protobuf.Adapt_IMessage.Adaptor>();
			appdomain.DelegateManager.RegisterMethodDelegate<Google.Protobuf.Adapt_IMessage.Adaptor>();
		    appdomain.DelegateManager.RegisterMethodDelegate<Action<Session>>();
		    appdomain.DelegateManager.RegisterMethodDelegate<Action<Session, byte, ushort, MemoryStream>>();

            CLRBindings.Initialize(appdomain);


		    appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
		    {
		        return new UnityEngine.Events.UnityAction(() =>
		        {
		            ((Action)act)();
		        });
		    });
		    appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.TweenCallback>((act) =>
		    {
		        return new DG.Tweening.TweenCallback(() =>
		        {
		            ((Action)act)();
		        });
		    });
		    appdomain.DelegateManager.RegisterMethodDelegate<System.String>();
		    appdomain.DelegateManager.RegisterMethodDelegate<System.Single>();
		    appdomain.DelegateManager.RegisterFunctionDelegate<Google.Protobuf.Adapt_IMessage.Adaptor, Google.Protobuf.Adapt_IMessage.Adaptor, System.Int32>();
		    appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<Google.Protobuf.Adapt_IMessage.Adaptor>>((act) =>
		    {
		        return new System.Comparison<Google.Protobuf.Adapt_IMessage.Adaptor>((x, y) =>
		        {
		            return ((Func<Google.Protobuf.Adapt_IMessage.Adaptor, Google.Protobuf.Adapt_IMessage.Adaptor, System.Int32>)act)(x, y);
		        });
		    });


            //appdomain.DelegateManager.RegisterDelegateConvertor<Action<Session>>((act) =>
            //{
            //    return new Action<Session>((s) =>
            //    {
            //        ((Action<Session>)act)(s);
            //    });
            //});
            // 注册适配器
            Assembly assembly = typeof(Init).Assembly;
			foreach (Type type in assembly.GetTypes())
			{
				object[] attrs = type.GetCustomAttributes(typeof(ILAdapterAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}
				object obj = Activator.CreateInstance(type);
				CrossBindingAdaptor adaptor = obj as CrossBindingAdaptor;
				if (adaptor == null)
				{
					continue;
				}
				appdomain.RegisterCrossBindingAdaptor(adaptor);
			}
            LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
		}
	}
}