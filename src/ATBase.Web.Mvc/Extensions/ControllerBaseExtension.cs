using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ATBase.Web.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public static class ControllerBaseExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="value"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static JsonResult Failure(this ControllerBase controller, Object value, String errMsg)
        {
            return Json(controller, new { Status = "FAILURE", ErrMsg = errMsg, Value = value });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static JsonResult Success(this ControllerBase controller, Object value)
        {
            return Json(controller, new { Status = "SUCCESS", Value = value });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="status"></param>
        /// <param name="value"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static JsonResult Json(this ControllerBase controller, String status, Object value, String errMsg = null)
        {
            if (String.IsNullOrWhiteSpace(errMsg))
            {
                return Json(controller, new { Status = status, Value = value });
            }
            else
            {
                return Json(controller, new { Status = status, ErrMsg = errMsg, Value = value });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="value"></param>
        public static JsonResult Json(this ControllerBase controller, Object value)
        {
            return new JsonResult(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="status"></param>
        /// <param name="value"></param>
        /// <param name="errMsg"></param>
        //[Obsolete("此方法已废弃，请改用Json()方法")]
        public static JsonResult FormatJson(this ControllerBase controller, String status, Object value, String errMsg = null)
        {
            return Json(controller, status, value, errMsg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="value"></param>
        /// <param name="errMsg"></param>
        //[Obsolete("此方法已废弃，请改用Failure()方法")]
        public static JsonResult FormatJson(this ControllerBase controller, Object value, String errMsg)
        {
            return new JsonResult(new
            {
                Status = "FAILURE",
                ErrMsg = errMsg,
                Value = value
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="value"></param>
        //[Obsolete("此方法已废弃，请改用Success()方法")]
        public static JsonResult FormatJson(this ControllerBase controller, Object value)
        {
            return new JsonResult(new
            {
                Status = "SUCCESS",
                Value = value
            });
        }
    }
}
