using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    [ObjectSystem]
    public class JoyLdsRoomComponentAwakeSystem : AwakeSystem<JoyLdsRoomComponent>
    {
        public override void Awake(JoyLdsRoomComponent self)
        {
            self.Awake();
        }
    }
    public class JoyLdsRoomComponent : Component
    {
        public Dictionary<long,JoyLdsRoom> pJoyLdsRoomDic=new Dictionary<long, JoyLdsRoom>();
        public void Awake()
        {
            
        }
    }
}
