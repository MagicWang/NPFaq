using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NPFaq.Web.Utils
{
    public static class ContextHelper
    {
        /// <summary>
        /// 创建禁用代理和延迟加载的数据上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateContext<T>() where T : DbContext, new()
        {
            var t = new T();
            t.Configuration.LazyLoadingEnabled = false;
            t.Configuration.ProxyCreationEnabled = false;
            return t;
        }
    }
}