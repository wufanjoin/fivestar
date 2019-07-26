using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.LobbyMiddlePanel)]
    public class LobbyMiddlePanelComponent:ChildUIView
    {
        #region 脚本工具生成的代码
        private GameObject List_GroupGo;
        private GameObject mGameIconItemGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            List_GroupGo = rc.Get<GameObject>("List_GroupGo");
            mGameIconItemGo=rc.Get<GameObject>("GameIconItemGo");
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
          //  GameVersionsConfig[] AllGameVersionsConfig =Game.Scene.GetComponent<GameVersionsConfigComponent>().GetAllGameVersionsConfig();
           // foreach (var gameVersions in AllGameVersionsConfig)
            {
            //    if (gameVersions.Id == ToyGameId.Common)
          //      {
          //          continue;
          //      }
              GameObject go= GameObject.Instantiate(mGameIconItemGo, mGameIconItemGo.transform.parent);
        //      ComponentFactory.Create<LobbyGameIconItem,GameObject,GameVersionsConfig>(go,gameVersions);
            }
            mGameIconItemGo.SetActive(false);
        }

        private void mGameIconItemGoEvnet()
        {
            Game.Scene.GetComponent<UIComponent>().Show(UIType.BaseHallPanel);
        }
    }
}
