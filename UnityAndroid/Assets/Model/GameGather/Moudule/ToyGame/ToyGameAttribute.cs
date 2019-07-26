using System;

namespace ETModel
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ToyGameAttribute : BaseAttribute
    {
        public long Type { get; protected set; }

        public ToyGameAttribute(long type)
        {
            this.Type = type;
        }
    }
}

