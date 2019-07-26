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
    public class PlayerHuCardHintUI : InitBaseItem
    {
        public Button _HintBtn;
        public HuCardHintPanel _HuCardHintPanel;
        public Transform _HintPanel;
        public Transform _HintPanelPointGo;
        private GameObject _titleGo;
        public override void Init(GameObject go)
        {
            base.Init(go);
            _HintBtn = gameObject.FindChild("HintBtn").GetComponent<Button>();
            _HintPanel = gameObject.FindChild("HintPanel");
            _HintPanelPointGo = gameObject.FindChild("HintPanelPointGo");
            _titleGo = gameObject.FindChild("titleGo").gameObject;
        }

        public void InitPanel(int clientSeat)
        {
            if (_HintPanel != null)
            {
                _HuCardHintPanel=_HintPanel.gameObject.AddItem<DownHuCardHintPanel>();
            }
            else if (_HintPanelPointGo != null)
            {
                GameObject hintPanelPrefab=ResourcesComponent.Ins.GetResoure(UIType.FiveStarMingPaiHintPanel, "OtherHintPanel") as GameObject;
                GameObject hintPanel=GameObject.Instantiate(hintPanelPrefab, _HintPanelPointGo);
                if (clientSeat == 1)
                {
                    hintPanel.GetComponent<RectTransform>().pivot=Vector2.one;
                }
                _HuCardHintPanel = hintPanel.AddItem<HuCardHintPanel>();
            }
            PointerEvent _HintBtnPointerEvent = _HintBtn.gameObject.AddComponent<PointerEvent>();
            _HintBtnPointerEvent.OnPointerDownAction -= _HuCardHintPanel.Show;
            _HintBtnPointerEvent.OnPointerDownAction += _HuCardHintPanel.Show;
            _HintBtnPointerEvent.OnPointerUpAction -= _HuCardHintPanel.Hide;
            _HintBtnPointerEvent.OnPointerUpAction += _HuCardHintPanel.Hide;
        }

        //明牌时候 的胡牌提示
        public async void ShowMingCardHuHint(List<int> cards)
        {
            Show();
            _HintBtn.gameObject.SetActive(true);
            _titleGo.gameObject.SetActive(true);
            _HuCardHintPanel.RefreshHintPanel(cards);
            await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(2500);
            _HuCardHintPanel.Hide();
        }
        //录像明牌时候 的胡牌提示
        public void Video_ShowMingCardHuHint(List<int> cards)
        {
            Show();
            _HintBtn.gameObject.SetActive(true);
            _titleGo.gameObject.SetActive(true);
            _HuCardHintPanel.Hide();
        }

        //打牌时候的提示
        public void ShowChuCardHint(List<int> cards)
        {
            Show();
            _HuCardHintPanel.gameObject.GetComponent<RectTransform>().localPosition=new Vector2(220,-190);
            _HintBtn.gameObject.SetActive(false);
            _titleGo.gameObject.SetActive(false);
            _HuCardHintPanel.Show();
            _HuCardHintPanel.RefreshHintPanel(cards);
        }

        //打完牌听牌时候的提示
        public void ShowTingHuCardHint(List<int> cards)
        {
            Show();
            _HuCardHintPanel.gameObject.GetComponent<RectTransform>().localPosition = new Vector2(388, -190);
            _HintBtn.gameObject.SetActive(true);
            _titleGo.gameObject.SetActive(false);
            _HuCardHintPanel.RefreshHintPanel(cards);
            _HuCardHintPanel.Hide();
        }
    }
}
