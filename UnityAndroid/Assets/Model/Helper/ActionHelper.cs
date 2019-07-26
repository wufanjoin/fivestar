using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ETModel
{
	public static class ActionHelper
	{
		public static void Add(this Button button, Action action,bool isAnim=true)
		{
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(()=> { action(); });
			if (isAnim&&button.GetComponent<ButtonAnimationEffect>() == null)
			{
				button.transition = Selectable.Transition.None;
				button.gameObject.AddComponent<ButtonAnimationEffect>();
			}
		}

	    public static void Add(this Toggle toggle, Action<bool> action)
	    {
	        toggle.graphic.gameObject.SetActive(toggle.isOn);
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener((bol) =>
            {
                toggle.graphic.gameObject.SetActive(toggle.isOn);
                action(bol);
            });
        }

	    public static void Add(this Slider slider, Action<float> action)
	    {
	        slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener((value)=> { action(value); });
        }

        public static void Add(this Button.ButtonClickedEvent button, Action action)
		{
			button.AddListener(()=> { action(); });
		}
	}
}