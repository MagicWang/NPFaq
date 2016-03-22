using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NPFaq.Web
{
    /// <summary>
    /// 上传文件处理程序
    /// </summary>
    public class UploadFileHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            var path = context.Request.QueryString["filepath"];
            string filename = context.Request.QueryString["filename"];
            int package = 0;
            int.TryParse(context.Request.QueryString["packageNumber"], out package);
            var dir = context.Server.MapPath("~/" + path);
            var filePath = dir + "\\" + filename;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            if (package == 0 && File.Exists(filePath))
                File.Delete(filePath);

            using (FileStream fs = new FileStream(filePath, FileMode.Append))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = context.Request.InputStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    fs.Write(buffer, 0, bytesRead);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}