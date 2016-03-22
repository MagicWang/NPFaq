using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace NPFaq.Web.Models
{
    /// <summary>
    /// 提供客户端追踪实体状态的接口
    /// </summary>
    interface IBaseMode
    {
        EntityState State { get; set; }
    }
}