using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
   public class CoroutineMgr:MonoBehaviour
   {
       private static CoroutineMgr Ins;
        public void Awake()
        {
            Ins = this;
        }

       public static Coroutine StartCoroutinee(IEnumerator routine)
       {
           if (Ins == null)
           {
               return null;
           }
           return Ins.StartCoroutine(routine);
        }
       public static void StopCoroutinee(Coroutine coroutine)
       {
           if (Ins == null)
           {
               return;
           }
            Ins.StopCoroutine(coroutine);
       }

       public static void StopAllCoroutinee()
       {
           if (Ins == null)
           {
               return;
           }
            Ins.StopAllCoroutines();
       }
    }
}
