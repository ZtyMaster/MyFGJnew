//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CZBK.ItcastOA.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_AppOpen
    {
        public long ID { get; set; }
        public int UserID { get; set; }
        public bool AppBoll { get; set; }
        public short del { get; set; }
        public System.DateTime AddTime { get; set; }
        public System.DateTime OverTime { get; set; }
        public string AppId { get; set; }
    
        public virtual UserInfo UserInfo { get; set; }
    }
}