using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ATBase.Core.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonConfig
    {
        /// <summary>
        /// 加载指定的*.json配置文件，并返回IConfiguration接口
        /// </summary>
        /// <param name="jsonConfigFilePath">*.json配置文件</param>
        public static IConfiguration Load(String jsonConfigFilePath)
        {
            if (String.IsNullOrWhiteSpace(jsonConfigFilePath) ||
                !File.Exists(jsonConfigFilePath))
            {
                return null;
            }

            var cb = new ConfigurationBuilder();
            cb.AddJsonFile(jsonConfigFilePath);
            return cb.Build();
        }
    }
}
