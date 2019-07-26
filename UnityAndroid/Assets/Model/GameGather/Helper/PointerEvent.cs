
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{
    public class PointerEvent : MonoBehaviour, IPointerEnterHandler,IPointerDownHandler, IPointerUpHandler, IPointerExitHandler//EventTrigger包含所有的
    {
        public Action OnPointerDownAction;
        public Action OnPointerUpAction;
        public Action OnPointerEnterAction;
        public Action OnPointerExitAction;

        public void OnPointerDown(PointerEventData eventData)
        {
            // Log.Debug(this.name + "OnPointerDown");
            OnPointerDownAction?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Log.Debug(this.name + "OnPointerUp");
            OnPointerUpAction?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Log.Debug(this.name + "IPointerEnterHandler");
            OnPointerEnterAction?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitAction?.Invoke();
        }
    }
}
