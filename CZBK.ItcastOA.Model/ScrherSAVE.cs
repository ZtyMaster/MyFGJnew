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
    
    public partial class ScrherSAVE
    {
        public long ID { get; set; }
        public int addUserID { get; set; }
        public Nullable<decimal> Strmoney { get; set; }
        public Nullable<decimal> Stpmoney { get; set; }
        public Nullable<decimal> Srtmm { get; set; }
        public Nullable<decimal> Stpmm { get; set; }
        public Nullable<int> HxID { get; set; }
        public string Quyu { get; set; }
        public string CName { get; set; }
        public Nullable<short> DEL { get; set; }
        public Nullable<System.DateTime> Addtime { get; set; }
        public Nullable<System.DateTime> Edittime { get; set; }
        public string Zhuangxiu { get; set; }
        public string Bak { get; set; }
    
        public virtual UserInfo UserInfo { get; set; }
    }
}
