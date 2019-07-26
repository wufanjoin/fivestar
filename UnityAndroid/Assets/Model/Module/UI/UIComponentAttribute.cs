using System;

namespace ETModel
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UIComponentAttribute: UIAttribute
    {
        public UIComponentAttribute(string type):base(type)
        {
            this.Type = type;
        }
    }
}

