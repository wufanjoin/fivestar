using System;

namespace ETModel
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class UIAttribute: BaseAttribute
    {
        public string Type { get; protected set; }

        public UIAttribute(string type)
        {
            this.Type = type;
        }
    }
}

