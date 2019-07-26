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
    unsafe class ETModel_GameVersionsConfig_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.GameVersionsConfig);

            field = type.GetField("Version", flag);
            app.RegisterCLRFieldGetter(field, get_Version_0);
            app.RegisterCLRFieldSetter(field, set_Version_0);


        }



        static object get_Version_0(ref object o)
        {
            return ((ETModel.GameVersionsConfig)o).Version;
        }
        static void set_Version_0(ref object o, object v)
        {
            ((ETModel.GameVersionsConfig)o).Version = (System.Double)v;
        }


    }
}
