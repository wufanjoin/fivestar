using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace ETModel
{
   // [ILAdapter]
   public class IEnumeratorClassInheritanceAdaptor : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(IEnumerator);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(IEnumeratorAdaptor);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new IEnumeratorAdaptor(appdomain, instance);
        }


        public class IEnumeratorAdaptor : IEnumerator, CrossBindingAdaptorType
        {
            private ILTypeInstance instance;
            private ILRuntime.Runtime.Enviorment.AppDomain appDomain;


            public IEnumeratorAdaptor()
            {
            }

            public IEnumeratorAdaptor(ILRuntime.Runtime.Enviorment.AppDomain appDomain, ILTypeInstance instance)
            {
                this.appDomain = appDomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance
            {
                get
                {
                    return instance;
                }
            }

            private IMethod mMoveNext;
            private IMethod mReset;
            private readonly object[] param1 = new object[1];
            public bool MoveNext()
            {
                if (this.mMoveNext == null)
                {
                    mMoveNext = instance.Type.GetMethod("MoveNext", 0);
                }
               return (bool)this.appDomain.Invoke(mMoveNext, instance, null);
            }

            public  void Reset()
            {
                if (this.mReset == null)
                {
                    mReset = instance.Type.GetMethod("Reset", 0);
                }
                this.appDomain.Invoke(mMoveNext, instance, null);
            }

            public object Current
            {
                get { return instance; }
            }
        }
    }
}
