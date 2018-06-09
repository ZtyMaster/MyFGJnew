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
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    
    public partial class UserInfo
    {
        public UserInfo()
        {
            this.R_UserInfo_ActionInfo = new HashSet<R_UserInfo_ActionInfo>();
            this.Department = new HashSet<Department>();
            this.RoleInfo = new HashSet<RoleInfo>();
            this.T_SaveHtmlData = new HashSet<T_SaveHtmlData>();
            this.UserInfo_City = new HashSet<UserInfo_City>();
            this.T_SeeClickPhoto = new HashSet<T_SeeClickPhoto>();
            this.T_UserClick = new HashSet<T_UserClick>();
            this.T_UserSave = new HashSet<T_UserSave>();
            this.T_SaveHtmlData1 = new HashSet<T_SaveHtmlData>();
            this.T_SaveHtmlData11 = new HashSet<T_SaveHtmlData>();
            this.T_YxPerson = new HashSet<T_YxPerson>();
            this.T_FGJHtmlData = new HashSet<T_FGJHtmlData>();
            this.T_ZhuaiJiaBak = new HashSet<T_ZhuaiJiaBak>();
            this.T_ChuZhuInfo = new HashSet<T_ChuZhuInfo>();
            this.T_QiuZhuQiuGou = new HashSet<T_QiuZhuQiuGou>();
            this.T_UpHerfCity = new HashSet<T_UpHerfCity>();
            this.SeeQzCzs = new HashSet<SeeQzCz>();
            this.TLoginbaks = new HashSet<TLoginbak>();
            this.ScrherSAVEs = new HashSet<ScrherSAVE>();
            this.T_ScehMiShu = new HashSet<T_ScehMiShu>();
            this.T_AppOpen = new HashSet<T_AppOpen>();
            this.WxUsers = new HashSet<WxUser>();
            this.Ttexts = new HashSet<Ttext>();
        }
    
        public int ID { get; set; }
        public string UName { get; set; }
        public string UPwd { get; set; }
        public System.DateTime SubTime { get; set; }
        public short DelFlag { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public string Remark { get; set; }
        public string Sort { get; set; }
        public System.DateTime OverTime { get; set; }
        public Nullable<int> UserXiaoHao { get; set; }
        public Nullable<int> Click { get; set; }
        public Nullable<bool> ThisMastr { get; set; }
        public Nullable<int> MasterID { get; set; }
        public string Login_now { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<decimal> Umoney { get; set; }
        public Nullable<long> YxPersonID { get; set; }
        public string khName { get; set; }
        public string KhPhoto { get; set; }
        public Nullable<int> GeRenSaveOpen { get; set; }
        public Nullable<int> Bak1 { get; set; }
        public string Bak2 { get; set; }
        public string Bak3 { get; set; }

        [JsonIgnore]
        public virtual ICollection<R_UserInfo_ActionInfo> R_UserInfo_ActionInfo { get; set; }
        [JsonIgnore]
        public virtual ICollection<Department> Department { get; set; }
        [JsonIgnore]
        public virtual ICollection<RoleInfo> RoleInfo { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_SaveHtmlData> T_SaveHtmlData { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserInfo_City> UserInfo_City { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_SeeClickPhoto> T_SeeClickPhoto { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_UserClick> T_UserClick { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_UserSave> T_UserSave { get; set; }
        [JsonIgnore]
        public virtual T_City T_City { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_SaveHtmlData> T_SaveHtmlData1 { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_SaveHtmlData> T_SaveHtmlData11 { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_YxPerson> T_YxPerson { get; set; }
        [JsonIgnore]
        public virtual T_YxPerson T_YxPerson1 { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_FGJHtmlData> T_FGJHtmlData { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_ZhuaiJiaBak> T_ZhuaiJiaBak { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_ChuZhuInfo> T_ChuZhuInfo { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_QiuZhuQiuGou> T_QiuZhuQiuGou { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_UpHerfCity> T_UpHerfCity { get; set; }
        [JsonIgnore]
        public virtual ICollection<SeeQzCz> SeeQzCzs { get; set; }
        [JsonIgnore]
        public virtual ICollection<TLoginbak> TLoginbaks { get; set; }
        [JsonIgnore]
        public virtual ICollection<ScrherSAVE> ScrherSAVEs { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_ScehMiShu> T_ScehMiShu { get; set; }
        [JsonIgnore]
        public virtual ICollection<T_AppOpen> T_AppOpen { get; set; }
        [JsonIgnore]
        public virtual ICollection<WxUser> WxUsers { get; set; }
        [JsonIgnore]
        public virtual ICollection<Ttext> Ttexts { get; set; }
    }
}
