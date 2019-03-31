using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace ATBase.Data.MongoDb
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class IdentityEntity
    {
        /// <summary>
        /// 系统主键
        /// </summary>
        [BsonId]
        public Int64 MongoId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected IdentityEntity()
        {
            var bytes = Guid.NewGuid().ToByteArray();
            this.MongoId = BitConverter.ToInt64(bytes, 0);
        }
    }
}
