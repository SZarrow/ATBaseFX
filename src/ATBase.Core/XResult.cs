using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace ATBase.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class XResult<T> : IResult<T>, IEquatable<XResult<T>>
    {

        private const Int32 CODE_SUCCESS = 0;
        private const Int32 CODE_FAILURE = 1;
        private readonly Int64 _id;

        /// <summary>
        /// Gets the instance of Result&lt;T&gt; with default constructor。
        /// </summary>
        public static XResult<T> Default { get; } = new XResult<T>(default(T));

        /// <summary>
        /// Represents the operation is success, and the value of property "Value" will return the default(T).
        /// </summary>
        public XResult(T value) : this(value, null)
        {
            this.Value = value;
        }
        /// <summary>
        /// Initialize the instance of Result with value and exception.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="exception"></param>
        public XResult(T value, Exception exception)
            : this(value,
            exception != null ? CODE_FAILURE : CODE_SUCCESS,
            exception)
        { }

        /// <summary>
        /// Initialize the instance of Result with value and error code.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorCode"></param>
        public XResult(T value, Int32 errorCode) : this(value, errorCode, ErrorCodeDescriptor.GetDescription(errorCode)) { }

        /// <summary>
        /// Initialize the instance of Result with value, error code and exception.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        public XResult(T value, Int32 errorCode, String errorMessage) : this(value, errorCode, new SystemException(errorMessage)) { }

        /// <summary>
        /// Initialize the instance of Result with value, error code and exception.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorCode"></param>
        /// <param name="exception"></param>
        public XResult(T value, Int32 errorCode, Exception exception)
        {
            _id = DateTime.Now.Ticks;
            this.Value = value != null ? value : default;
            this.ErrorCode = errorCode;
            this.ErrorMessage = ErrorCodeDescriptor.GetDescription(errorCode);

            if (exception != null)
            {
                this.FirstException = exception;
                this.ErrorMessage = this.FirstException.Message;
            }
            else
            {
                if (this.ErrorCode != CODE_SUCCESS)
                {
                    this.FirstException = new SystemException(this.ErrorMessage);
                }
            }

            this.Success = this.ErrorCode == CODE_SUCCESS && this.FirstException == null;
        }

        /// <summary>
        /// Gets error code.
        /// </summary>
        public Int32 ErrorCode { get; }
        /// <summary>
        /// Gets error message.
        /// </summary>
        public String ErrorMessage { get; }
        /// <summary>
        /// Gets the first exception.
        /// </summary>
        public Exception FirstException { get; }
        /// <summary>
        /// Indicates whether the execution result is successful or failed. True is successful, False is failed.
        /// </summary>
        public Boolean Success { get; }
        /// <summary>
        /// Gets the return value object.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public Boolean Equals(XResult<T> other)
        {
            return this._id == other._id;
        }
    }
}
