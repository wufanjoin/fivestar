using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace ETModel
{
    public class ButtonAnimationEffect : MonoBehaviour,IPointerDownHandler, IPointerUpHandler//EventTrigger包含所有的
    {
        private  static Vector3 downScale=new Vector3(1.1f,1.1f,1.1f);
        private  static Vector3 upScale=Vector3.one;

        public static Action ButtonPointerUpAction;//按钮抬起事件
        public void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log("按下" + this.gameObject.name);
            this.transform.localScale=downScale;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            ButtonPointerUpAction?.Invoke();
           //Debug.Log("抬起" + this.gameObject.name);
           this.transform.localScale=upScale;
        }
    }
}