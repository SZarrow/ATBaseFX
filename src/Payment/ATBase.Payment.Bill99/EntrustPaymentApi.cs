using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ATBase.Core;
using ATBase.Net;
using ATBase.Payment.Bill99.Domain;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99
{
    /// <summary>
    /// 委托代收API
    /// </summary>
    public class EntrustPaymentApi
    {
        private readonly HttpClient _client;
        private readonly XSerializer _serializer;
        private const String NS = "http://www.99bill.com/mas_cnp_merchant_interface";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="merchantId"></param>
        /// <param name="terminalId"></param>
        public EntrustPaymentApi(HttpClient client, String merchantId, String terminalId)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (String.IsNullOrWhiteSpace(merchantId))
            {
                throw new ArgumentNullException(nameof(merchantId));
            }

            if (String.IsNullOrWhiteSpace(terminalId))
            {
                throw new ArgumentNullException(nameof(terminalId));
            }

            _client = client;
            _serializer = new XSerializer();

            this.MerchantId = merchantId;
            this.TerminalId = terminalId;
        }

        /// <summary>
        /// 
        /// </summary>
        public String MerchantId { get; }
        /// <summary>
        /// 
        /// </summary>
        public String TerminalId { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public XResult<EntrustPayResponse> EntrustPay(String requestUrl, EntrustPayRequest request)
        {
            if (String.IsNullOrWhiteSpace(requestUrl))  
            {
                return new XResult<EntrustPayResponse>(null, new ArgumentNullException(nameof(requestUrl)));
            }

            if (request == null)
            {
                return new XResult<EntrustPayResponse>(null, new ArgumentNullException(nameof(request)));
            }

            String xml = _serializer.Serialize(request, doc =>
            {
                var txnMsgContentEl = doc.Root.Element(XName.Get("TxnMsgContent", doc.Root.Name.NamespaceName));
                if (txnMsgContentEl != null)
                {
                    var terminalIdEl = new XElement("terminalId", this.TerminalId);
                    if (!String.IsNullOrWhiteSpace(txnMsgContentEl.Name.NamespaceName))
                    {
                        terminalIdEl.Name = XName.Get(terminalIdEl.Name.LocalName, txnMsgContentEl.Name.NamespaceName);
                    }
                    txnMsgContentEl.AddFirst(terminalIdEl);

                    var merchantIdEl = new XElement("merchantId", this.MerchantId);
                    if (!String.IsNullOrWhiteSpace(txnMsgContentEl.Name.NamespaceName))
                    {
                        merchantIdEl.Name = XName.Get(merchantIdEl.Name.LocalName, txnMsgContentEl.Name.NamespaceName);
                    }
                    txnMsgContentEl.AddFirst(merchantIdEl);
                }
            });

            WriteLog("EntrustPayRequestData：" + xml);

            XResult<EntrustPayResponse> result = null;

            var task = _client.PostXmlAsync<EntrustPayResponse>(requestUrl, xml).ContinueWith(t0 =>
            {
                if (t0.IsCompleted)
                {
                    if (t0.IsCanceled || t0.IsFaulted)
                    {
                        throw new TaskCanceledException($"RequestUrl:{requestUrl},Content:{xml}");
                    }

                    result = t0.Result;
                }
            });

            try
            {
                task.Wait();
                return result;
            }
            catch (Exception ex)
            {
                return new XResult<EntrustPayResponse>(null, ex);
            }
        }

        /// <summary>
        /// 查询交易流水
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="request">请求内容</param>
        public XResult<EntrustQueryResponse> EntrustQuery(String requestUrl, EntrustQueryRequest request)
        {
            if (String.IsNullOrWhiteSpace(requestUrl))
            {
                return new XResult<EntrustQueryResponse>(null, new ArgumentNullException(nameof(requestUrl)));
            }

            if (request == null)
            {
                return new XResult<EntrustQueryResponse>(null, new ArgumentNullException(nameof(request)));
            }

            String xml = _serializer.Serialize(request, doc =>
            {
                var qryTxnMsgContentEl = doc.Root.Element(XName.Get("QryTxnMsgContent", doc.Root.Name.NamespaceName));
                if (qryTxnMsgContentEl != null)
                {
                    var terminalIdEl = new XElement("terminalId", this.TerminalId);
                    if (!String.IsNullOrWhiteSpace(qryTxnMsgContentEl.Name.NamespaceName))
                    {
                        terminalIdEl.Name = XName.Get(terminalIdEl.Name.LocalName, qryTxnMsgContentEl.Name.NamespaceName);
                    }
                    qryTxnMsgContentEl.AddFirst(terminalIdEl);

                    var merchantIdEl = new XElement("merchantId", this.MerchantId);
                    if (!String.IsNullOrWhiteSpace(qryTxnMsgContentEl.Name.NamespaceName))
                    {
                        merchantIdEl.Name = XName.Get(merchantIdEl.Name.LocalName, qryTxnMsgContentEl.Name.NamespaceName);
                    }
                    qryTxnMsgContentEl.AddFirst(merchantIdEl);
                }
            });

            XResult<EntrustQueryResponse> result = null;

            var task = _client.PostXmlAsync<EntrustQueryResponse>(requestUrl, xml).ContinueWith(t0 =>
            {
                if (t0.IsCompleted)
                {
                    if (t0.IsCanceled || t0.IsFaulted)
                    {
                        throw new TaskCanceledException($"RequestUrl:{requestUrl},Content:{xml}");
                    }

                    result = t0.Result;
                }
            });

            try
            {
                task.Wait();
                return result;
            }
            catch (Exception ex)
            {
                return new XResult<EntrustQueryResponse>(null, ex);
            }
        }

        private void WriteLog(String content)
        {
            String logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"log\{DateTime.Now.ToString("yyyy-MM-dd")}.txt");
            File.AppendAllText(logFile, Environment.NewLine + Environment.NewLine + content);
        }
    }
}
