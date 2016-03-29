using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace NPFaq.Web.Models
{
    /// <summary>
    /// 带有缩略图的附件
    /// </summary>
    public class MatchAttach : attach
    {
        [DataMember]
        public byte[] Thumbnail { get; set; }
        [DataMember]
        public long Size { get; set; }
        [DataMember]
        public string URL { get; set; }
    }
}