using System;
using System.Collections.Generic;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class MagicExpressionsType
    {
        public const string JiDan = "jidan";
        public const string MeiGui = "meigui";
        public const string ZanJie = "zanjie";
        public const string ZhaDan = "zhadan";
        public const string ZhuanTou = "zhuantou";
    }

    public class MagicExpressionsAnim
    {
        public const string IconSpritePrefix = "MagicIcon_";
        private GameObject _iconGo;
        private string currType;
        public MagicExpressionsPanelComponent magicPanel;
        private Vector3 _endPoint;
        public void ExecuteAnim(Vector3 startPoint, Vector3 endPoint, string type)
        {
            _endPoint = endPoint;
            currType = type;
            _iconGo = magicPanel.GetIconGo(type);
            //坐标还原到起点
            _iconGo.transform.transform.position = startPoint;
            float dictance=Vector3.Distance(startPoint, endPoint);
            dictance=Mathf.Clamp(dictance / 200, 0.5f, 0.8f);//距离除以200 就是运动时间 最低0.5秒 最长0.8秒
            Log.Debug("距离"+ dictance);  
            //移动到终点
            _iconGo.transform.DOMove(endPoint, dictance).OnComplete(IconMoveOnCmplete);
        }

        
        public async void IconMoveOnCmplete()
        {
            GameObject animGo = magicPanel.GetAnimGo();
            animGo.transform.position = _endPoint;
            Animator animator = animGo.GetComponent<Animator>();
            animator.SetBool(currType, true);

            //await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(500);
            magicPanel.ReplaceIconGo(_iconGo);

            await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(3000);
            animator.SetBool(currType, false);

            magicPanel.ReplaceAnimGo(animGo);
            magicPanel.ReplaceExecuteAnim(this);
        }
    }
    [UIComponent(UIType.MagicExpressionsPanel)]
    public class MagicExpressionsPanelComponent : ChildUIView
    {
        #region 脚本工具生成的代码
        private GameObject mMagicExpressionAnimGoPrefab;
        private GameObject mMagicExpressionIconGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mMagicExpressionIconGo = rc.Get<GameObject>("MagicExpressionIconGo");
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mMagicExpressionAnimGoPrefab = GetResoure<GameObject>("MagicExpressionAnimGo");
            InitAnimationClipDic();
        }
        public Dictionary<string, AnimationClip> _AnimationClipDic = new Dictionary<string, AnimationClip>();

        private void InitAnimationClipDic()
        {
            List<string> clipsNames = new List<string>()
            {
                MagicExpressionsType .JiDan,
                MagicExpressionsType.MeiGui,
                MagicExpressionsType.ZanJie,
                MagicExpressionsType.ZhaDan,
                MagicExpressionsType.ZhuanTou
            };
            for (int i = 0; i < clipsNames.Count; i++)
            {
                _AnimationClipDic[clipsNames[i]] = ResourcesComponent.Ins.GetResoure(UIType.MagicExpressionsPanel, clipsNames[i]) as AnimationClip;
            }

        }


        private GameObject _iconGo;

        public void ShowMagicExpressions(Vector3 startPoint, Vector3 endPoint, string type)
        {
            Show();
            //获取一个动画 执行类 让他去执行
            GetExecuteAnim().ExecuteAnim(startPoint, endPoint, type);
        }
        //执行类对象池
        private List<MagicExpressionsAnim> _executeAnimPool = new List<MagicExpressionsAnim>();

        public MagicExpressionsAnim GetExecuteAnim()
        {
            MagicExpressionsAnim executeAnim;
            if (_AnimPool.Count > 0)
            {
                executeAnim = _executeAnimPool[0];

                _executeAnimPool.RemoveAt(0);
                return executeAnim;
            }
            executeAnim = new MagicExpressionsAnim();
            executeAnim.magicPanel = this;
            return executeAnim;
        }
        public void ReplaceExecuteAnim(MagicExpressionsAnim animExecut)
        {
            _executeAnimPool.Add(animExecut);
        }

        //动画对象池
        List<GameObject> _AnimPool = new List<GameObject>();
        public GameObject GetAnimGo()
        {
            GameObject go;
            if (_AnimPool.Count > 0)
            {
                go = _AnimPool[0];
                _AnimPool[0].SetActive(true);
                _AnimPool.RemoveAt(0);
                return go;
            }
            go = GameObject.Instantiate(mMagicExpressionAnimGoPrefab, gameObject.transform);
            go.SetActive(true);
            return go;
        }

        public void ReplaceAnimGo(GameObject go)
        {
            _AnimPool.Add(go);
        }

        //Icon对象池子
        List<GameObject> _IconPool = new List<GameObject>();

        public GameObject GetIconGo(string type)
        {
            GameObject go;
            if (_IconPool.Count > 0)
            {
                go = _IconPool[0];
                _IconPool.RemoveAt(0);
                SetIcon(go, type);
                return go;
            }
            go = GameObject.Instantiate(mMagicExpressionIconGo, mMagicExpressionIconGo.transform.parent);
            SetIcon(go, type);
            return go;
        }

        public void SetIcon(GameObject go, string type)
        {
            go.SetActive(true);
            Image image = go.GetComponent<Image>();
            image.SetNativeSize();
            image.sprite = GetResoure<Sprite>(MagicExpressionsAnim.IconSpritePrefix + type);
        }
        public void ReplaceIconGo(GameObject go)
        {
            go.SetActive(false);
            _IconPool.Add(go);
        }

    }
}
