using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
    public static class DragonBonesEditor
    {
        [MenuItem("Tools/顺序列化")]
        public static void xzc()
        {
            TextAsset tsTextAsset = Resources.Load<TextAsset>("mecha_2903/mecha_2903_ske");
            string str = tsTextAsset.text;
            DragonBonesJsonRoot jsonRoot = JsonMapper.ToObject<DragonBonesJsonRoot>(str);

          
            Debug.Log("成功");
        }

        [MenuItem("Tools/顺序列化222")]
        public static void xzc222()
        {
            TextAsset tsTextAsset = Resources.Load<TextAsset>("putongnv/123456789");
            string str = tsTextAsset.text;
            JoyLandlordsJsonRoot jsonRoot = JsonMapper.ToObject<JoyLandlordsJsonRoot>(str);

            AnimJsonConversion animJsonConversion = new AnimJsonConversion();
            DragonBonesJsonRoot dragonBonesJsonRoot=animJsonConversion.ConversionRoot(jsonRoot);
            string strjson=JsonMapper.ToJson(dragonBonesJsonRoot);
            File.WriteAllText("jsontest.json", strjson);
            Debug.Log("成功2222");
        }
    }
}

