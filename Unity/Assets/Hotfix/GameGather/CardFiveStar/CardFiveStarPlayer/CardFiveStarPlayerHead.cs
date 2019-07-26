using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ETModel;

namespace ETHotfix
{
    public class CardFiveStarPlayerHead : Entity
    {
        public CardFiveStarPlayer _Player;
        public User _User;
        public GameObject gameObject;
        public Image _HeadImage;
        public Text _NameText;
        public Text _ScoreText;
        public Text _PiaoNumText;
        public Text _GetScoreText;
        public Transform _ExpressionPoint;
        public GameObject _LostConnectionHintGo;
        public GameObject _HouseIconGo;

        public void Init(CardFiveStarPlayer player, GameObject go)
        {
            _Player = player;
            _User = player._user;
            gameObject = go;
            Transform transform = gameObject.transform;
            _HeadImage = ETModel.GameObjectHelper.FindChild(transform, "HeadImage").GetComponent<Image>();
            _NameText = ETModel.GameObjectHelper.FindChild(transform, "HeadImage/NameBgGo/NameText").GetComponent<Text>();
            _ScoreText = ETModel.GameObjectHelper.FindChild(transform, "ScoreBg/ScoreText").GetComponent<Text>();
            _PiaoNumText = ETModel.GameObjectHelper.FindChild(transform, "PiaoNumText").GetComponent<Text>();
            _GetScoreText = ETModel.GameObjectHelper.FindChild(transform, "GetScoreText").GetComponent<Text>();
            _LostConnectionHintGo = ETModel.GameObjectHelper.FindChild(transform, "LostConnectionHintGo").gameObject;
            _HouseIconGo = ETModel.GameObjectHelper.FindChild(transform, "HouseIconGo").gameObject;
            _ExpressionPoint = ETModel.GameObjectHelper.FindChild(transform, "ExpressionPoint");
            gameObject.GetComponent<Button>().Add(ShowUserInfo,false);
            InitHeadInfo(_Player);


            EventMsgMgr.RegisterEvent(CommEventID.UserOnLine, UserOnLine);//用户上线
            EventMsgMgr.RegisterEvent(CommEventID.UserOffLine, UserOffLine);//用户下线
            
        }
        public void UserOnLine(params object[] objs)
        {
            long userId = (long) objs[0];
            if (userId == _User.UserId)
            {
                SetLostConnectionState(false);
            }
           

        }
        public void UserOffLine(params object[] objs)
        {
            long userId = (long)objs[0];
            if (userId == _User.UserId)
            {
                SetLostConnectionState(true);
            }
        }
        public void ShowUserInfo()
        {
            UIComponent.GetUiView<PersonageInfoPanelComponent>().ShowUserInfo(_Player.ClientSeatIndex, _Player._PersonageInfoPoint, _User);
        }
        public async void InitHeadInfo(CardFiveStarPlayer player)
        {
            _Player = player;
            _User = player._user;
            _NameText.text = _User.Name.OmitStr(4);//名字
            RenewalSocre();
            _HeadImage.sprite = null;
            _HeadImage.sprite =await _User.GetHeadSprite();
            SetLostConnectionState(!_User.IsOnLine);
            _PiaoNumText.gameObject.SetActive(false);
            _HouseIconGo.SetActive(_Player.ServerSeatIndex == 0);
        }

        public const string addScoreFontName = "mjaddscore";
        public const string subScoreFontName = "mjsubscore";
        //玩家分数变化
        public virtual void ScoreChang(int getScore,bool isShowGetScore=true)
        {
            RenewalSocre();
            if (isShowGetScore&& getScore!=0)
            {
                _GetScoreText.gameObject.SetActive(true);
                if (getScore >= 0)
                {
                    _GetScoreText.font = ResourcesComponent.Ins.GetResoure(UIType.CardFiveStarRoomPanel, addScoreFontName) as Font;
                }
                else
                {
                    _GetScoreText.font = ResourcesComponent.Ins.GetResoure(UIType.CardFiveStarRoomPanel, subScoreFontName) as Font;
                }
                _GetScoreText.text = "J" + getScore;
                WaitHideGetScore();
            }
        }
        //更新同步分数
        public void RenewalSocre()
        {
           _ScoreText.text = _Player._NowSocre.ConvertorTenUnit();//分数
        }
        //等待2秒隐藏 得分情况
        public async void WaitHideGetScore()
        {
            await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(2000);
            _GetScoreText.gameObject.SetActive(false);
        }

        //显示漂数
        public virtual void ShowPiaoNum(int num)
        {
            _PiaoNumText.gameObject.SetActive(true);
            if (num > 0)
            {
                _PiaoNumText.text = "漂" + num;
            }
            else
            {
                _PiaoNumText.text = "不漂";
            }
        }
        //隐藏漂数
        public void HidePiaoNum()
        {
            _PiaoNumText.gameObject.SetActive(false);
        }
        //设置离线状态
        public void SetLostConnectionState(bool state)
        {
            _LostConnectionHintGo.SetActive(state);
        }

        private ExpressionAnim _expressionAnim;
        //显示表情
        public  void ShowExpression(int expressionIndex)
        {
            if (_expressionAnim != null)
            {
                _expressionAnim.Destroy();
                _expressionAnim = null;
            }
            _expressionAnim = ExpressionAnimPool.Ins.Create(expressionIndex, _ExpressionPoint);
            _expressionAnim.LocalPositionZero();
            DelayActionTool.ExecuteDelayAction(DelayActionId.ChatExpressionHide + _Player.ClientSeatIndex, 5000, HideExpression);
        }
        //隐藏表情
        public void HideExpression()
        {
            _expressionAnim.Destroy();
            _expressionAnim = null;
        }
    }
}
 