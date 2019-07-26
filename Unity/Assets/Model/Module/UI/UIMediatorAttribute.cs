using System;

namespace ETModel
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UIMediatorAttribute: UIAttribute
    {
        public UIMediatorAttribute(string type):base(type)
        {
            this.Type = type;
        }
    }
}
