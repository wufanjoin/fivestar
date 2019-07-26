using System;
using System.Collections.Generic;
using System.IO;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class ShowCraterRoomPanelType
    {
        public const int NormalCraterRoom = 1;
        public const int AlterWanFa = 2;
    }
    [UIComponent(UIType.CreateRoomPanel)]
    public class CreateRoomPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Text mTitleText;
        private Button mCloseBtn;
        private GameObject mWanFaItemGo;
        private GameObject mOptionItemGo;
        private Text mJewelAmoutText;
        private GameObject mhintGo;
        private Button mConfirmBtn;
        private GameObject mConsumeParentGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mTitleText = rc.Get<GameObject>("TitleText").GetComponent<Text>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mWanFaItemGo = rc.Get<GameObject>("WanFaItemGo");
            mOptionItemGo = rc.Get<GameObject>("OptionItemGo");
            mJewelAmoutText = rc.Get<GameObject>("JewelAmoutText").GetComponent<Text>();
            mhintGo = rc.Get<GameObject>("hintGo");
            mConfirmBtn = rc.Get<GameObject>("ConfirmBtn").GetComponent<Button>();
            mConsumeParentGo = rc.Get<GameObject>("ConsumeParentGo");
            InitPanel();
        }
        #endregion

        public Action<RepeatedField<int>> _ConfirmCallAction;
        public RepeatedField<int> _CreateRoomDefaultConfigs;
        private List<CardFiveStarRoomConfig> CardFiveStarRoomConfigs=new List<CardFiveStarRoomConfig>();

        public string _DefaultConfigFilePath = PathHelper.LocalConfigPath + "/DefaultCreateConfig.txt";
        public void InitPanel()
        {
            IConfig[] iconfigs = Game.Scene.GetComponent<ConfigComponent>().GetAll(typeof(CardFiveStarRoomConfig));
            for (int i = 0; i < iconfigs.Length; i++)
            {
                CardFiveStarRoomConfigs.Add(iconfigs[i] as CardFiveStarRoomConfig);
            }
            for (int i = 0; i < CardFiveStarRoomConfig.ConfigCount; i++)
            {
                _CurrSelectRoomConfig.Add(0);
            }
            mCloseBtn.Add(Hide);
            mConfirmBtn.Add(ConfirmEvent);
            InitCreateRoomDefaultConfig();//初始默认选项列表
            InitCreateRoomOption();//初始化创建房间选项
        }
        //获取默认配置列表
        public RepeatedField<int> GetDefaultConfigs()
        {
            if (_CreateRoomDefaultConfigs == null)
            {
                InitCreateRoomDefaultConfig();
            }
            return _CreateRoomDefaultConfigs;
        }
        public void ConfirmEvent()
        {
            RepeatedField<int> currSelectRoomConfig=GetCurrSelectRoomConfig();
            _ConfirmCallAction.Invoke(currSelectRoomConfig);
            if (_CurrPanelStatu == ShowCraterRoomPanelType.NormalCraterRoom)
            {
                SaveCreateRoomDefaultConfig(currSelectRoomConfig);
            }
            Hide();
        }
        //消耗的钻石数量改变
        public void ConsumeJewelAmountChange()
        {
            int needJewelNum = 0;
            foreach (var optionItem in _OptionItems)
            {
                needJewelNum+=optionItem.Value.ConsumeJewelCount;
            }
            if (_OptionItems.ContainsKey(CardFiveStarRoomConfig.FenTuoDiId))
            {
                needJewelNum -= _OptionItems[CardFiveStarRoomConfig.FenTuoDiId].ConsumeJewelCount;//分拖底的钻石 减去
            }
            mJewelAmoutText.text = "x " + needJewelNum;
        }
        private Dictionary<int, OptionItem> _OptionItems = new Dictionary<int, OptionItem>();
        //初始化创建房间选项
        public void InitCreateRoomOption()
        {
            Transform OptionItemParentTrm = mOptionItemGo.transform.parent;
            for (int i = 0; i < CardFiveStarRoomConfigs.Count-1; i++)
            {
                GameObject.Instantiate(mOptionItemGo, OptionItemParentTrm);
            }
            mConsumeParentGo.transform.SetParent(OptionItemParentTrm.parent);
            for (int i = 0; i < OptionItemParentTrm.childCount; i++)
            {
                OptionItem optionItem=OptionItemParentTrm.GetChild(i).AddItemIfHaveInit<OptionItem, CardFiveStarRoomConfig>(CardFiveStarRoomConfigs[i]);
                if (_CreateRoomDefaultConfigs.Count <= CardFiveStarRoomConfigs[i].Id)
                {
                    _CreateRoomDefaultConfigs.Add(CardFiveStarRoomConfigs[i].DefaultValue);
                }
                optionItem.PitchValue = _CreateRoomDefaultConfigs[(int) CardFiveStarRoomConfigs[i].Id];
                _OptionItems[(int)CardFiveStarRoomConfigs[i].Id] = optionItem;
            }
            mConsumeParentGo.transform.SetParent(OptionItemParentTrm);
            mConsumeParentGo.transform.SetAsLastSibling();
            _OptionItems[CardFiveStarRoomConfig.EndTypeId].Hide();//不要结束方式 只有局数
            _OptionItems[CardFiveStarRoomConfig.FenTuoDiId].Hide();//不要拖底分
            _OptionItems[CardFiveStarRoomConfig.BottomScoreId].Hide();//不要底分
            _OptionItems[CardFiveStarRoomConfig.IsHaveOverTimeId].Hide();//不要超时
            ConsumeJewelAmountChange();
        }
        private RepeatedField<int> _CurrSelectRoomConfig=new RepeatedField<int>();
        //获取选择当前选择
        public RepeatedField<int> GetCurrSelectRoomConfig()
        {
            foreach (var optionItem in _OptionItems)
            {
                _CurrSelectRoomConfig[optionItem.Key] = optionItem.Value.PitchValue;
            }
            return _CurrSelectRoomConfig;
        }
        //初始化房间默认配置
        public void InitCreateRoomDefaultConfig()
        {
            if (_CreateRoomDefaultConfigs != null)
            {
                return;
            }
            if (File.Exists(_DefaultConfigFilePath))
            {
                _CreateRoomDefaultConfigs = new RepeatedField<int>();
                string[]  configStr=File.ReadAllText(_DefaultConfigFilePath).Split(GlobalConstant.ParameteSeparator);
                for (int i = 0; i < configStr.Length; i++)
                {
                    _CreateRoomDefaultConfigs.Add(int.Parse(configStr[i])); 
                }
            }
            else
            {
                int[] roomConfigs = new int[CardFiveStarRoomConfig.ConfigCount];
                for (int i = 0; i < CardFiveStarRoomConfigs.Count; i++)
                {
                    roomConfigs[CardFiveStarRoomConfigs[i].Id] = CardFiveStarRoomConfigs[i].DefaultValue;
                }
                _CreateRoomDefaultConfigs = new RepeatedField<int>() { roomConfigs };
            }
        }
        //保存创建房间配置
        public void SaveCreateRoomDefaultConfig(RepeatedField<int> roomConfigs)
        {
            string configContent = "";
            for (int i = 0; i < roomConfigs.Count; i++)
            {
                if (i == roomConfigs.Count - 1)
                {
                    configContent += roomConfigs[i].ToString();
                }
                else
                {
                    configContent += roomConfigs[i].ToString() + GlobalConstant.ParameteSeparator;
                }
            }
            File.WriteAllText(_DefaultConfigFilePath, configContent);
        }

        private int _CurrPanelStatu = 0;
        //显示创建房间面板
        public void ShowCraterRoomPanel(int type, Action<RepeatedField<int>> call, RepeatedField<int> defaultConfig=null)
        {
            Show();
            _CurrPanelStatu = type;
            if (ShowCraterRoomPanelType.AlterWanFa == type)
            {
                mTitleText.text = "修改玩法";
                _OptionItems[CardFiveStarRoomConfig.PayMoneyId].Hide();//不要支付方式 亲友圈的房间 圈主扣
            }
            else
            {
                mTitleText.text = "创建房间";
                _OptionItems[CardFiveStarRoomConfig.PayMoneyId].Show();//不要支付方式 亲友圈的房间 圈主扣
            }
            _ConfirmCallAction = call;
        }
    }
}
