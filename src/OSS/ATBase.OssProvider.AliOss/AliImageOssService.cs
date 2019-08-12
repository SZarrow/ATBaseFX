using System;
using System.Collections.Concurrent;
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
    public class AliImageOssService : IImageOssService
    {
        private readonly IOssProvider _ossProvider;
        /// <summary>
        /// 
        /// </summary>
        public AliImageOssService() : this(null) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        public AliImageOssService(IOssProvider provider)
        {
            if (provider == null)
            {
                provider = new AliOssProvider();
            }

            _ossProvider = provider;
        }
        /// <summary>
        /// 获取带签名的访问地址
        /// </summary>
        /// <param name="bucketName">Bucket的名称</param>
        /// <param name="imageKey">图片的相对路径</param>
        /// <param name="expireInterval">过期时间间隔</param>
        /// <param name="styleName">对图片应用的样式名称</param>
        public XResult<String> GetSignedAccessUrl(String bucketName, String imageKey, TimeSpan expireInterval, String styleName = null)
        {
            if (String.IsNullOrWhiteSpace(bucketName))
            {
                return new XResult<String>(null, new ArgumentNullException("bucketName"));
            }

            if (String.IsNullOrWhiteSpace(imageKey))
            {
                return new XResult<String>(null, new ArgumentNullException("imageKey"));
            }

            IDictionary<String, String> subRes = null;

            if (!String.IsNullOrWhiteSpace(styleName))
            {
                subRes = new Dictionary<String, String>(1);
                subRes["x-oss-process"] = "style/" + styleName;
            }

            return _ossProvider.GetSignedAccessUrl(bucketName, imageKey, expireInterval, subRes);
        }

        /// <summary>
        /// 使用指定的参数来处理图片
        /// </summary>
        /// <param name="image">要处理的图片</param>
        /// <param name="parameters">图片处理参数，可通过<see cref="ImageProcessParameterBuilder"/>来进行构建参数</param>
        public XResult<XOssObject> Process(XOssObject image, params ImageProcessParameter[] parameters)
        {
            if (image == null)
            {
                return new XResult<XOssObject>(null, new ArgumentNullException("image"));
            }

            if (parameters == null || parameters.Length == 0)
            {
                return new XResult<XOssObject>(null, new ArgumentNullException("未指定图片处理参数"));
            }

            XResult<XOssObject> result = null;

            if (parameters != null && parameters.Length > 0)
            {
                result = _ossProvider.GetObject(image.BucketName, image.ObjectKey, MergeImageProcessParameters(parameters));
            }
            else
            {
                result = _ossProvider.GetObject(image.BucketName, image.ObjectKey);
            }

            return result;
        }

        /// <summary>
        /// 使用指定参数来批量处理图片
        /// </summary>
        /// <param name="images">要处理的图片集合</param>
        /// <param name="parameters">图片处理参数，可通过<see cref="ImageProcessParameterBuilder"/>来构建参数</param>
        public XResult<IEnumerable<XOssObject>> Process(IEnumerable<XOssObject> images, params ImageProcessParameter[] parameters)
        {
            if (images == null)
            {
                return new XResult<IEnumerable<XOssObject>>(null, new ArgumentNullException("images"));
            }

            if (parameters == null || parameters.Length == 0)
            {
                return new XResult<IEnumerable<XOssObject>>(null, new ArgumentNullException("parameters"));
            }

            Int32 capacity = images.Count();
            List<XOssObject> succeedObjects = new List<XOssObject>(capacity);
            List<Exception> innerExceptions = new List<Exception>(capacity);

            foreach (var image in images)
            {
                var result = Process(image, parameters);
                if (result.Success)
                {
                    succeedObjects.Add(result.Value);
                }
                else
                {
                    innerExceptions.Add(result.FirstException);
                }
            }

            return new XResult<IEnumerable<XOssObject>>(succeedObjects, new AggregateException(innerExceptions));
        }

        /// <summary>
        /// 上传图片，并用指定参数来处理图片
        /// </summary>
        /// <param name="image">要上传的图片</param>
        /// <param name="parameters">图片处理参数，可通过<see cref="ImageProcessParameterBuilder"/>来构建参数</param>
        public XResult<XOssObject> Upload(XOssObject image, params ImageProcessParameter[] parameters)
        {
            if (image == null)
            {
                return new XResult<XOssObject>(null, new ArgumentNullException("image"));
            }

            if (image.Content == null || image.Content.Length == 0)
            {
                return new XResult<XOssObject>(null, new FormatException("the format of image is invalid"));
            }

            Func<XOssObject, XResult<XOssObject>> callback = null;

            if (parameters != null && parameters.Length > 0)
            {
                callback = img =>
                {
                    return Process(img, parameters);
                };
            }

            return _ossProvider.PutObject(image, callback);
        }

        /// <summary>
        /// 批量上传图片，并用指定参数来处理图片
        /// </summary>
        /// <param name="images">要批量上传的图片</param>
        /// <param name="parameters">图片处理参数，可通过<see cref="ImageProcessParameterBuilder"/>来构建参数</param>
        /// <returns></returns>
        public XResult<IEnumerable<XOssObject>> Upload(IEnumerable<XOssObject> images, params ImageProcessParameter[] parameters)
        {
            if (images == null || images.Count() == 0)
            {
                return new XResult<IEnumerable<XOssObject>>(null, new ArgumentNullException("images"));
            }

            Func<XOssObject, XResult<XOssObject>> callback = null;

            if (parameters != null && parameters.Length > 0)
            {
                callback = img =>
                {
                    return Process(img, parameters);
                };
            }

            return _ossProvider.PutObjects(images, callback);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="image">要删除的图片，构建XOssObject时无需指定Stream</param>
        public XResult<Boolean> Delete(XOssObject image)
        {
            if (image == null)
            {
                return new XResult<Boolean>(false, new ArgumentNullException("image"));
            }

            return _ossProvider.DeleteObject(image);
        }

        /// <summary>
        /// 批量删除图片
        /// </summary>
        /// <param name="bucketName">Bucket的名称</param>
        /// <param name="imageKeys">要删除的图片的相对路径</param>
        public XResult<IEnumerable<String>> Delete(String bucketName, String[] imageKeys)
        {
            if (String.IsNullOrWhiteSpace(bucketName))
            {
                return new XResult<IEnumerable<String>>(null, new ArgumentNullException("bucketName"));
            }

            if (imageKeys == null || imageKeys.Length == 0)
            {
                return new XResult<IEnumerable<String>>(null, new ArgumentNullException("imageKeys"));
            }

            return _ossProvider.DeleteObjects(bucketName, imageKeys);
        }

        /// <summary>
        /// 删除指定目录下的所有文件
        /// </summary>
        /// <param name="bucketName">Bucket的名称</param>
        /// <param name="directoryPath">目录的相对路径</param>
        public XResult<IEnumerable<String>> Delete(String bucketName, String directoryPath)
        {
            if (String.IsNullOrWhiteSpace(bucketName))
            {
                return new XResult<IEnumerable<String>>(null, new ArgumentNullException(nameof(bucketName)));
            }

            if (String.IsNullOrWhiteSpace(directoryPath))
            {
                return new XResult<IEnumerable<String>>(null, new ArgumentNullException(nameof(directoryPath)));
            }

            return _ossProvider.DeleteObjects(bucketName, directoryPath);
        }

        private String MergeImageProcessParameters(ImageProcessParameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return String.Empty;
            }

            return "image" + String.Concat(from p in parameters select p.GetProcessArguments());
        }
    }
}
