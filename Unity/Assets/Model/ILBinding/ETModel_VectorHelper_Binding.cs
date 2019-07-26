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
    unsafe class ETModel_VectorHelper_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(ETModel.VectorHelper);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("GetSameVector2", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetSameVector2_0);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("GetSameVector3", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetSameVector3_1);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("GetSameColor", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetSameColor_2);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("GetLucencyWhiteColor", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetLucencyWhiteColor_3);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("GetLucencyBlackColor", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetLucencyBlackColor_4);


        }


        static StackObject* GetSameVector2_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @size = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = ETModel.VectorHelper.GetSameVector2(@size);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetSameVector3_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @size = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = ETModel.VectorHelper.GetSameVector3(@size);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetSameColor_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @value = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = ETModel.VectorHelper.GetSameColor(@value);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetLucencyWhiteColor_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @value = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = ETModel.VectorHelper.GetLucencyWhiteColor(@value);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetLucencyBlackColor_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @value = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = ETModel.VectorHelper.GetLucencyBlackColor(@value);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
