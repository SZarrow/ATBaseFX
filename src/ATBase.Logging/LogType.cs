
using System;
using ATBase.Core;

namespace ATBase.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum LogType
    {
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
