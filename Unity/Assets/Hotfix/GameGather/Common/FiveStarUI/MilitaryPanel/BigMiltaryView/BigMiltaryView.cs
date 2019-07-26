using System;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class BigMiltaryView : BaseView
    {
        #region 脚本工具生成的代码
        private Button mExamineOtherBtn;
        private GameObject mBigMiltaryItemGo;
        private GameObject mQueryPlaybackViewGo;
        private GameObject mNoneRecordGo;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mExamineOtherBtn = rc.Get<GameObject>("ExamineOtherBtn").GetComponent<Button>();
            mBigMiltaryItemGo = rc.Get<GameObject>("BigMiltaryItemGo");
            mQueryPlaybackViewGo = rc.Get<GameObject>("QueryPlaybackViewGo");
            mNoneRecordGo = rc.Get<GameObject>("NoneRecordGo");
            Ins = this;
            InitPanel();
        }
        #endregion
        public static BigMiltaryView Ins { get; private set; }
        private QueryPlaybackView _QueryPlaybackView;
        public void InitPanel()
        {
            _QueryPlaybackView=mQueryPlaybackViewGo.AddItem<QueryPlaybackView>();
            mExamineOtherBtn.Add(ExamineOtherBtnEvent);
        }

        public void ExamineOtherBtnEvent()
        {
            _QueryPlaybackView.Show();
        }


        public void ShowBigMiltary(RepeatedField<Miltary> miltaries)
        {
            Show();
            mNoneRecordGo.SetActive(miltaries.Count==0);
            miltaries.Sort(MiltaryTimeSort);//战绩按时间排序一下
            mBigMiltaryItemGo.transform.parent.CreatorChildAndAddItem<BigMiltaryItem, Miltary>(miltaries);
        }

        public int MiltaryTimeSort(Miltary x, Miltary y)
        {
            if (x.Time > y.Time)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}
