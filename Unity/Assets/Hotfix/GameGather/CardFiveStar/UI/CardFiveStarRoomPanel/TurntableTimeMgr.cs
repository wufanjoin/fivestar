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
    public class TurntableTimeMgr : Single<TurntableTimeMgr>
    {
        public override void Init()
        {
            base.Init();
        }

        public GameObject gameObject;
        public Text _TimeText;
        public Dictionary<int,Image> _DirectionLight=new Dictionary<int, Image>();
        public void InitUI(GameObject go)
        {
            gameObject = go;
            _TimeText = gameObject.FindChild("TimeText").GetComponent<Text>();
            _DirectionLight.Add(0, gameObject.FindChild("Directions/DownLight").GetComponent<Image>());
            _DirectionLight.Add(1, gameObject.FindChild("Directions/RightLight").GetComponent<Image>());
            _DirectionLight.Add(2, gameObject.FindChild("Directions/UpLight").GetComponent<Image>());
            _DirectionLight.Add(3, gameObject.FindChild("Directions/LeftLight").GetComponent<Image>());
            StartTime();
        }

        private bool _IsBeingTime = false;
        public void StartTime()
        {
            _IsBeingTime = true;
            ResidueTimeReduce();
            LightPresent();
        }
        public void EndTime()
        {
            _IsBeingTime = false;
        }
        //隐藏灯光和重置时间
        public void HideLigheAndRestTime(int time)
        {
            ResetTime(time);
            foreach (var image in _DirectionLight)
            {
                image.Value.gameObject.SetActive(false);
            }
        }
        //显示灯光和重置时间
        private Image _PresentFickerLight;
        private int _CurrResidueTimeSecond;
        public void ShowLightAndResetTime(int clientSeatIndex,int time)
        {
            ResetTime(time);
            if (!_DirectionLight.ContainsKey(clientSeatIndex))
            {
                Log.Error("转盘显示的灯光 索引不存在:"+ clientSeatIndex);
            }
            foreach (var image in _DirectionLight)
            {
                image.Value.gameObject.SetActive(false);
            }
            _PresentFickerLight=_DirectionLight[clientSeatIndex];
            _PresentFickerLight.gameObject.SetActive(true);
        }

        //重置时间
        public void ResetTime(int time)
        {
            _CurrResidueTimeSecond = time;
            _TimeText.text = _CurrResidueTimeSecond.ToString();
        }
        private bool isResidueTimeReduceIn = false;
        public async void ResidueTimeReduce()
        {
            if (isResidueTimeReduceIn)
            {
                return;
            }
            isResidueTimeReduceIn = true;
            while (true)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);
                if (_CurrResidueTimeSecond > 0)
                {
                    _TimeText.text = (--_CurrResidueTimeSecond).ToString();
                }
            }
            isResidueTimeReduceIn = false;
        }
        private bool isLightPresentIn = false;
        public async void LightPresent()
        {
            if (isLightPresentIn)
            {
                return;
            }
            isLightPresentIn = true;
            while (true)
            {
                for (float i = 0; i <=1; i+=0.1f)
                {
                    await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
                    if (_PresentFickerLight == null)
                    {
                        continue;
                    }
                    _PresentFickerLight.color= VectorHelper.GetLucencyWhiteColor(i);
                }
                for (float i = 1; i >= 0; i -= 0.1f)
                {
                    await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
                    if (_PresentFickerLight == null)
                    {
                        continue;
                    }
                    _PresentFickerLight.color = VectorHelper.GetLucencyWhiteColor(i);
                }
            }
            isLightPresentIn = false;

        }
    }
}
