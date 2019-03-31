using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATBase.Core
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ObjectResult : XResult<Object>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="exception"></param>
        public ObjectResult(Object value, Exception exception = null) : base(value, exception) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorCode"></param>
        public ObjectResult(Object value, Int32 errorCode) : base(value, errorCode) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorCode"></param>
        /// <param name="exception"></param>
        public ObjectResult(Object value, Int32 errorCode, Exception exception) : base(value, errorCode, exception) { }
    }
}
