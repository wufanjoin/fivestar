using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETHotfix;

namespace ETModel
{
    [ObjectSystem]
    public class FiveStarRoomComponentAwakeSystem : AwakeSystem<FiveStarRoomComponent>
    {
        public override void Awake(FiveStarRoomComponent self)
        {
            self.Awake();
        }
    }
    public class FiveStarRoomComponent : Component
    {
        public static FiveStarRoomComponent Ins { get; private set; }
        public readonly List<int> RoomIds=new List<int>();//存放所有的房间id
        public readonly Dictionary<int, FiveStarRoom> pJoyLdsRoomDic = new Dictionary<int, FiveStarRoom>();//存储现在有所房间
        public DBProxyComponent dbProxyComponent;
        public static long CurrTime = 0;

        private int _MaxMiltaryId=100;
        private int _MaxRecordDataId=106585;
        public async void Awake()
        {
            Ins = this;
            dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
            List<Miltary> miltaryIdMax = await dbProxyComponent.SortQuery<Miltary>(miltary => true, miltary => miltary.MiltaryId == -1, 1);
            if (miltaryIdMax.Count > 0)
            {
                _MaxMiltaryId = miltaryIdMax[0].MiltaryId;
            }
            List<ParticularMiltaryRecordDataInfo> dataIdMax = await dbProxyComponent.SortQuery<ParticularMiltaryRecordDataInfo>
                (data => true, data => data.DataId == -1, 1);
            if (dataIdMax.Count > 0)
            {
                _MaxRecordDataId = dataIdMax[0].DataId;
            }

        }
        //获得大局 录像ID
        public  int GetMiltaryVideoId()
        {
            return ++_MaxMiltaryId;
        }
        //获取小局数据 起始ID
        public  int GetMiltaryDataStartId()
        {
            _MaxRecordDataId += 20;
            return _MaxRecordDataId;
        }
        public async Task SaveVideo(ComponentWithId component)
        {
           await dbProxyComponent.Save(component);
        }
      
    }
}
