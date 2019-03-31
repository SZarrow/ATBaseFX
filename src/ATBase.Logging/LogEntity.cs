using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ATBase.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ATBase.Logging
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class LogEntity
    {
        private static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new DefaultContractResolver(),
            DateFormatString = "yyyy-MM-dd HH:mm:ss"
        };

        /// <summary>
        /// 
        /// </summary>
        public String AppName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String TraceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String ThreadId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public LogType LogType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String TraceType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String CallResultStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String Service { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String Tag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public LogPhase Phase { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String LogContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Object KeyData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Exception[] Exceptions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            var sb = new StringBuilder();

            FormatArguments();

            sb.AppendLine($"[{this.CreateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff")} {this.LogType.ToString().ToUpperInvariant()} {this.ThreadId}{this.TraceId}{this.AppName}{this.TraceType}{this.CallResultStatus}{this.Service}{this.Tag} {this.Phase.ToString()}]{this.LogContent}");

            if (this.KeyData != null)
            {
                IEnumerable<Object> datas = null;

                if (this.KeyData is Object[])
                {
                    datas = this.KeyData as Object[];
                }
                else
                {
                    datas = new Object[] { this.KeyData };
                }

                foreach (var data in datas)
                {
                    sb.Append(":");

                    if (data is String || data is ValueType)
                    {
                        sb.AppendLine(data.ToString());
                        continue;
                    }

                    try
                    {
                        sb.AppendLine(JsonConvert.SerializeObject(data, DefaultJsonSerializerSettings));
                    }
                    catch (Exception ex)
                    {
                        sb.AppendLine($"无法序列化KeyData，原因：{ex.Message}");
                    }
                }
            }

            if (this.Exceptions != null && this.Exceptions.Length > 0)
            {
                sb.AppendLine(LogUtil.MiniFormatExceptions(this.Exceptions, DefaultJsonSerializerSettings));
            }

            return sb.ToString();
        }

        private void FormatArguments()
        {
            if (this.AppName.HasValue())
            {
                if (this.AppName.IndexOf(" ") >= 0)
                {
                    this.AppName = this.AppName.Replace(" ", String.Empty);
                }

                this.AppName = $" {this.AppName}";
            }

            if (this.TraceType.HasValue())
            {
                if (this.TraceType.IndexOf(" ") >= 0)
                {
                    this.TraceType = this.TraceType.Replace(" ", String.Empty);
                }

                this.TraceType = $" {this.TraceType}";
            }

            if (this.CallResultStatus.HasValue())
            {
                if (this.CallResultStatus.IndexOf(" ") >= 0)
                {
                    this.CallResultStatus = this.CallResultStatus.Replace(" ", String.Empty);
                }

                this.CallResultStatus = $" {this.CallResultStatus}";
            }

            if (this.TraceId.HasValue())
            {
                if (this.TraceId.IndexOf(" ") >= 0)
                {
                    this.TraceId = this.TraceId.Replace(" ", String.Empty);
                }

                this.TraceId = $" {this.TraceId}";
            }

            if (this.Service.HasValue())
            {
                if (this.Service.IndexOf(" ") >= 0)
                {
                    this.Service = this.Service.Replace(" ", String.Empty);
                }

                this.Service = $" {this.Service}";
            }

            if (this.Tag.HasValue())
            {
                if (this.Tag.IndexOf(" ") >= 0)
                {
                    this.Tag = this.Tag.Replace(" ", String.Empty);
                }

                this.Tag = $" {this.Tag}";
            }
        }
    }
}
