using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public partial class User
    {
        private Sprite _SpriteIcon;

        public async ETTask<Sprite> GetHeadSprite()
        {
            if (_SpriteIcon != null)
            {
                return _SpriteIcon;
            }
            Sprite sprite = await LoadResoure.LoadSprite(Icon);
            if (sprite == null)
            {
                //默认头像
                sprite=ResourcesComponent.Ins.GetResoure("", "defaultIcon") as Sprite;
            }
            _SpriteIcon = sprite;
            return _SpriteIcon;
        }

        //获取显示ID 因为有可能是机器人
        private long _showUserId=0;
        public long GetShowUserId()
        {
            if (UserId >= 1000)
            {
                return UserId;
            }
            if (_showUserId == 0)
            {
                _showUserId = RandomTool.Random(1235, 9999);
            }
            return _showUserId;
        }

        //定位信息 str
        private string[] _LocationSplit;
        private string[] LocationSplit
        {
            get
            {
                if (_LocationSplit == null)
                {
                    _LocationSplit = Location.Split(GlobalConstant.ParameteSeparator);
                }
                return _LocationSplit;
            }
        }
        //定位地址名称
        public string GetLocationAddress()
        {
            if (LocationSplit.Length != 3)
            {
                return string.Empty;
            }
            return LocationSplit[0];
        }
        //定位经度
        public double GetLng()
        {
            return double.Parse(LocationSplit[1]);
        }
        //定位纬度
        public double GetLat()
        {
            return double.Parse(LocationSplit[2]);
        }

        //根据定位算出两个User的距离
        public double GetDistance(User targetUser)
        {
            if (LocationSplit.Length != 3 || targetUser.LocationSplit.Length != 3)
            {
                return -1;
            }
            return GetDistance(GetLng(), GetLat(), targetUser.GetLng(), targetUser.GetLat());
        }

        public static double GetDistance(double lng1, double lat1, double lng2,
            double lat2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2)
                                               + Math.Cos(radLat1) * Math.Cos(radLat2)
                                               * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000d) / 10000d;
            s = s * 1000;//返回的是米
            return s;
        }
        private static double EARTH_RADIUS = 6378.137;
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
    }
}
