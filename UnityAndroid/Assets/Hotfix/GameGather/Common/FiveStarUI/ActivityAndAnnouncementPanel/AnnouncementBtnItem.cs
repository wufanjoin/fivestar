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
    [ObjectSystem]
    public class AnnouncementBtnItemAwakeSystem : AwakeSystem<AnnouncementBtnItem, GameObject, AnnouncementConfig>
    {
        public override void Awake(AnnouncementBtnItem self, GameObject go, AnnouncementConfig data)
        {
            self.Awake(go, data, UIType.AgencyMgrPanel);
        }
    }
    public   class AnnouncementBtnItem:BaseItem<AnnouncementConfig>
    {
        public Toggle _Toggle;
        private Text _NameText;
        private Text _HicNameText;
        public override void Awake(GameObject go, AnnouncementConfig data, string uiType)
        {
            base.Awake(go, data, uiType);
            _Toggle = gameObject.GetComponent<Toggle>();
            _NameText = gameObject.FindChild("Text").GetComponent<Text>();
            _HicNameText = gameObject.FindChild("hic/Text").GetComponent<Text>();
            _Toggle.Add(ToggleEvent);
            _NameText.text = mData.Name;
            _HicNameText.text = mData.Name;
        }

        public void ToggleEvent(bool isOn)
        {
            if (isOn)
            {
                UIComponent.GetUiView<ActivityAndAnnouncementPanelComponent>()._AnnouncementView.ShowAnnouncement(mData);
            }
        }
    }
}
