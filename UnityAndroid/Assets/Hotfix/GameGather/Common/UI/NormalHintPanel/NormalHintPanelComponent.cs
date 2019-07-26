using System;
using System.Threading;
using System.Threading.Tasks;
using ETModel;
using NPOI.SS.Formula.Functions;
using UnityEngine;
using UnityEngine.UI;
using Text = UnityEngine.UI.Text;

namespace ETHotfix
{
    [UIComponent(UIType.NormalHintPanel)]
    public class NormalHintPanelComponent:HintUIView
    {
        #region 脚本工具生成的代码
        private Text mContentText;
        private Image mBgImage;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mContentText=rc.Get<GameObject>("ContentText").GetComponent<Text>();
            mBgImage=rc.Get<GameObject>("BgImage").GetComponent<Image>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
        }

        public void ShowHintPanel(string content)
        {
            Show();
            mContentText.text = content;
            mBgImage.color = InitialColor;
            mContentText.color = InitialColor;
            GradientHide();
        }
        
        Color InitialColor=new Color(1,1,1,1);
        public override  void OnShow()
        {
            base.OnShow();
        }


        //控制永远只有一个
        bool isBeGradientHide=false;
        //计时器
        private int timer=1000;
        //悬停的时间
        private int HoverTime = 1000;
        private async void GradientHide()
        {
            timer = HoverTime;
            if (isBeGradientHide)
            {
                return;
            }
            isBeGradientHide = true;
            while (timer>0)
            {
                await Task.Delay(100);
                timer -= 100;
            }
            ColorGradientHide();
            isBeGradientHide = false;
        }

        //颜色逐渐消失
        private async void ColorGradientHide()
        {
            while (mBgImage.color.a>0)
            {
                await Task.Delay(100);
                Color newColor = new Color(1, 1, 1, mBgImage.color.a - 0.1f);
                mBgImage.color=newColor;
                mContentText.color=newColor;
                if (timer > 0)
                {
                    return;
                }
            }
            this.Hide();
        }
    }
}
