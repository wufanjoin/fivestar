using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public class InitBaseItem : Entity
    {
        public GameObject gameObject;

        public virtual void Init(GameObject go)
        {
            gameObject = go;
        }
        public virtual async void Show()
        {
            gameObject.SetActive(true);
        }
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
