using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Data.MongoDb;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class MongoDbServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddMongoDb(this IServiceCollection services, Action<MongoDbOption> configAction = null)
        {
            services.AddScoped(provider =>
            {
                var option = new MongoDbOption();

                if (configAction != null)
                {
                    configAction(option);
                }

                return new MongoDbContext(option);
            });

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="configAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddMongoDb<T>(this IServiceCollection services, Action<MongoDbOption> configAction = null) where T : MongoDbContext
        {
            services.AddScoped(provider =>
            {
                var option = new MongoDbOption();

                if (configAction != null)
                {
                    configAction(option);
                }

                var ctorInfo = typeof(T).GetConstructor(new Type[] { typeof(MongoDbOption) });
                if (ctorInfo == null)
                {
                    throw new NullReferenceException($"unable to reflect the constructor info of  type '{typeof(T).FullName}'");
                }

                try
                {
                    return ctorInfo.Invoke(new Object[] { option }) as T;
                }
                catch (Exception)
                {
                    return default(T);
                }
            });

            return services;
        }
    }
}
