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
    unsafe class ViChatPacket_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::ViChatPacket);

            field = type.GetField("Data", flag);
            app.RegisterCLRFieldGetter(field, get_Data_0);
            app.RegisterCLRFieldSetter(field, set_Data_0);
            field = type.GetField("Length", flag);
            app.RegisterCLRFieldGetter(field, get_Length_1);
            app.RegisterCLRFieldSetter(field, set_Length_1);
            field = type.GetField("DataLength", flag);
            app.RegisterCLRFieldGetter(field, get_DataLength_2);
            app.RegisterCLRFieldSetter(field, set_DataLength_2);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_Data_0(ref object o)
        {
            return ((global::ViChatPacket)o).Data;
        }
        static void set_Data_0(ref object o, object v)
        {
            ((global::ViChatPacket)o).Data = (System.Byte[])v;
        }
        static object get_Length_1(ref object o)
        {
            return ((global::ViChatPacket)o).Length;
        }
        static void set_Length_1(ref object o, object v)
        {
            ((global::ViChatPacket)o).Length = (System.Int32)v;
        }
        static object get_DataLength_2(ref object o)
        {
            return ((global::ViChatPacket)o).DataLength;
        }
        static void set_DataLength_2(ref object o, object v)
        {
            ((global::ViChatPacket)o).DataLength = (System.Int32)v;
        }

        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new global::ViChatPacket();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
