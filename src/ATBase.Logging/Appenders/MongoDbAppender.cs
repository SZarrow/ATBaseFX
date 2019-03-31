using System;
using System.Collections.Generic;
using System.Linq;
using ATBase.Core;
using ATBase.Data.MongoDb;

namespace ATBase.Logging.Appenders
{
    /// <summary>
    /// 
    /// </summary>
    public class MongoDbAppender : Appender
    {
        private readonly MongoDbLogConfig _config;
        private readonly MongoDbContext _db;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public MongoDbAppender(MongoDbLogConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            _config = config;
            _db = new MongoDbContext(new MongoDbOption()
            {
                ConnectionString = _config.ConnectionString
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public override LogLevel LogLevel
        {
            get
            {
                return _config.LogLevel;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contents"></param>
        public override XResult<IEnumerable<LogEntity>> Write(IEnumerable<LogEntity> contents)
        {
            if (contents == null || contents.Count() == 0)
            {
                return new XResult<IEnumerable<LogEntity>>(null);
            }

            _db.AddRange(contents);
            var result = _db.SaveChanges();
            if (result.Success)
            {
                return new XResult<IEnumerable<LogEntity>>(contents);
            }

            return new XResult<IEnumerable<LogEntity>>(null, result.FirstException);
        }
    }
}
