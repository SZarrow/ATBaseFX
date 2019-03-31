using System;
using System.Collections.Generic;
using System.Text;

namespace ATBase.Core
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumValueAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public EnumValueAttribute(Object value)
        {
            this.Value = value;
        }
        /// <summary>
        /// 
        /// </summary>
        public Object Value { get; }
    }
}
