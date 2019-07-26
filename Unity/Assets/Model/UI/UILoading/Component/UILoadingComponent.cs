using System;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
	[ObjectSystem]
	public class UiLoadingComponentAwakeSystem : AwakeSystem<UILoadingComponent>
	{
		public override void Awake(UILoadingComponent self)
		{
		    self.Awake();

        }
	}

    public class PopHintOptionType
    {
        public const int Single = 1;
        public const int Double = 2;
    }
	public class UILoadingComponent : Component
	{
	    public static UILoadingComponent Ins;
        public Image _ProgressbarImage;
	    public Text _ScheduleText;
	    public GameObject _PopHintBgGo;
	    public GameObject _ProgressbarBgGo;

        public Text _HintContentText;

        public Button _ConsentBtn;
	    public Text _ConsentText;
	    public Button _TurnBtn;
	    public Text _TurnText;
	    public Button _AffirmBtn;
	    public Text _AffirmText;
	    private TimerComponent timerComponent;
        public void Awake()
        {
            Ins = this;
            _ProgressbarImage = GetParent<UI>().GameObject.Get<GameObject>("ProgressbarImage").GetComponent<Image>();
	        _ScheduleText = GetParent<UI>().GameObject.Get<GameObject>("ScheduleText").GetComponent<Text>();

	        _PopHintBgGo = GetParent<UI>().GameObject.Get<GameObject>("PopHintBgGo");
            _ProgressbarBgGo = GetParent<UI>().GameObject.Get<GameObject>("ProgressbarBgGo");
            _HintContentText = GetParent<UI>().GameObject.Get<GameObject>("HintContentText").GetComponent<Text>();
            _ConsentBtn = GetParent<UI>().GameObject.Get<GameObject>("ConsentBtn").GetComponent<Button>();
	        _ConsentText = GetParent<UI>().GameObject.Get<GameObject>("ConsentText").GetComponent<Text>();
	        _TurnBtn = GetParent<UI>().GameObject.Get<GameObject>("TurnBtn").GetComponent<Button>();
	        _TurnText = GetParent<UI>().GameObject.Get<GameObject>("TurnText").GetComponent<Text>();

	        _AffirmBtn = GetParent<UI>().GameObject.Get<GameObject>("AffirmBtn").GetComponent<Button>();
	        _AffirmText = GetParent<UI>().GameObject.Get<GameObject>("AffirmText").GetComponent<Text>();
	        timerComponent = Game.Scene.GetComponent<TimerComponent>();

            _ConsentBtn.Add(ClickYseActionBtn);
	        _AffirmBtn.Add(ClickYseActionBtn);
	        _TurnBtn.Add(ClickNoActionBtn);
	        GameUpdateMgr.Ins.JieYaResAction += JieYaResAction;
	        GameUpdateMgr.Ins.UpdateResAction += UpdateResAction;
	        GameUpdateMgr.Ins.DownPackeageAction += DownPackeageAction;
        }

        //复制资源
	    public void JieYaResAction(float prog)
	    {
	        //显示假进度条
	        ShowFakeProgress("解压资源中不消耗流量");

	    }


        //热更下载资源
	    public  void UpdateResAction(float prog, int currNum, int TotalNum)
	    {
            //显示假进度条
	       // ShowFakeProgress("加载资源中");假进度条
	        ShowProgressbar();
	        _ProgressbarImage.fillAmount = prog / 100.00f;
            _ScheduleText.text = $"加载资源中...   {(int)prog}% ({currNum}/{TotalNum})";
        }

        private float _FakeProg = 0;

	    private bool IsShowFakeProgreeIn = false;

	    private string _FakeProgressHint;
        //显示假进度条
	    public async void ShowFakeProgress(string hitncontent)
	    {
	        ShowProgressbar();
	        _FakeProg = 0;
	        _FakeProgressHint = hitncontent;

            if (IsShowFakeProgreeIn)
	        {
	            return;
	        }
	        IsShowFakeProgreeIn = true;
            //假进度条
            while (true)
            {
            
                if (_FakeProg <= 0.98f)
	            {
	                _FakeProg += 0.01f;
	            }
	            await timerComponent.WaitAsync(100);
                if (IsDisposed)
                {
                    break;
                }
                _ProgressbarImage.fillAmount = _FakeProg;
	            _ScheduleText.text = $"{_FakeProgressHint}...   {(int)(_FakeProg * 100)}%";

	        }
	        IsShowFakeProgreeIn = false;
	    }
        //下载apk安装包
        public void DownPackeageAction(float prog)
	    {
	        ShowProgressbar();
            //apk进度条 是真实的
	        _ProgressbarImage.fillAmount = prog;
	        _ScheduleText.text = $"下载安装包中...   {prog * 100}%";
        }

        public void ClickYseActionBtn()
	    {
	        _PopHintBgGo.SetActive(false);
            _ClickCall?.Invoke(true);

        }
	    public void ClickNoActionBtn()
	    {
	        _PopHintBgGo.SetActive(false);
            _ClickCall?.Invoke(false);
        }
        private Action<bool> _ClickCall;

	    //显示提示UI
        public void ShowHint(string coentent,Action<bool> clickCall,int optoionType= PopHintOptionType.Double, string consentStr="下载",string turnStr="取消")
	    {
	        if (IsDisposed)
	        {
	            return;
	        }
            _ProgressbarBgGo.SetActive(false);
	        _ScheduleText.gameObject.SetActive(false);
	        _PopHintBgGo.SetActive(true);
	        _ClickCall = clickCall;

            _HintContentText.text = coentent;
	        if (optoionType == PopHintOptionType.Double)
	        {
	            _ConsentBtn.gameObject.SetActive(true);
	                _TurnBtn.gameObject.SetActive(true);
                _AffirmBtn.gameObject.SetActive(false);
            }
	        else
	        {
	            _ConsentBtn.gameObject.SetActive(false);
	            _TurnBtn.gameObject.SetActive(false);
	            _AffirmBtn.gameObject.SetActive(true);
            }
	        _ConsentText.text = consentStr;
	        _TurnText.text = turnStr;
	        _AffirmText.text = consentStr;

        }
        //显示进度条UI
	    public void ShowProgressbar()
	    {
	        if (IsDisposed)
	        {
	            return;
	        }
            _ProgressbarBgGo.SetActive(true);
	        _ScheduleText.gameObject.SetActive(true);
	        _PopHintBgGo.SetActive(false);
        }


    }
}
