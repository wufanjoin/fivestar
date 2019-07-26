using System;
using System.Collections.Generic;
using System.Text;
using ETHotfix;

namespace ETModel
{
    public class SessionUserComponent : Component
    {
        public User user { get; set; }//Session所对应的User


        public long UserId
        {
            get { return user.UserId; }
        }
        public long GamerSessionActorId { get; set; }
    }

}
