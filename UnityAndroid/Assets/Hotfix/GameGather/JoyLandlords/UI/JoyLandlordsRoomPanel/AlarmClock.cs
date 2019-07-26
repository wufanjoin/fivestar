using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class AlarmClock:Entity
    {
        public GameObject gameObject;
        public Text TimerText;
        public void Init(GameObject go)
        {
            gameObject = go;
            gameObject.transform.localPosition = Vector3.zero;
            TimerText = gameObject.transform.GetChild(0).GetComponent<Text>();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            currTime = 0;
        }

        public int currTime = 30;
        public void Show(int residueTime,int siblingIndex)
        {
            gameObject.transform.SetSiblingIndex(siblingIndex);
            Show(residueTime);
        }
        public void Show(int residueTime)
        {
            currTime = residueTime;
            gameObject.SetActive(true);
            TimerText.text = residueTime.ToString();
            StartTime();
        }

        private bool isEndTime = false;
        public async void StartTime()
        {
            if (!isEndTime)
            {
                return;
            }
            isEndTime = false;
            while (--currTime>=0)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);
                TimerText.text = currTime.ToString();
            }
            isEndTime = true;
        }
    }
}
