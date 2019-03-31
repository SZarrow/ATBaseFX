using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Core;
using ATBase.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ATBase.Web.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public class XDIControllerActivator : IControllerActivator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Object Create(ControllerContext context)
        {
            var controllerType = context.ActionDescriptor.ControllerTypeInfo.AsType();
            var controller = context.HttpContext.RequestServices.GetService(controllerType);

            if (controller != null)
            {
                XDI.CreateScope(controller);
                XDI.Composite(controller);
            }

            return controller;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="controller"></param>
        public void Release(ControllerContext context, Object controller)
        {
            XDI.Release(controller);
        }
    }
}
