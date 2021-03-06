﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ATBase.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="instance"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Object XInvoke(this MethodInfo methodInfo, Object instance, Object[] parameters)
        {
            if (methodInfo == null) { return null; }

            try
            {
                return methodInfo.Invoke(instance, parameters);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 通过构造函数实例化对象。
        /// </summary>
        /// <param name="constructorInfo"></param>
        /// <param name="parameters">注意：这个地方如果传List&lt;Object&gt;类型的参数，要先ToArray()一下。</param>
        public static Object XConstruct(this ConstructorInfo constructorInfo, Object[] parameters)
        {
            if (constructorInfo == null) { return null; }

            try
            {
                return constructorInfo.Invoke(parameters);
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public static void XSetValue(this PropertyInfo propertyInfo, Object instance, Object value)
        {
            if (propertyInfo == null) { return; }
            try
            {
                propertyInfo.SetValue(instance, value);
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static Object XGetValue(this PropertyInfo propertyInfo, Object instance)
        {
            if (propertyInfo == null) { return null; }

            try
            {
                return propertyInfo.GetValue(instance);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public static void XSetValue(this FieldInfo fieldInfo, Object instance, Object value)
        {
            if (fieldInfo == null) { return; }

            try
            {
                fieldInfo.SetValue(instance, value);
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static Object XGetValue(this FieldInfo fieldInfo, Object instance)
        {
            if (fieldInfo == null) { return null; }

            try
            {
                return fieldInfo.GetValue(instance);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
