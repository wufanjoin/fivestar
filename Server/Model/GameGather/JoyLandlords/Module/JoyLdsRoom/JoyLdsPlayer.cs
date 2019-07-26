using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;
using ETHotfix;
using Google.Protobuf.Collections;

namespace ETModel
{
    [ObjectSystem]
    public class JoyLdsPlayerAwakeSystem : AwakeSystem<JoyLdsPlayer>
    {
        public override void Awake(JoyLdsPlayer self)
        {
            self.Awake();
        }
    }
    public class JoyLdsPlayer : Entity
    {
        public int pSeatIndex;
        public RepeatedField<int> pHnads;
        public User pUser;
        public JoyLdsRoom pJoyLdsRoom;

        public int pHandNum
        {
            get { return pHnads.Count; }
        }
        public void Awake()
        {
            
        }

        public override void Dispose()
        {
            base.Dispose();
            pSeatIndex = 0;
            pHnads = null;
            pUser = null;
            pJoyLdsRoom = null;
        }
    }
}
