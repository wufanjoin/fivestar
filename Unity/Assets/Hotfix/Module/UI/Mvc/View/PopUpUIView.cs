using DG.Tweening;
using ETModel;
using NPOI.SS.UserModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class PopUpUIView:UIView
    {
        private static GameObject maskObje;
        private static bool isHaveMask = false;
        private static Transform paremtTransform;
        public virtual float MaskLucencyValue
        {
            get
            {
                return 0.5f;
            }
        }
        public  Color maskColor
        {
            get
            {
                return VectorHelper.GetLucencyBlackColor(MaskLucencyValue);
            }
        }

        public override string pCavasName
        {
            get
            {
                return CanvasType.PopUpCanvas;
            }
        }


        public override void Awake()
        {
            
            base.Awake();
            //如果是第一个出现PopUp层UI就要实力化遮罩
            if (!isHaveMask)
            {
                paremtTransform = this.gameObject.transform.parent;
                isHaveMask = true;
                maskObje = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UI/mask"), paremtTransform);
                maskObje.transform.SetAsFirstSibling();
            }
        }
        public virtual void MaskClickEvent()
        {
            this.Hide();
        }

        public virtual bool isShakeAnimation
        {
            get { return true; }
        } 
        //显示类似抖动的效果
        public override async void OnShow()
        {
            base.OnShow();
            maskObje.SetActive(true);
            if (isShakeAnimation)
            {
                this.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                this.gameObject.transform.DOScale(1.1f, 0.1f).OnComplete(DOScaleOnComplete);
            }
            maskObje.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex()-1);
            SetMaskColor();
            //默认所有的PopUI点击Mask会隐藏
            maskObje.GetComponent<Button>().Add(MaskClickEvent);
        }

        public void DOScaleOnComplete()
        {
            this.gameObject.transform.DOScale(1, 0.15f);
        }
        private void SetMaskColor()
        {
            SetmaskColor(maskColor);
        }

        protected void SetmaskColor(Color color)
        {
            maskObje.GetComponent<Image>().color = color;
        }
        //隐藏的时候判断要不要隐藏mask
        public override void OnHide()
        {
            base.OnHide();
            for (int i = 0; i < paremtTransform.childCount; i++)
            {
                if (paremtTransform.GetChild(i).gameObject.activeInHierarchy&& paremtTransform.GetChild(i).gameObject!= maskObje)
                {
                    if (i == 0)
                    {
                        maskObje.transform.SetAsFirstSibling();
                    }
                    else
                    {
                        maskObje.transform.SetSiblingIndex(i - 1);
                    }
                    return;
                }
            }
            maskObje.SetActive(false);
        }
    }
}