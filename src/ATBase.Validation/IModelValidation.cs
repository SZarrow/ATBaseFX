using System;
using System.Collections.Generic;
using System.Text;

namespace ATBase.Validation
{
    /// <summary>
    /// 
    /// </summary>
    public interface IModelValidation
    {
        /// <summary>
        /// 
        /// </summary>
        Boolean IsValid { get; }
        /// <summary>
        /// 
        /// </summary>
        String ErrorMessage { get; }
    }
}
