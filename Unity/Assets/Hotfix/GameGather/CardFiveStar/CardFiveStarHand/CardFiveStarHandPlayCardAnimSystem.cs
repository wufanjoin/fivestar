using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;

namespace ETHotfix
{
 public  partial class CardFiveStarHandComponent
    {
        public bool _PlayCardAnimIn = false;//是否正在播放出牌动画
        private RepeatedField<int> _NewestHands;//最新手牌数组 仅仅用来记录刷新 和胡牌提示
        private Vector3 YiWanVector3 = new Vector3(10000, 0, 0);//把打出的牌 移动到看不见的地方位置
        //打牌 动画 不要在调整了 DOTween的时间不是那么 正确 现在移牌是0.6秒 在剩0.1秒的时候 移动摸的牌 只能剩0.1秒的时候移动 不然容易出显示BUG
        public async Task PlayCardAnimation()
        {
            try
            {
                if (_NewHand == UpChuCardHand || CardFiveStarRoom.Ins.GetUserPlayerInfo().IsLiangDao || _isSelectLiang)//打出的牌就是手牌 或者是亮倒模式下 没有动画
                {
                    return;
                }
                _PlayCardAnimIn = true;
                _NewChuCardAnim.SetCardUI(_NewHand.CardSize);//显示 播放出牌动画 的牌
                int newHandPlaceIndex = GetNewHandPlaceIndex();//获取新摸的牌 应该放置的位置



                Vector3 vacncyLocation = _HandLists[newHandPlaceIndex].gameObject.transform.position; //记录摸的牌应该放置的位置

                if (_HandLists[newHandPlaceIndex].gameObject.transform.localPosition == YiWanVector3)//如果牌 是拖动牌 位置要 用之前记录的位置
                {
                    vacncyLocation = _DragOriginalPoint;
                }
                //记录摸的牌应该放置的位置
                Vector3 vacncyBackLocation;
                if (newHandPlaceIndex + 1 == _HandLists.Count)
                {
                    vacncyBackLocation = vacncyLocation;
                }
                else
                {
                    vacncyBackLocation = _HandLists[newHandPlaceIndex + 1].gameObject.transform.position; //记录摸的牌应该放置的位置 的后一个位置
                    if (_HandLists[newHandPlaceIndex + 1].gameObject.transform.localPosition == YiWanVector3)//如果牌 是拖动牌 位置要 用之前记录的位置
                    {
                        vacncyBackLocation = _DragOriginalPoint;
                    }
                }

                UpChuCardHand.gameObject.transform.localPosition = YiWanVector3;//把牌移动到看不见的地方

                //根据打出牌和摸的牌来判断左移还是右移
                if (UpChuCardHand.iHandIndex > newHandPlaceIndex)
                {
                    for (int i = newHandPlaceIndex; i < UpChuCardHand.iHandIndex; i++)
                    {
                        _HandLists[i].RightMove();
                    }
                }
                else
                {
                    for (int i = newHandPlaceIndex; i > UpChuCardHand.iHandIndex; i--)
                    {
                        _HandLists[i].LeftMove();
                    }
                }

                //牌移动50%的时候 摸的牌移动位置 动画回调 刷新手牌 还原摸的牌
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(400);
                _NewChuCardAnim.MoveToVacancy(vacncyLocation, vacncyBackLocation, RestoreHands);
                _PlayCardAnimIn = false;
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }

        }
        //刷新手牌 隐藏摸的牌
        public void RestoreHands()
        {
            RefreshHand(_NewestHands);
            _NewChuCardAnim.SetActive(false);//隐藏动画的牌
            _NewChuCardAnim.LocalPositionZero();//还原动画牌的位置
        }

        //获取新手牌应该放置的位置
        public int GetNewHandPlaceIndex()
        {
            _NewestHands.Remove(UpChuCardHand.CardSize);
            _NewestHands.Sort();
            if (_NewestHands[_NewestHands.Count - 1] <= _NewHand.CardSize)
            {
                return _NewestHands.Count - 1;
            }
            for (int i = 0; i < _NewestHands.Count; i++)
            {
                if (_NewestHands[i] == _NewHand.CardSize)
                {
                    //if (_HandLists.Count-1 < i)
                    //{
                    //    return _HandLists.Count - 1;
                    //}
                    return i;
                }
            }
            return 0;
        }

        //开启检测鼠标位置出牌
        public void StartDetecionMousePlayCard()
        {
            if (!_IsCanChuCard)
            {
                return;//不可以出牌 就直接返回
            }
            DetectionMouseLocation();
        }
        private bool _IsDetetionPoint = false;//是否检测鼠标位置

        private int _MousePositionYExcusionPlay = 50;//往上偏移多少 就出牌

        public Vector3 _DragOriginalPoint = Vector3.zero;//拖动牌原有位置位置
        public Vector3 _DragPlayPoint = Vector3.zero;//拖动出牌位置
        //跟踪记录鼠标的位置y轴超过一定 就请求出牌
        private async void DetectionMouseLocation()
        {
            if (_IsDetetionPoint)
            {
                return;
            }
            _IsDetetionPoint = true;
            _DragOriginalPoint = _partyChuHand.gameObject.transform.position;//记录原本位置

            //牌弹起
            _partyChuHand.SetPitchStatu(true);
            Vector3 handOriginalPoint = _partyChuHand.gameObject.transform.localPosition;//记录被选中时候的位置

           Vector3 gap=Camera.main.ScreenToWorldPoint(Input.mousePosition)- _partyChuHand.gameObject.transform.position;//转换世界坐标后 计算与牌位置的差值

            Canvas handCanvas = _partyChuHand.gameObject.AddComponent<Canvas>();
            handCanvas.overrideSorting = true;
            handCanvas.sortingOrder = 10000;//设置渲染层级在最上面

          

            while (_IsDetetionPoint)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(50);

                //视口位置坐标再转化为世界坐标
                _partyChuHand.gameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gap;
            }
            GameObject.Destroy(handCanvas);//删除这个分层级的组件

            if (_partyChuHand.gameObject.transform.localPosition.y - handOriginalPoint.y > _MousePositionYExcusionPlay) //拉的高度 高于原来的50 就直接发起出牌请求
            {
                _DragPlayPoint = _partyChuHand.gameObject.transform.position;//记录拖动出牌 牌的位置
                if (RequestPlayCard(_partyChuHand))//直接出预选牌 
                {
                    _partyChuHand.gameObject.transform.localPosition = YiWanVector3;//如果出牌成功 把牌移动到看不见的地方
                }
                else
                {
                    _partyChuHand.gameObject.transform.localPosition = handOriginalPoint;//还原牌的位置 到选中的位置
                }
            }
            else
            {
                _partyChuHand.gameObject.transform.localPosition = handOriginalPoint;//还原牌的位置 到选中的位置
            }
        }
        //结束鼠标位置检测 出牌
        public void EndDetecionMousePlayCard()
        {
            _IsDetetionPoint = false;
        }

    }
}
