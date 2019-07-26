
//using System;
//using System.Reflection;
//using ETModel;
//using LitJson;
//using NPOI.SS.Formula.Functions;

//namespace ETModel
//{
//    public static class CloneObject
//    {


//        /// <summary>
//        /// 克隆一个对象
//        /// </summary>
//        /// <param name="sampleObject"></param>
//        /// <returns></returns>
//        public static object Clone(object sampleObject)
//        {
//            Type T = typeof(object);
//            //在ILRuntime模式下 无法是引用 用对象转Json在转成对象就完成了复制
//            return  JsonMapper.ToObject<T>(JsonMapper.ToJson(sampleObject));
//            return null;
//            Type t = sampleObject.GetType();
//            PropertyInfo[] properties = t.GetProperties();
//            object p = t.InvokeMember("", BindingFlags.CreateInstance, null, sampleObject, null);
//            foreach (PropertyInfo pi in properties)
//            {
//                if (pi.CanWrite)
//                {
//                    object value = pi.GetValue(sampleObject, null);
//                    pi.SetValue(p, value, null);
//                }
//            }
//            return p;
//        }
//    }
//}

