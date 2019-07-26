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
    unsafe class ETModel_PointerEvent_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.PointerEvent);

            field = type.GetField("OnPointerDownAction", flag);
            app.RegisterCLRFieldGetter(field, get_OnPointerDownAction_0);
            app.RegisterCLRFieldSetter(field, set_OnPointerDownAction_0);
            field = type.GetField("OnPointerUpAction", flag);
            app.RegisterCLRFieldGetter(field, get_OnPointerUpAction_1);
            app.RegisterCLRFieldSetter(field, set_OnPointerUpAction_1);
            field = type.GetField("OnPointerEnterAction", flag);
            app.RegisterCLRFieldGetter(field, get_OnPointerEnterAction_2);
            app.RegisterCLRFieldSetter(field, set_OnPointerEnterAction_2);
            field = type.GetField("OnPointerExitAction", flag);
            app.RegisterCLRFieldGetter(field, get_OnPointerExitAction_3);
            app.RegisterCLRFieldSetter(field, set_OnPointerExitAction_3);


        }



        static object get_OnPointerDownAction_0(ref object o)
        {
            return ((ETModel.PointerEvent)o).OnPointerDownAction;
        }
        static void set_OnPointerDownAction_0(ref object o, object v)
        {
            ((ETModel.PointerEvent)o).OnPointerDownAction = (System.Action)v;
        }
        static object get_OnPointerUpAction_1(ref object o)
        {
            return ((ETModel.PointerEvent)o).OnPointerUpAction;
        }
        static void set_OnPointerUpAction_1(ref object o, object v)
        {
            ((ETModel.PointerEvent)o).OnPointerUpAction = (System.Action)v;
        }
        static object get_OnPointerEnterAction_2(ref object o)
        {
            return ((ETModel.PointerEvent)o).OnPointerEnterAction;
        }
        static void set_OnPointerEnterAction_2(ref object o, object v)
        {
            ((ETModel.PointerEvent)o).OnPointerEnterAction = (System.Action)v;
        }
        static object get_OnPointerExitAction_3(ref object o)
        {
            return ((ETModel.PointerEvent)o).OnPointerExitAction;
        }
        static void set_OnPointerExitAction_3(ref object o, object v)
        {
            ((ETModel.PointerEvent)o).OnPointerExitAction = (System.Action)v;
        }


    }
}
