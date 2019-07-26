using System;
using System.Collections;
using System.Collections.Generic;
using ETModel;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace ETModel
{


    //自己傻逼了 因为资源是在mono下打包 然后在ILRuntime模式下面运行报错
    //[ILAdapter]
    public class AEventAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get { return null; }
        }

        public override Type[] BaseCLRTypes
        {
            get
            {
                //跨域继承只能有1个Adapter，因此应该尽量避免一个类同时实现多个外部接口，对于coroutine来说是IEnumerator<object>,IEnumerator和IDisposable，
                //ILRuntime虽然支持，但是一定要小心这种用法，使用不当很容易造成不可预期的问题
                //日常开发如果需要实现多个DLL外部接口，请在Unity这边先做一个基类实现那些个接口，然后继承那个基类
                return new Type[] { typeof(AEvent<string>),typeof(IMHandler) };
            }
        }

        public override Type AdaptorType
        {
            get { return typeof(Adaptor); }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain,
            ILTypeInstance instance)
        {
            return new Adaptor(appdomain, instance);
        }

        //Coroutine生成的类实现了IEnumerator<System.Object>, IEnumerator, IDisposable,所以都要实现，这个可以通过reflector之类的IL反编译软件得知
        internal class Adaptor : AEvent<string>, IMHandler,  CrossBindingAdaptorType
        {
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adaptor()
            {

            }

            public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance
            {
                get { return instance; }
            }

            private IMethod mHandle;
            public void Handle()
            {
                if (mHandle == null)
                {
                    mHandle= instance.Type.GetMethod("Handle", 0);
                }
                appdomain.Invoke(mHandle, instance, null);
            }

            private IMethod mHandleA;
            public void Handle(object a)
            {
                if (mHandleA == null)
                {
                    mHandleA = instance.Type.GetMethod("Handle", 1);
                }
                appdomain.Invoke(mHandleA, instance, a);
            }
            private IMethod mHandleB;
            public void Handle(object a, object b)
            {
                if (mHandleB == null)
                {
                    mHandleB = instance.Type.GetMethod("Handle", 2);
                }
                appdomain.Invoke(mHandleB, instance, a,b);
            }
            private IMethod mHandleC;
            public void Handle(object a, object b, object c)
            {
                if (mHandleC == null)
                {
                    mHandleC = instance.Type.GetMethod("Handle", 3);
                }
                appdomain.Invoke(mHandleC, instance, a,b,c);
            }

            private IMethod mRun;
            public override void Run(string a)
            {
                if (mRun == null)
                {
                    mRun = instance.Type.GetMethod("Run", 1);
                }
                if (mRun == null)
                {
                    return;
                }
                appdomain.Invoke(mRun, instance, a);
            }

            private IMethod mIHandle;
            public void Handle(Session session, object message)
            {
                if (mIHandle == null)
                {
                    mIHandle = instance.Type.GetMethod("Handle", 2);
                }
                appdomain.Invoke(mIHandle, instance, session, message);
            }

            private IMethod mGetMessageType;
            public Type GetMessageType()
            {
                if (mGetMessageType == null)
                {
                    mGetMessageType = instance.Type.GetMethod("GetMessageType", 0);
                }
              return  (Type)appdomain.Invoke(mHandleB, instance,null);
            }
        }
    }
}
