using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Core;
using ATBase.LocalMQ.Common;

namespace ATBase.LocalMQ.Appenders
{
    internal class FileAppender : IAppender
    {
        public XResult<Boolean> Append(Byte[] data)
        {
            throw new NotImplementedException();
        }

        public XResult<Boolean> Append(Byte[] data, Int32 offset, Int32 length)
        {
            throw new NotImplementedException();
        }
    }
}
