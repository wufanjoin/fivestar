using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
  public  class CopyUser
    {
        public static User Copy(User user)
        {
            User newUser=new User();
            newUser.UserId = user.UserId;
            newUser.Beans = user.Beans;
            newUser.Jewel = user.Jewel;
            newUser.Name = user.Name;
            newUser.Icon = user.Icon;
            newUser.Sex = user.Sex;
            newUser.Ip = user.Ip;
            newUser.Location = user.Location;
            newUser.IsOnLine = user.IsOnLine;
            return newUser;
        }
    }
}
