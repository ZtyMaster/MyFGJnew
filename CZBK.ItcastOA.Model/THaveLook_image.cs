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
    
    public partial class THaveLook_image
    {
        public long ID { get; set; }
        public Nullable<int> del { get; set; }
        public long HaveLook_ID { get; set; }
        public string Str_image { get; set; }
    
        public virtual THavelook THavelook { get; set; }
    }
}
