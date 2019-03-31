using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ATBase.Core;
using ATBase.Net.Exceptions;
using ATBase.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ATBase.Net
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpClientExtensions
    {
        private static readonly XSerializer serializer = new XSerializer();

        /// <summary>
        /// 
        /// </summary>
        public static JsonSerializerSettings JsonSerializerSettings { get; set; } = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatString = "yyyy-MM-dd HH:mm:ss",
            ContractResolver = new DefaultContractResolver()
        };


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="client"></param>
        /// <param name="postUrl"></param>
        /// <param name="jsonString"></param>
        /// <param name="config"></param>
        /// <param name="token"></param>
        public static XResult<TResult> PostJson<TResult>(this HttpClient client, String postUrl, String jsonString, Action<HttpContent> config = null, CancellationToken? token = null)
        {
            try
            {
                return client.PostJsonAsync<TResult>(postUrl, jsonString, config, token).ConfigureAwait(false).GetAwaiter().GetResult();
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
        /// <param name="client"></param>
        /// <param name="postUrl"></param>
        /// <param name="xml"></param>
        /// <param name="config"></param>
        /// <param name="token"></param>
        public static XResult<TResult> PostXml<TResult>(this HttpClient client, String postUrl, String xml, Action<HttpContent> config = null, CancellationToken? token = null)
        {
            try
            {
                return client.PostXmlAsync<TResult>(postUrl, xml, config, token).ConfigureAwait(false).GetAwaiter().GetResult();
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
        /// <param name="client"></param>
        /// <param name="postUrl"></param>
        /// <param name="formData"></param>
        /// <param name="config"></param>
        /// <param name="token"></param>
        public static XResult<TResult> PostForm<TResult>(this HttpClient client, String postUrl, IDictionary<String, String> formData, Action<HttpContent> config = null, CancellationToken? token = null)
        {
            try
            {
                return client.PostFormAsync<TResult>(postUrl, formData, config, token).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                return new XResult<TResult>(default(TResult), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="postUrl"></param>
        /// <param name="jsonString"></param>
        /// <param name="config"></param>
        /// <param name="token"></param>
        public async static Task<XResult<TResult>> PostJsonAsync<TResult>(this HttpClient client, String postUrl, String jsonString, Action<HttpContent> config = null, CancellationToken? token = null)
        {
            var respResult = await client.PostJsonAsync(postUrl, jsonString, config, token).ConfigureAwait(false);
            if (!respResult.Success)
            {
                return new XResult<TResult>(default(TResult), respResult.FirstException);
            }

            String respString = null;
            try
            {
                if (respResult.Value.IsSuccessStatusCode)
                {
                    respString = await respResult.Value.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (respString.IsNullOrWhiteSpace())
                    {
                        return new XResult<TResult>(default(TResult), new RemoteException($"目标地址{postUrl}未返回任何数据"));
                    }
                }
                else
                {
                    return new XResult<TResult>(default(TResult), new HttpRequestException($"{((Int32)respResult.Value.StatusCode).ToString()}:{respResult.Value.ReasonPhrase}"));
                }
            }
            catch (Exception ex)
            {
                return new XResult<TResult>(default(TResult), ex);
            }

            try
            {
                var result = JsonConvert.DeserializeObject<TResult>(respString, JsonSerializerSettings);
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
        /// <typeparam name="TResult"></typeparam>
        /// <param name="client"></param>
        /// <param name="postUrl"></param>
        /// <param name="xml"></param>
        /// <param name="config"></param>
        /// <param name="token"></param>
        public async static Task<XResult<TResult>> PostXmlAsync<TResult>(this HttpClient client, String postUrl, String xml, Action<HttpContent> config = null, CancellationToken? token = null)
        {
            var respResult = await client.PostXmlAsync(postUrl, xml, config, token).ConfigureAwait(false);
            if (!respResult.Success)
            {
                return new XResult<TResult>(default(TResult), respResult.FirstException);
            }

            String respString = null;
            try
            {
                if (respResult.Value.IsSuccessStatusCode)
                {
                    respString = await respResult.Value.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (respString.IsNullOrWhiteSpace())
                    {
                        return new XResult<TResult>(default(TResult), new RemoteException($"目标地址{postUrl}未返回任何数据"));
                    }
                }
                else
                {
                    return new XResult<TResult>(default(TResult), new HttpRequestException($"{((Int32)respResult.Value.StatusCode).ToString()}:{respResult.Value.ReasonPhrase}"));
                }
            }
            catch (Exception ex)
            {
                return new XResult<TResult>(default(TResult), ex);
            }

            try
            {
                var result = serializer.Deserialize<TResult>(respString);
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
        /// <typeparam name="TResult"></typeparam>
        /// <param name="client"></param>
        /// <param name="postUrl"></param>
        /// <param name="formData"></param>
        /// <param name="config"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async static Task<XResult<TResult>> PostFormAsync<TResult>(this HttpClient client, String postUrl, IDictionary<String, String> formData, Action<HttpContent> config = null, CancellationToken? token = null)
        {
            var respResult = await client.PostFormAsync(postUrl, formData, config, token).ConfigureAwait(false);
            if (!respResult.Success)
            {
                return new XResult<TResult>(default(TResult), respResult.FirstException);
            }

            String respString = null;
            try
            {
                if (respResult.Value.IsSuccessStatusCode)
                {
                    respString = await respResult.Value.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (respString.IsNullOrWhiteSpace())
                    {
                        return new XResult<TResult>(default(TResult), new RemoteException($"目标地址{postUrl}未返回任何数据"));
                    }
                }
                else
                {
                    return new XResult<TResult>(default(TResult), new HttpRequestException($"{((Int32)respResult.Value.StatusCode).ToString()}:{respResult.Value.ReasonPhrase}"));
                }
            }
            catch (Exception ex)
            {
                return new XResult<TResult>(default(TResult), ex);
            }

            try
            {
                var result = JsonConvert.DeserializeObject<TResult>(respString, JsonSerializerSettings);
                return new XResult<TResult>(result);
            }
            catch (Exception ex)
            {
                return new XResult<TResult>(default(TResult), ex);
            }
        }

        /*==========================================================================================================================*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="postUrl"></param>
        /// <param name="jsonString"></param>
        /// <param name="config"></param>
        /// <param name="token"></param>
        public static XResult<HttpResponseMessage> PostJson(this HttpClient client, String postUrl, String jsonString, Action<HttpContent> config = null, CancellationToken? token = null)
        {
            try
            {
                return client.PostJsonAsync(postUrl, jsonString, config, token).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                return new XResult<HttpResponseMessage>(null, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="postUrl"></param>
        /// <param name="content"></param>
        /// <param name="token"></param>
        public static XResult<HttpResponseMessage> PostForm(this HttpClient client, String postUrl, HttpContent content, CancellationToken? token = null)
        {
            try
            {
                return client.PostFormAsync(postUrl, content, token).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                return new XResult<HttpResponseMessage>(null, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="postUrl"></param>
        /// <param name="formData"></param>
        /// <param name="config"></param>
        /// <param name="token"></param>
        public static XResult<HttpResponseMessage> PostForm(this HttpClient client, String postUrl, IDictionary<String, String> formData, Action<HttpContent> config = null, CancellationToken? token = null)
        {
            try
            {
                return client.PostFormAsync(postUrl, formData, config, token).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                return new XResult<HttpResponseMessage>(null, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="postUrl"></param>
        /// <param name="xml"></param>
        /// <param name="config"></param>
        /// <param name="token"></param>
        public static XResult<HttpResponseMessage> PostXml(this HttpClient client, String postUrl, String xml, Action<HttpContent> config = null, CancellationToken? token = null)
        {
            try
            {
                return client.PostXmlAsync(postUrl, xml, config, token).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                return new XResult<HttpResponseMessage>(null, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUrl"></param>
        /// <param name="parameters"></param>
        /// <param name="token"></param>
        public static XResult<HttpResponseMessage> Get(this HttpClient client, String requestUrl, IDictionary<String, String> parameters, CancellationToken? token = null)
        {
            try
            {
                if (parameters != null && parameters.Count > 0)
                {
                    String queryString = String.Join("&", (from t0 in parameters
                                                           select $"{t0.Key}={HttpUtility.UrlEncode(t0.Value)}"));
                    if (requestUrl.IndexOf('?') < 0)
                    {
                        queryString = "?" + queryString;
                    }

                    requestUrl += queryString;
                }
                return client.GetAsync(requestUrl, token).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                return new XResult<HttpResponseMessage>(null, ex);
            }
        }
        /*==========================================================================================================================*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="postUrl"></param>
        /// <param name="jsonString"></param>
        /// <param name="config"></param>
        /// <param name="token"></param>
        public async static Task<XResult<HttpResponseMessage>> PostJsonAsync(this HttpClient client, String postUrl, String jsonString, Action<HttpContent> config = null, CancellationToken? token = null)
        {
            using (var content = new StringContent(jsonString, Encoding.UTF8, "application/json"))
            {

                if (config != null)
                {
                    config(content);
                }

                return await client.PostAsync(postUrl, content, token).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="postUrl"></param>
        /// <param name="formData"></param>
        /// <param name="config"></param>
        /// <param name="token"></param>
        public async static Task<XResult<HttpResponseMessage>> PostFormAsync(this HttpClient client, String postUrl, IDictionary<String, String> formData, Action<HttpContent> config = null, CancellationToken? token = null)
        {
            using (var content = new FormUrlEncodedContent(formData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                content.Headers.ContentEncoding.Add("UTF-8");

                if (config != null)
                {
                    config(content);
                }

                return await client.PostAsync(postUrl, content, token).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="postUrl"></param>
        /// <param name="xml"></param>
        /// <param name="config"></param>
        /// <param name="token"></param>
        public async static Task<XResult<HttpResponseMessage>> PostXmlAsync(this HttpClient client, String postUrl, String xml, Action<HttpContent> config = null, CancellationToken? token = null)
        {
            using (var content = new StringContent(xml, Encoding.UTF8, "text/xml"))
            {

                if (config != null)
                {
                    config(content);
                }

                return await client.PostAsync(postUrl, content, token).ConfigureAwait(false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="postUrl"></param>
        /// <param name="content"></param>
        /// <param name="token"></param>
        public async static Task<XResult<HttpResponseMessage>> PostFormAsync(this HttpClient client, String postUrl, HttpContent content, CancellationToken? token = null)
        {
            return await client.PostAsync(postUrl, content, token).ConfigureAwait(false);
        }
        /*==========================================================================================================================*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="postUrl"></param>
        /// <param name="postContent"></param>
        /// <param name="token"></param>
        public async static Task<XResult<HttpResponseMessage>> PostAsync(this HttpClient client, String postUrl, HttpContent postContent, CancellationToken? token = null)
        {
            try
            {
                var respMsg = token != null
                      ? await client.PostAsync(postUrl, postContent, token.Value).ConfigureAwait(false)
                      : await client.PostAsync(postUrl, postContent).ConfigureAwait(false);

                return new XResult<HttpResponseMessage>(respMsg);
            }
            catch (Exception ex)
            {
                return new XResult<HttpResponseMessage>(null, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUrl"></param>
        /// <param name="token"></param>
        public async static Task<XResult<HttpResponseMessage>> GetAsync(this HttpClient client, String requestUrl, CancellationToken? token = null)
        {
            try
            {
                var respMsg = token != null ? await client.GetAsync(requestUrl, token.Value).ConfigureAwait(false) : await client.GetAsync(requestUrl).ConfigureAwait(false);
                return new XResult<HttpResponseMessage>(respMsg);
            }
            catch (Exception ex)
            {
                return new XResult<HttpResponseMessage>(null, ex);
            }
        }
    }
}
