using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Core;

namespace ATBase.MQCore.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        XResult<Boolean> Validate();
    }
}
