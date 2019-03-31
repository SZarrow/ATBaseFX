using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ATBase.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IResult<T>
    {
        /// <summary>
        /// Indicates whether the execution result is successful or failed. True is successful, False is failed.
        /// </summary>
        Boolean Success { get; }

        /// <summary>
        /// Gets the return value object.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Gets error code.
        /// </summary>
        Int32 ErrorCode { get; }

        /// <summary>
        /// Gets error message.
        /// </summary>
        String ErrorMessage { get; }

        /// <summary>
        /// Gets the first exception.
        /// </summary>
        Exception FirstException { get; }
    }
}
