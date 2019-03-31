using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ATBase.Core;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace ATBase.Web.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mvcBuilder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddXDI(this IMvcBuilder mvcBuilder)
        {
            var feature = new ControllerFeature();
            mvcBuilder.PartManager.PopulateFeature(feature);

            foreach (var controller in feature.Controllers.Select(c => c.AsType()))
            {
                mvcBuilder.Services.AddTransient(controller, controller);
            }

            //.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            mvcBuilder.Services.AddHttpContextAccessor();

            XDI.AddServices(mvcBuilder.Services);

            //mvcBuilder.Services.Add(new ServiceDescriptor(typeof(IDIContainer), containerBuilder.Build()));
            mvcBuilder.Services.Add(ServiceDescriptor.Transient<IControllerActivator, XDIControllerActivator>());

            return mvcBuilder;
        }
    }
}
