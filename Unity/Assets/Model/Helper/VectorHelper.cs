using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public static class VectorHelper
    {
        private static Dictionary<float, Vector3> _sameVector3 = new Dictionary<float, Vector3>();
        public static Vector3 GetSameVector3(float size)
        {
            if (_sameVector3.ContainsKey(size))
            {
                return _sameVector3[size];
            }
            else
            {
                _sameVector3.Add(size, new Vector3(size, size, size));
            }
            return _sameVector3[size];
        }

        private static Dictionary<float, Vector2> _sameVector2 = new Dictionary<float, Vector2>();
        public static Vector2 GetSameVector2(float size)
        {
            if (_sameVector2.ContainsKey(size))
            {
                return _sameVector2[size];
            }
            else
            {
                _sameVector2.Add(size, new Vector3(size, size, size));
            }
            return _sameVector2[size];
        }

        private static Dictionary<float, Color> _sameColor = new Dictionary<float, Color>();
        public static Color GetSameColor(float value)
        {
            if (_sameColor.ContainsKey(value))
            {
                return _sameColor[value];
            }
            else
            {
                _sameColor.Add(value, new Color(value, value, value));
            }
            return _sameColor[value];
        }

        private static Dictionary<float, Color> _lucencyBlack = new Dictionary<float, Color>();
        public static Color GetLucencyBlackColor(float value)
        {
            if (_lucencyBlack.ContainsKey(value))
            {
                return _lucencyBlack[value];
            }
            else
            {
                _lucencyBlack.Add(value, new Color(0, 0, 0, value));
            }
            return _lucencyBlack[value];
        }

        private static Dictionary<float, Color> _lucencyWhite = new Dictionary<float, Color>();
        public static Color GetLucencyWhiteColor(float value)
        {
            if (_lucencyWhite.ContainsKey(value))
            {
                return _lucencyWhite[value];
            }
            else
            {
                _lucencyWhite.Add(value, new Color(1, 1, 1, value));
            }
            return _lucencyWhite[value];
        }
    }
}
