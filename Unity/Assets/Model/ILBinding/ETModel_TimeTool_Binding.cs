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
    unsafe class ETModel_TimeTool_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(ETModel.TimeTool);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("ConvertLongToTimeDesc", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ConvertLongToTimeDesc_0);
            args = new Type[]{};
            method = type.GetMethod("GetCurrenDateTime", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetCurrenDateTime_1);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("TimeStampIsToday", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, TimeStampIsToday_2);
            args = new Type[]{};
            method = type.GetMethod("GetCurrenTimeStamp", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetCurrenTimeStamp_3);


        }


        static StackObject* ConvertLongToTimeDesc_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @timeStamp = *(long*)&ptr_of_this_method->Value;


            var result_of_this_method = ETModel.TimeTool.ConvertLongToTimeDesc(@timeStamp);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetCurrenDateTime_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = ETModel.TimeTool.GetCurrenDateTime();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* TimeStampIsToday_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @timeStamp = *(long*)&ptr_of_this_method->Value;


            var result_of_this_method = ETModel.TimeTool.TimeStampIsToday(@timeStamp);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* GetCurrenTimeStamp_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = ETModel.TimeTool.GetCurrenTimeStamp();

            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }



    }
}
