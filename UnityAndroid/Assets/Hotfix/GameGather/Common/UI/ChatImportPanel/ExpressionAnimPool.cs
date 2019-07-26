using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
   public class ExpressionAnimPool: Single<ExpressionAnimPool>
   {
       public const int _ExpressionAnimCount = 18;
       private Dictionary<int, string> _expressionAnimDic = new Dictionary<int, string>()
       {
           {1, "ziya|5"},
           {2, "zhuakuang|9"},
           {3, "zaijian|5"},
           {4, "yun|4"},
           {5, "ye|7"},
           {6, "touxiao|4"},
           {7, "shaoxiang|4"},
           {8, "se|4"},
           {9, "outu|10"},
           {10, "kelian|3"},
           {11, "kanren|2"},
           {12, "haqian|10"},
           {13, "han|9"},
           {14, "haixiu|8"},
           {15, "fanu|11"},
           {16, "deyi|10"},
           {17, "daku|3"},
           {18, "bang|8"},
       };
      private Dictionary<int, Sprite[]> _expressionSpriteDic=new Dictionary<int, Sprite[]>();

       public override void Init()
       {
           base.Init();
           InitSprites();
       }

       public void InitSprites()
       {
           foreach (var anim in _expressionAnimDic)
           {
              string[] splits=anim.Value.Split('|');
               string spriteName = splits[0];
               int maxCount = int.Parse(splits[1]);
               _expressionSpriteDic[anim.Key] = new Sprite[maxCount];
               for (int i = 1; i <= maxCount; i++)
               {
                   string zere = "000";
                   if (i >= 10)
                   {
                       zere = "00";
                    }
                   _expressionSpriteDic[anim.Key][i - 1] =
                       ResourcesComponent.Ins.GetResoure(UIType.ChatImportPanel, spriteName + zere + i) as Sprite;
               }
           }
       }

       private GameObject _expressionAnimGoPrefab;

       private GameObject _ExpressionAnimGoPrefab
       {
           get
           {
               if (_expressionAnimGoPrefab == null)
               {
                   _expressionAnimGoPrefab = ResourcesComponent.Ins.GetResoure(UIType.ChatImportPanel, "ExpressionAnimGo") as GameObject;
                }
               return _expressionAnimGoPrefab;
           }
       }
        private List<ExpressionAnim> _ExpressionAnims=new List<ExpressionAnim>();
       public ExpressionAnim Create(int expressionIndex,Transform expressionParent)
       {
           ExpressionAnim expressionAnim;
           if (_ExpressionAnims.Count > 0)
           {
               expressionAnim = _ExpressionAnims[0];
               _ExpressionAnims.RemoveAt(0);
               expressionAnim.Show(_expressionSpriteDic[expressionIndex]);
               expressionAnim.SetParent(expressionParent);
               return expressionAnim;
           }
           expressionAnim=new ExpressionAnim();
           GameObject expressionAnimGo=GameObject.Instantiate(_ExpressionAnimGoPrefab, expressionParent);
           expressionAnim.Init(expressionAnimGo.GetComponent<Image>(),_expressionSpriteDic[expressionIndex]);
           return expressionAnim;
       }

       public void DestroyExpression(ExpressionAnim expressionAnim)
       {
           _ExpressionAnims.Add(expressionAnim);
       }
    }
}
