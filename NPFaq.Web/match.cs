//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace NPFaq.Web
{
    using System;
    using System.Collections.Generic;
    
    public partial class match
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string Status { get; set; }
        public Nullable<int> PublisherID { get; set; }
        public string Level { get; set; }
        public string Sponsor { get; set; }
        public Nullable<System.DateTime> StartSign { get; set; }
        public Nullable<System.DateTime> EndSign { get; set; }
        public string Address { get; set; }
    }
}
