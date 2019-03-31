using System;
using System.Collections.Generic;
using System.Text;

namespace ATBase.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class XSelectAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xpath"></param>
        public XSelectAttribute(String xpath)
        {
            this.XPath = xpath;
        }

        /// <summary>
        /// 
        /// </summary>
        public String XPath { get; }
    }
}
