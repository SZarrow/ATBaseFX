# OSS - 对象存储服务
[English](README.md) | 中文

OssFx 提供了一种便捷的方式来使用对象存储服务，目前仅支持阿里云OSS。

### 项目依赖

依赖的 nuget 包如下：

* Aliyun.OSS.SDK -v2.8.0

* Newtonsoft.Json -v11.0.2

### 基础调用实例
注意：以下初始化 <code>IOssProvider</code> 接口的方式仅限于demo，实际环境下应使用IoC框架来进行注入，所以请不要迷恋下面的创建方式。

关于 <code>bucketName</code> 和 <code>key</code> 的说明：

* **bucketName** 它可以理解为你的云账户下的一个根目录（文件夹）。

* **key** 它可以理解为根目录下的文件的相对路径。

如果 key 的值仅仅是一个文件名而不包含路径信息的话，那么上传的对象将会被存储到根目录下。

如果 key 的值是一个包含路径的文件名，那么上传的对象将会存储到与路径对应的目录下。
例如：如果key="images/2018/aaa.jpg"，那么"aaa.jpg"将会被存储到云端的"{bucketName}/images/2018/"这个目录下，
其中{bucketName}就是根目录。

### 上传一个对象到云端

* 使用 PutObject() 方法

```csharp
IOssProvider provider = new AliOssProvider();

String bucketName = "sh-oss-1";
String key = "TestPut.jpg";

String filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"images\1.jpg");

using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read))
{
    var result = provider.PutObject(new XOssObject(bucketName, key, fs));
    if (result.Sucess)
    {
        Trace.WriteLine($"Upload Succ.");
    }
    else
    {
        Trace.WriteLine("Upload Fail.");
    }
}
```

* 使用 AppendObject() 方法

```csharp
IOssProvider provider = new AliOssProvider();

String bucketName = "sh-oss-1";
String key = "images/2018/TestAppend.jpg";
            
String filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"images\2.jpg");

XResult<Boolean> result = null;

using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read))
{
    result = provider.AppendObject(new XOssObject(bucketName, key, fs));
}

if (result.Sucess)
{
    Trace.WriteLine("Upload Succ.");
}
else
{
    Trace.WriteLine($"Upload Fail, Reason: {result.ErrorMessage}");
}
```

### 删除云端的一个对象

```csharp
IOssProvider provider = new AliOssProvider();

var result = provider.DeleteObjects("sh-oss-1", new String[] { "TestAppend.jpg", "TestPut.jpg" });
if (result.Sucess)
{
    Trace.WriteLine("Delete Succ.");
}
else
{
    Trace.WriteLine($"Delete Fail, Reason: {result.ErrorMessage}");
}
```

### 获取云端的一个对象

```csharp
IOssProvider provider = new AliOssProvider();

var result = provider.GetObject("sh-oss-1", "TestAppend.jpg");

if (result.Sucess)
{
    XOssObject obj = result.Value;
    Trace.WriteLine($"Get: {obj.Key}");

    //保存对象流到文件
    Stream stream = obj.Content;
    String savePath = Path.Combine(AppContext.BaseDirectory, obj.Key);

    if (stream != null && stream.Length > 0)
    {
        using (var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
        {
            Byte[] buffer = new Byte[2048];
            Int32 read = 0;

            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fs.Write(buffer, 0, read);
            }
        }
    }
}
else
{
    Trace.WriteLine($"Get Fail, Reason: {result.ErrorMessage}");
}
```

### 获取云端的多个对象

```csharp
IOssProvider provider = new AliOssProvider();

var result = provider.GetObjects("sh-oss-1");
if (result.Sucess)
{
    String info = String.Join(", ", (from t0 in result.Value
                                        select t0.Key));
    Trace.WriteLine($"Get all files: {info}");
}

Assert.IsTrue(result.Sucess);

//注意: 目录必须是从根目录开始
//目录不能以"/"开头，但必须以"/"结尾，中间子目录用"/"隔开。
String dir = "images/2018/";

result = provider.GetObjects("sh-oss-1", dir);
if (result.Sucess)
{
    String info = String.Join(", ", (from t0 in result.Value
                                        select t0.Key));
    Trace.WriteLine($"Get all files: {info}");
}

Assert.IsTrue(result.Sucess);
```

### 图片处理

```csharp
[TestMethod]
public void TestImageProcessParameters()
{
    var pb = new ImageProcessParameterBuilder();

    //设置改变大小参数
    pb.CreateResizeImageProcessParameter()
        .SetResizeMode(ResizeMode.LFit)
        .SetWidth(800)
        .SetResizeLimit(false);

    //设置旋转参数
    pb.CreateRotateImageProcessParameter()
        .SetRotateOrientation(true);

    //设置质量参数
    pb.CreateFormatImageProcessParameter()
        .SetRelativeQuality(90)
        .SetFormat(SupportedImageFormat.jpg)
        .SetInterlace(true);

    //添加水印参数
    pb.CreateWaterMarkImageProcessParameter()
        .SetFontText(new UrlSafeBase64String("Hello World"))
        .SetFontType(new UrlSafeBase64String("wqy-microhei"))
        .SetFontSize(80)
        .SetTransparency(100)
        .SetFontColor("ffffff")
        .SetPosition(GridPosition.Center)
        .SetX(10)
        .SetY(10);

    IImageOssService imgServ = new AliImageOssService();

	//假设我们想处理"/sh-oss-1/images/2018/TestAppend.jpg"这个路径的文件
    var ossObject = new XOssObject("sh-oss-1", "images/2018/TestAppend.jpg");

    //开始处理
    var result = imgServ.Process(ossObject, pb.Build());

    //处理成功后写入本地文件 
    if (result.Sucess)
    {
        var imgStream = result.Value.Content;
        String filePath = @"D:\test\3_processed.jpg";

        using (var fs = File.OpenWrite(filePath))
        {
            Byte[] buf = new Byte[1024];
            Int32 read = 0;

            while ((read = imgStream.Read(buf, 0, buf.Length)) > 0)
            {
                fs.Write(buf, 0, read);
            }
        }

        Trace.WriteLine($"Process Succ: {filePath}");
    }
    else
    {
        Trace.WriteLine($"Process Fail: {result.ErrorMessage}");
    }
}
```

### 生成签名过的访问地址

```csharp
[TestMethod]
public void TestGetImageSignedAccessUrl()
{
    IImageOssService imgServ = new AliImageOssService();
    var result = imgServ.GetSignedAccessUrl("sh-oss-1", "TestPut.jpg");
    Assert.IsTrue(result.Sucess);
    String accessUrl = result.Value;
    Trace.WriteLine(accessUrl);
}
```
