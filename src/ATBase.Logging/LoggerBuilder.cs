using System;
using System.Collections.Generic;
using System.Text;

namespace ATBase.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class LoggerBuilder
    {
        private List<LogConfig> _configs;

        /// <summary>
        /// 
        /// </summary>
        public LoggerBuilder()
        {
            _configs = new List<LogConfig>(2);
        }

        internal void AddLogConfig(LogConfig config)
        {
            if (config == null) { return; }
            _configs.Add(config);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LogConfig> Build()
        {
            return _configs;
        }
    }
}
