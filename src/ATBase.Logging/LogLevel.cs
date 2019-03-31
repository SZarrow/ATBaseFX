using System;
using System.Collections.Generic;
using System.Text;

namespace ATBase.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum LogLevel
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0x00000,
        /// <summary>
        /// 
        /// </summary>
        Trace = 0x00001,
        /// <summary>
        /// 
        /// </summary>
        Debug = 0x00010,
        /// <summary>
        /// 
        /// </summary>
        Info = 0x00100,
        /// <summary>
        /// 
        /// </summary>
        Warn = 0x01000,
        /// <summary>
        /// 
        /// </summary>
        Error = 0x10000
    }
}
