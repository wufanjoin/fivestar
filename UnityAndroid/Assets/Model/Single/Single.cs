using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    public abstract class Single<T> where T : Single<T>, new()
    {
        private static T ins;

        public static T Ins
        {
            get
            {
                if (ins == null)
                {
                    ins=new T();
                    ins.Init();
                }
                return ins;
            }
        }

        public virtual void Init()
        {
            
        }
      
    }

}
