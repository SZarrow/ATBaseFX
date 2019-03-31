using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ATBase.Core;
using ATBase.Logging;
using ATBase.Net;
using ATBase.Payment.Bill99.Domain;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99
{
    /// <summary>
    /// 协议支付API
    /// </summary>
    public class AgreementPaymentApi
    {
        private readonly ILogger _logger;
        private readonly HttpClient _client;
        private readonly XSerializer _serializer;
        private const String NS = "http://www.99bill.com/mas_cnp_merchant_interface";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="merchantId"></param>
        /// <param name="terminalId"></param>
        public AgreementPaymentApi(HttpClient client, String merchantId, String terminalId)
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

            this.MerchantId = merchantId;
            this.TerminalId = terminalId;

            _logger = LogManager.GetLogger();
            _client = client;
            _serializer = new XSerializer();
        }

        /// <summary>
        /// 商户Id
        /// </summary>
        public String MerchantId { get; }
        /// <summary>
        /// 终端Id
        /// </summary>
        public String TerminalId { get; }

        /// <summary>
        /// 签约申请
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="request">请求内容</param>
        public XResult<AgreementApplyResponse> AgreementApply(String requestUrl, AgreementApplyRequest request)
        {
            if (String.IsNullOrWhiteSpace(requestUrl))
            {
                return new XResult<AgreementApplyResponse>(null, new ArgumentNullException(nameof(requestUrl)));
            }

            if (request == null)
            {
                return new XResult<AgreementApplyResponse>(null, new ArgumentNullException(nameof(request)));
            }

            String xml = _serializer.Serialize(request, doc =>
            {
                var indAuthContentEl = doc.Root.Element(XName.Get("indAuthContent", doc.Root.Name.NamespaceName));
                if (indAuthContentEl != null)
                {
                    var terminalIdEl = new XElement("terminalId", this.TerminalId);
                    if (!String.IsNullOrWhiteSpace(indAuthContentEl.Name.NamespaceName))
                    {
                        terminalIdEl.Name = XName.Get(terminalIdEl.Name.LocalName, indAuthContentEl.Name.NamespaceName);
                    }
                    indAuthContentEl.AddFirst(terminalIdEl);

                    var merchantIdEl = new XElement("merchantId", this.MerchantId);
                    if (!String.IsNullOrWhiteSpace(indAuthContentEl.Name.NamespaceName))
                    {
                        merchantIdEl.Name = XName.Get(merchantIdEl.Name.LocalName, indAuthContentEl.Name.NamespaceName);
                    }
                    indAuthContentEl.AddFirst(merchantIdEl);
                }
            });

            _logger.Trace("AGREEPAY_API", "OK", $"{this.GetType().FullName}:AgreementApply()", "快钱协议支付", LogPhase.BEGIN, $"申请绑卡", xml);

            XResult<AgreementApplyResponse> result = null;

            var task = _client.PostXmlAsync<AgreementApplyResponse>(requestUrl, xml).ContinueWith(t0 =>
            {
                if (t0.IsCompleted)
                {
                    if (t0.IsCanceled || t0.IsFaulted)
                    {
                        throw new TaskCanceledException();
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
                return new XResult<AgreementApplyResponse>(null, ex);
            }
        }

        /// <summary>
        /// 签约验证
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="request">请求内容</param>
        public XResult<AgreementBindResponse> AgreementVerify(String requestUrl, AgreementBindRequest request)
        {
            if (String.IsNullOrWhiteSpace(requestUrl))
            {
                return new XResult<AgreementBindResponse>(null, new ArgumentNullException(nameof(requestUrl)));
            }

            if (request == null)
            {
                return new XResult<AgreementBindResponse>(null, new ArgumentNullException(nameof(request)));
            }

            String xml = _serializer.Serialize(request, doc =>
            {
                var indAuthDynVerifyContentEl = doc.Root.Element(XName.Get("indAuthDynVerifyContent", doc.Root.Name.NamespaceName));
                if (indAuthDynVerifyContentEl != null)
                {
                    var merchantIdEl = new XElement("merchantId", this.MerchantId);
                    if (!String.IsNullOrWhiteSpace(indAuthDynVerifyContentEl.Name.NamespaceName))
                    {
                        merchantIdEl.Name = XName.Get(merchantIdEl.Name.LocalName, indAuthDynVerifyContentEl.Name.NamespaceName);
                    }
                    indAuthDynVerifyContentEl.AddFirst(merchantIdEl);
                }
            });

            _logger.Trace("AGREEPAY_API", "OK", $"{this.GetType().FullName}:AgreementVerify()", "快钱协议支付", LogPhase.BEGIN, $"签约绑卡", xml);

            XResult<AgreementBindResponse> result = null;

            var task = _client.PostXmlAsync<AgreementBindResponse>(requestUrl, xml).ContinueWith(t0 =>
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
                return new XResult<AgreementBindResponse>(null, ex);
            }
        }

        /// <summary>
        /// 签约支付
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="request">请求内容</param>
        public XResult<AgreementPayResponse> AgreementPay(String requestUrl, AgreementPayRequest request)
        {
            if (String.IsNullOrWhiteSpace(requestUrl))
            {
                return new XResult<AgreementPayResponse>(null, new ArgumentNullException(nameof(requestUrl)));
            }

            if (request == null)
            {
                return new XResult<AgreementPayResponse>(null, new ArgumentNullException(nameof(request)));
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

            _logger.Trace("AGREEPAY_API", "OK", $"{this.GetType().FullName}:AgreementPay()", "快钱协议支付", LogPhase.BEGIN, $"消费支付", xml);

            XResult<AgreementPayResponse> result = null;

            var task = _client.PostXmlAsync<AgreementPayResponse>(requestUrl, xml).ContinueWith(t0 =>
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
                return new XResult<AgreementPayResponse>(null, ex);
            }
        }

        /// <summary>
        /// 查询交易流水
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="request">请求内容</param>
        public XResult<AgreementQueryResponse> AgreementQuery(String requestUrl, AgreementQueryRequest request)
        {
            if (String.IsNullOrWhiteSpace(requestUrl))
            {
                return new XResult<AgreementQueryResponse>(null, new ArgumentNullException(nameof(requestUrl)));
            }

            if (request == null)
            {
                return new XResult<AgreementQueryResponse>(null, new ArgumentNullException(nameof(request)));
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

            XResult<AgreementQueryResponse> result = null;

            var task = _client.PostXmlAsync<AgreementQueryResponse>(requestUrl, xml).ContinueWith(t0 =>
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
                return new XResult<AgreementQueryResponse>(null, ex);
            }
        }

        /// <summary>
        /// PCI查询
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="request">请求内容</param>
        public XResult<PCIQueryResponse> PCIQuery(String requestUrl, PCIQueryRequest request)
        {
            if (String.IsNullOrWhiteSpace(requestUrl))
            {
                return new XResult<PCIQueryResponse>(null, new ArgumentNullException(nameof(requestUrl)));
            }

            if (request == null)
            {
                return new XResult<PCIQueryResponse>(null, new ArgumentNullException(nameof(request)));
            }

            String xml = _serializer.Serialize(request, doc =>
            {
                var pciQueryContentEl = doc.Root.Element(XName.Get("PciQueryContent", doc.Root.Name.NamespaceName));
                if (pciQueryContentEl != null)
                {
                    var merchantIdEl = new XElement("merchantId", this.MerchantId);
                    if (!String.IsNullOrWhiteSpace(pciQueryContentEl.Name.NamespaceName))
                    {
                        merchantIdEl.Name = XName.Get(merchantIdEl.Name.LocalName, pciQueryContentEl.Name.NamespaceName);
                    }
                    pciQueryContentEl.AddFirst(merchantIdEl);
                }
            });

            XResult<PCIQueryResponse> result = null;

            var task = _client.PostXmlAsync<PCIQueryResponse>(requestUrl, xml).ContinueWith(t0 =>
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
                return new XResult<PCIQueryResponse>(null, ex);
            }
        }

    }
}
