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
    unsafe class ETModel_ResourcesComponentHelper_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(ETModel.ResourcesComponentHelper);
            args = new Type[]{typeof(ETModel.ResourcesComponent), typeof(System.String), typeof(System.String)};
            method = type.GetMethod("GetResoure", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetResoure_0);
            args = new Type[]{typeof(ETModel.ResourcesComponent), typeof(System.String)};
            method = type.GetMethod("AwayPrefixLoadBundle", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AwayPrefixLoadBundle_1);
            args = new Type[]{typeof(ETModel.ResourcesComponent), typeof(System.String), typeof(System.String)};
            method = type.GetMethod("AwayPrefixGetAsset", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AwayPrefixGetAsset_2);
            args = new Type[]{typeof(ETModel.ResourcesComponent), typeof(System.String)};
            method = type.GetMethod("AwayPrefixUnloadBundle", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AwayPrefixUnloadBundle_3);


        }


        static StackObject* GetResoure_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @resName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @uiType = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            ETModel.ResourcesComponent @resourcesComponent = (ETModel.ResourcesComponent)typeof(ETModel.ResourcesComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = ETModel.ResourcesComponentHelper.GetResoure(@resourcesComponent, @uiType, @resName);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* AwayPrefixLoadBundle_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @assetBundleName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ETModel.ResourcesComponent @resourcesComponent = (ETModel.ResourcesComponent)typeof(ETModel.ResourcesComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            ETModel.ResourcesComponentHelper.AwayPrefixLoadBundle(@resourcesComponent, @assetBundleName);

            return __ret;
        }

        static StackObject* AwayPrefixGetAsset_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @prefab = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @assetBundleName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            ETModel.ResourcesComponent @resourcesComponent = (ETModel.ResourcesComponent)typeof(ETModel.ResourcesComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = ETModel.ResourcesComponentHelper.AwayPrefixGetAsset(@resourcesComponent, @assetBundleName, @prefab);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* AwayPrefixUnloadBundle_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @assetBundleName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ETModel.ResourcesComponent @resourcesComponent = (ETModel.ResourcesComponent)typeof(ETModel.ResourcesComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            ETModel.ResourcesComponentHelper.AwayPrefixUnloadBundle(@resourcesComponent, @assetBundleName);

            return __ret;
        }



    }
}
