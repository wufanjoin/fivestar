using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace ETHotfix
{

    public class CardFiveStarNewHand : CardFiveStarHand
    {
        private static CardFiveStarNewHand ins;
        public static CardFiveStarNewHand Ins
        {
            get
            {
                if (ins == null)
                {
                    //ins = CardFiveStarCardFactory.CreateNewHand(6, new Transform());
                }
                return ins;
            }
        }
        public Vector3 _UpMoveLocation;
        public Vector3 _UpMoveRotate =new Vector3(0,0,-40);
       // public float _height;

        public override void SetCardUI(int size)
        {
            base.SetCardUI(size);
            gameObject.transform.localPosition=Vector3.zero;
        }

        public override void Init(GameObject go)
        {
            base.Init(go);
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
           // _height = rectTransform.rect.height;
            gameObject.transform.localPosition=Vector3.zero;
            _UpMoveLocation = gameObject.transform.localPosition -
                              new Vector3(rectTransform.rect.width, -rectTransform.rect.height*0.9f, 0);
        }

        private Vector3 _VacncyLocation;
        private Vector3 _VacncyBackLocation;
        private TweenCallback _ToVacancyCallback;
        public void MoveToVacancy(Vector3 vacncyLocation, Vector3 vacncyBackLocation, TweenCallback callAction)
        {
            _VacncyLocation = vacncyLocation;
            _VacncyBackLocation = vacncyBackLocation;
            _ToVacancyCallback = callAction;
            if (_VacncyLocation == _VacncyBackLocation)
            {
                gameObject.transform.DOMoveX(_VacncyLocation.x, 0.2f).OnComplete(_ToVacancyCallback);
                return;
            }
            gameObject.transform.DOLocalMove(_UpMoveLocation,0.2f);
            gameObject.transform.DOLocalRotate(_UpMoveRotate, 0.2f).OnComplete(MoveToVacancy_2);

           
        }

        private void MoveToVacancy_2()
        {
            float distance=Mathf.Abs(_VacncyBackLocation.x - gameObject.transform.position.x);
            distance *= 0.05f;
            gameObject.transform.DOMoveX(_VacncyBackLocation.x, distance).OnComplete(MoveToVacancy_3);
        }

        private void MoveToVacancy_3()
        {
            gameObject.transform.DOMove(_VacncyLocation, 0.2f);
            gameObject.transform.DOLocalRotate(Vector3.zero, 0.2f).OnComplete(_ToVacancyCallback);
            
        }
    }
}
