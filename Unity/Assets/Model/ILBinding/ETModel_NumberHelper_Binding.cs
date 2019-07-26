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
    unsafe class ETModel_NumberHelper_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(ETModel.NumberHelper);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("ConvertorTenUnit", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ConvertorTenUnit_0);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("ConvertorTenUnit", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ConvertorTenUnit_1);


        }


        static StackObject* ConvertorTenUnit_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @l = *(long*)&ptr_of_this_method->Value;


            var result_of_this_method = ETModel.NumberHelper.ConvertorTenUnit(@l);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ConvertorTenUnit_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @i = ptr_of_this_method->Value;


            var result_of_this_method = ETModel.NumberHelper.ConvertorTenUnit(@i);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
