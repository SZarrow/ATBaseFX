using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATBase.Core;
using ATBase.OssCore.Common;
using ATBase.OssCore.Domain;

namespace ATBase.OssCore
{
    /// <summary>
    /// 图片处理接口，图片处理支持的格式：jpg、png、bmp、gif、webp、tiff。
    /// </summary>
    public interface IImageProcess
    {
        /// <summary>
        /// 根据参数处理指定图片
        /// </summary>
        /// <param name="bucketName">图片所在的bucket的名称</param>
        /// <param name="objectKey">图片的路径</param>
        /// <param name="parameters">处理参数</param>
        XResult<XOssObject> Process(String bucketName, String objectKey, params ImageProcessParameter[] parameters);
    }
}
