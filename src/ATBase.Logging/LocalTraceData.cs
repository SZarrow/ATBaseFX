using System;
using System.Collections.Generic;
using System.Text;

namespace ATBase.Logging
{
    [Serializable]
    public class LocalTraceData
    {
        public String AppName { get; set; }
        public String TraceId { get; set; }
    }
}
