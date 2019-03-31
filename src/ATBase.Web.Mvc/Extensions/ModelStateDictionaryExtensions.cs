using ATBase.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATBase.Web.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msd"></param>
        public static String GetFirstErrorMessage(this ModelStateDictionary msd)
        {
            if (msd != null && msd.Count > 0)
            {
                foreach (var fieldName in msd.Keys)
                {
                    if (msd[fieldName].ValidationState == ModelValidationState.Invalid)
                    {
                        return msd.GetErrorMessage(fieldName);
                    }
                }
            }

            return String.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msd"></param>
        /// <param name="fieldName"></param>
        public static String GetErrorMessage(this ModelStateDictionary msd, String fieldName)
        {
            if (msd != null && msd.Count > 0)
            {
                if (msd.TryGetValue(fieldName, out ModelStateEntry v))
                {
                    if (v.Errors != null && v.Errors.Count > 0)
                    {
                        return v.Errors[0].ErrorMessage;
                    }
                }
            }

            return String.Empty;
        }
    }
}