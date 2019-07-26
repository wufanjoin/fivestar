using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public class ChuCardJianTouMgr : Single<ChuCardJianTouMgr>
    {
        public GameObject JianTouGo;
        public override void Init()
        {
            base.Init();
            GameObject JianTouGoPrefabGo = ResourcesComponent.Ins.GetResoure(UIType.CardFiveStarRoomPanel, "ChuCardJianTouImage") as GameObject;
            JianTouGo = GameObject.Instantiate(JianTouGoPrefabGo,
                UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().gameObject.transform);
        }

        private Coroutine _AnimCoroutine;
        private Vector3 _offsetVector3 = new Vector3(0, 0, 0);
        public void Show(GameObject cardGo,int yaxleOfset)
        {
            if (_AnimCoroutine != null)
            {
                CoroutineMgr.StopCoroutinee(_AnimCoroutine);
            }

            JianTouGo.gameObject.SetActive(true);
            JianTouGo.transform.SetParent(UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().gameObject.transform);
            JianTouGo.transform.localScale =Vector3.one;
            JianTouGo.transform.SetParent(cardGo.transform);
            _offsetVector3.y = yaxleOfset;
            JianTouGo.transform.localPosition = Vector3.zero+ _offsetVector3;
            _AnimCoroutine=CoroutineMgr.StartCoroutinee(JinaTouAnim());
        }

        private Vector3 _animOffsetVector3 = new Vector3(0, 5, 0);
        private IEnumerator JinaTouAnim()
        {
            while (JianTouGo.activeInHierarchy)
            {
                for (int i = 0; i < 4; i++)
                {
                    JianTouGo.transform.localPosition += _animOffsetVector3;
                  yield return new WaitForSeconds(0.1f);
                }
                for (int i = 0; i < 4; i++)
                {
                    JianTouGo.transform.localPosition -= _animOffsetVector3;
                    yield return new WaitForSeconds(0.1f);
                }
            }
            Hide();
        }
        public void Hide()
        {
            JianTouGo.gameObject.SetActive(false);
        }
    }
}
