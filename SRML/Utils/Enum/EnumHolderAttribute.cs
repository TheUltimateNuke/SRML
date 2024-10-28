using System;

namespace SRML.Utils.Enum
{
    public class EnumHolderAttribute : Attribute
    {
        public bool shouldCategorize = true;

        public EnumHolderAttribute()
        {
        }

        public EnumHolderAttribute(bool shouldCategorize)
        {
            this.shouldCategorize = shouldCategorize;
        }
    }
}
