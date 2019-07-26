using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
public   static class AlarmClockFactory
{
        private static GameObject alarmClockPrefab;
        public static AlarmClock Create(Transform parentTransform)
        {
            AlarmClock alarmClock=ComponentFactory.Create<AlarmClock>();
            if (alarmClockPrefab == null)
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                alarmClockPrefab = resourcesComponent.GetResoure(UIType.JoyLandlordsRoomPanel, "AlarmClock") as GameObject;
            }
            alarmClock.Init(GameObject.Instantiate(alarmClockPrefab, parentTransform));
            return alarmClock;
        }
    }
}
