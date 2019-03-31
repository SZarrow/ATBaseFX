using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATBase.Core;
using ATBase.OssCore;
using ATBase.OssCore.Common;
using ATBase.OssCore.Domain;

namespace ATBase.OssProvider.AliOss
{
    /// <summary>
    /// 
    /// </summary>
    public class AliImageProcessor : IImageProcess
    {
        private IOssProvider _provider;

        /// <summary>
        /// 
        /// </summary>
        public AliImageProcessor() : this(null) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        public AliImageProcessor(IOssProvider provider)
        {
            if (provider == null)
            {
                provider = new AliOssProvider();
            }

            _provider = provider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public XResult<XOssObject> Process(String bucketName, String objectKey, params ImageProcessParameter[] parameters)
        {
            XResult<XOssObject> result = null;

            if (parameters != null && parameters.Length > 0)
            {
                result = _provider.GetObject(bucketName, objectKey, MergeImageProcessParameters(parameters));
            }
            else
            {
                result = _provider.GetObject(bucketName, objectKey);
            }

            return result;
        }

        private String MergeImageProcessParameters(ImageProcessParameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return String.Empty;
            }

            return "image/" + String.Join("/", (from t0 in parameters
                                                select t0.GetProcessArguments()));
        }
    }
}
