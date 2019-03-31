using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace ATBase.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Enum)]
    public class XElementAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="namespaceName"></param>
        public XElementAttribute(String elementName, String namespaceName = null)
        {
            if (String.IsNullOrWhiteSpace(elementName))
            {
                throw new ArgumentNullException(nameof(elementName));
            }

            this.ElementName = elementName;

            if (!String.IsNullOrWhiteSpace(namespaceName))
            {
                this.Namespace = namespaceName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String ElementName { get; }
        /// <summary>
        /// 
        /// </summary>
        public String Namespace { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class XElementAttributeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static Boolean IsXElement(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                return false;
            }

            return propertyInfo.GetCustomAttribute<XElementAttribute>() != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static Boolean IsXCollection(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                return false;
            }

            return propertyInfo.GetCustomAttribute<XCollectionAttribute>() != null;
        }
    }
}
