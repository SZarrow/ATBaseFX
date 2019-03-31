using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using ATBase.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace ATBase.Web.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpRequestExtensions
    {
        private static readonly CancellationTokenSource _cts = new CancellationTokenSource(new TimeSpan(0, 0, 10));

        /// <summary>
        /// 将当前键值对集合转换成指定类型的实体
        /// </summary>
        /// <typeparam name="T">要转换成的实体的类型</typeparam>
        /// <param name="request"></param>
        public static T MapForm<T>(this HttpRequest request)
        {
            if (request == null)
            {
                return default(T);
            }

            T instance;
            IFormCollection form;

            try
            {
                form = request.ReadFormAsync(_cts.Token).GetAwaiter().GetResult();
                instance = Activator.CreateInstance<T>();
            }
            catch (Exception)
            {
                return default(T);
            }

            if (form == null || instance == null)
            {
                return default(T);
            }

            var insType = typeof(T);

            var properties = insType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
            foreach (var property in properties)
            {
                if (form.TryGetValue(property.Name, out StringValues value))
                {
                    property.XSetValue(instance, (String)value);
                }
            }

            return instance;
        }
    }
}
