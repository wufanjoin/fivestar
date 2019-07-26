using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public class BaseView : InitBaseItem
    {

        protected ReferenceCollector rc;

        public override void Init(GameObject go)
        {
            base.Init(go);
            rc = gameObject.GetComponent<ReferenceCollector>();
        }
    }
}
