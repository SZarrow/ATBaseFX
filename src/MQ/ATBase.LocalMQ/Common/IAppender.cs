using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Core;

namespace ATBase.LocalMQ.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAppender
    {
        XResult<Boolean> Append(Byte[] data);
        XResult<Boolean> Append(Byte[] data, Int32 offset, Int32 length);
    }
}
