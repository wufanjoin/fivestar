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
    unsafe class ETModel_ButtonAnimationEffect_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.ButtonAnimationEffect);

            field = type.GetField("ButtonPointerUpAction", flag);
            app.RegisterCLRFieldGetter(field, get_ButtonPointerUpAction_0);
            app.RegisterCLRFieldSetter(field, set_ButtonPointerUpAction_0);


        }



        static object get_ButtonPointerUpAction_0(ref object o)
        {
            return ETModel.ButtonAnimationEffect.ButtonPointerUpAction;
        }
        static void set_ButtonPointerUpAction_0(ref object o, object v)
        {
            ETModel.ButtonAnimationEffect.ButtonPointerUpAction = (System.Action)v;
        }


    }
}
