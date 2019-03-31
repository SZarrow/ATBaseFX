using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATBase.Core;
using ATBase.Serialization;
using Newtonsoft.Json;

namespace ATBase.Net.Http
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpX : IDisposable
    {
        private readonly HttpClient _client = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public HttpX(HttpClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            _client = client;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postUrl"></param>
        /// <param name="postContent"></param>
        /// <param name="timeoutMiliSeconds">超时毫秒</param>
        [Obsolete("此方法已废弃，请改用HttpClientExtensions下的方法")]
        public async Task<XResult<HttpContent>> PostAsync(String postUrl, HttpContent postContent, Int32? timeoutMiliSeconds = null)
        {
            HttpResponseMessage respMsg = null;
            var cts = timeoutMiliSeconds != null ? new CancellationTokenSource(timeoutMiliSeconds.Value) : null;

            try
            {
                if (cts != null)
                {
                    respMsg = await _client.PostAsync(postUrl, postContent, cts.Token);
                }
                else
                {
                    respMsg = await _client.PostAsync(postUrl, postContent);
                }
            }
            catch (Exception ex)
            {
                return new XResult<HttpContent>(null, ex);
            }

            try
            {
                return new XResult<HttpContent>(respMsg.Content);
            }
            catch (Exception ex)
            {
                return new XResult<HttpContent>(null, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="postUrl"></param>
        /// <param name="jsonString"></param>
        /// <param name="config"></param>
        /// <param name="timeoutMiliSeconds">超时毫秒</param>
        [Obsolete("此方法已废弃，请改用HttpClientExtensions下的方法")]
        public async Task<XResult<TResult>> PostJsonAsync<TResult>(String postUrl, String jsonString, Action<HttpContent> config = null, Int32? timeoutMiliSeconds = null)
        {
            HttpContent content = new StringContent(jsonString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content.Headers.ContentEncoding.Add("UTF-8");

            if (config != null)
            {
                config(content);
            }

            var respContent = await PostAsync(postUrl, content, timeoutMiliSeconds);
            if (!respContent.Success)
            {
                return new XResult<TResult>(default(TResult), respContent.FirstException);
            }

            try
            {
                String respString = await respContent.Value.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TResult>(respString);
                return new XResult<TResult>(result);
            }
            catch (Exception ex)
            {
                return new XResult<TResult>(default(TResult), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postUrl"></param>
        /// <param name="formData"></param>
        /// <param name="timeoutMiliSeconds">超时毫秒</param>
        [Obsolete("此方法已废弃，请改用HttpClientExtensions下的方法")]
        public async Task<XResult<HttpContent>> PostFormAsync(String postUrl, IDictionary<String, String> formData, Int32? timeoutMiliSeconds = null)
        {
            var content = new FormUrlEncodedContent(formData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            content.Headers.ContentEncoding.Add("UTF-8");
            return await PostAsync(postUrl, content, timeoutMiliSeconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="postUrl"></param>
        /// <param name="xml"></param>
        /// <param name="timeoutMiliSeconds">超时毫秒</param>
        [Obsolete("此方法已废弃，请改用HttpClientExtensions下的方法")]
        public async Task<XResult<TResult>> PostXmlAsync<TResult>(String postUrl, String xml, Int32? timeoutMiliSeconds = null)
        {
            HttpContent content = new StringContent(xml);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            content.Headers.ContentEncoding.Add("UTF-8");

            var respContent = await PostAsync(postUrl, content, timeoutMiliSeconds);
            if (!respContent.Success)
            {
                return new XResult<TResult>(default(TResult), respContent.FirstException);
            }

            XSerializer serializer = new XSerializer();
            try
            {
                var respString = await respContent.Value.ReadAsStringAsync();
                var result = serializer.Deserialize<TResult>(respString);
                return result != null ? new XResult<TResult>(result) : new XResult<TResult>(default(TResult), new NullReferenceException(nameof(result)));
            }
            catch (Exception ex)
            {
                return new XResult<TResult>(default(TResult), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="requestUrl"></param>
        /// <param name="timeoutMiliSeconds">超时毫秒</param>
        [Obsolete("此方法已废弃，请改用HttpClientExtensions下的方法")]
        public async Task<XResult<HttpContent>> GetAsync<TResult>(String requestUrl, Int32? timeoutMiliSeconds)
        {
            HttpResponseMessage respMsg = null;
            var cts = timeoutMiliSeconds != null ? new CancellationTokenSource(timeoutMiliSeconds.Value) : null;

            try
            {
                if (cts != null)
                {
                    respMsg = await _client.GetAsync(requestUrl, cts.Token);
                }
                else
                {
                    respMsg = await _client.GetAsync(requestUrl);
                }
            }
            catch (Exception ex)
            {
                return new XResult<HttpContent>(null, ex);
            }

            try
            {
                return new XResult<HttpContent>(respMsg.Content);
            }
            catch (Exception ex)
            {
                return new XResult<HttpContent>(null, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }
    }
}
