using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class SdkCall_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::SdkCall);
            args = new Type[]{};
            method = type.GetMethod("get_Ins", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Ins_0);

            field = type.GetField("LocationAction", flag);
            app.RegisterCLRFieldGetter(field, get_LocationAction_0);
            app.RegisterCLRFieldSetter(field, set_LocationAction_0);
            field = type.GetField("UrlOpenAppAction", flag);
            app.RegisterCLRFieldGetter(field, get_UrlOpenAppAction_1);
            app.RegisterCLRFieldSetter(field, set_UrlOpenAppAction_1);
            field = type.GetField("OpenAppUrl", flag);
            app.RegisterCLRFieldGetter(field, get_OpenAppUrl_2);
            app.RegisterCLRFieldSetter(field, set_OpenAppUrl_2);
            field = type.GetField("WeChatLoginAction", flag);
            app.RegisterCLRFieldGetter(field, get_WeChatLoginAction_3);
            app.RegisterCLRFieldSetter(field, set_WeChatLoginAction_3);


        }


        static StackObject* get_Ins_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::SdkCall.Ins;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_LocationAction_0(ref object o)
        {
            return ((global::SdkCall)o).LocationAction;
        }
        static void set_LocationAction_0(ref object o, object v)
        {
            ((global::SdkCall)o).LocationAction = (System.Action<System.String>)v;
        }
        static object get_UrlOpenAppAction_1(ref object o)
        {
            return ((global::SdkCall)o).UrlOpenAppAction;
        }
        static void set_UrlOpenAppAction_1(ref object o, object v)
        {
            ((global::SdkCall)o).UrlOpenAppAction = (System.Action<System.String>)v;
        }
        static object get_OpenAppUrl_2(ref object o)
        {
            return global::SdkCall.OpenAppUrl;
        }
        static void set_OpenAppUrl_2(ref object o, object v)
        {
            global::SdkCall.OpenAppUrl = (System.String)v;
        }
        static object get_WeChatLoginAction_3(ref object o)
        {
            return ((global::SdkCall)o).WeChatLoginAction;
        }
        static void set_WeChatLoginAction_3(ref object o, object v)
        {
            ((global::SdkCall)o).WeChatLoginAction = (System.Action<System.String>)v;
        }


    }
}
