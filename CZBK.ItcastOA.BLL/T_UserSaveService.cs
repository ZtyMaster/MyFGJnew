using CZBK.ItcastOA.IBLL;
using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZBK.ItcastOA.Model.Enum;
using System.Threading.Tasks;


namespace CZBK.ItcastOA.BLL
{
    public partial class T_UserSaveService : BaseService<T_UserSave>,IT_UserSaveService
    {
        /// <summary>
        /// 多条件搜索用户信息
        /// </summary>
        /// <param name="userInfoSearchParam"></param>
        /// <returns></returns>
        public IQueryable<T_UserSave> LoadSearchEntities(Model.SearchParam.UserInfoParam userInfoSearchParam)
        {
            short Delflag = (short)DelFlagEnum.Normarl;
            
            var temp = this.GetCurrentDbSession.T_UserSaveDal.LoadEntities(x => x.UserID == userInfoSearchParam.C_id && x.delflag == Delflag);
            if (!userInfoSearchParam.IsMaster)
            {
                temp=this.GetCurrentDbSession.T_UserSaveDal.LoadEntities(x=>x.UserInfo.MasterID== userInfoSearchParam.C_id && x.delflag == Delflag);
            }
            if (!string.IsNullOrEmpty(userInfoSearchParam.Items))
            {
                int thiid = int.Parse(userInfoSearchParam.Items);
                temp = temp.Where<T_UserSave>(u => u.Items== thiid);
            }
            if (!string.IsNullOrEmpty(userInfoSearchParam.Str))
            {
                temp = temp.Where<T_UserSave>(u => u.TextName.Contains(userInfoSearchParam.Str) || u.Address.Contains(userInfoSearchParam.Str)||u.PresonPhoto.Contains(userInfoSearchParam.Str));
            }
            if (!string.IsNullOrEmpty(userInfoSearchParam.Zhuanxiu))
            {
                int thiid = int.Parse(userInfoSearchParam.Zhuanxiu);
                temp = temp.Where<T_UserSave>(u => u.ZhuanxiuStrID== thiid);
            }
            if (!string.IsNullOrEmpty(userInfoSearchParam.Pingmu) && userInfoSearchParam.Pingmu.Trim() != "0")
            {
                int thiid = int.Parse(userInfoSearchParam.Pingmu);

                temp = temp.Where<T_UserSave>(u => u.MianjiID == thiid);
            }
            if (!string.IsNullOrEmpty(userInfoSearchParam.money) && userInfoSearchParam.money.Trim() != "0")
            {
                int thiid = int.Parse(userInfoSearchParam.money);

                temp = temp.Where<T_UserSave>(u => u.SumMoneyID == thiid);
            }
            if (!string.IsNullOrEmpty(userInfoSearchParam.Tingshi) && userInfoSearchParam.Tingshi.Trim() != "0")
            {
                int thiid = int.Parse(userInfoSearchParam.Tingshi);

                temp = temp.Where<T_UserSave>(u => u.HuxingStrID == thiid);
            }
            userInfoSearchParam.TotalCount = temp.Count();
            return temp.OrderByDescending<T_UserSave, DateTime>(u => u.AddTime).Skip<T_UserSave>((userInfoSearchParam.PageIndex - 1) * userInfoSearchParam.PageSize).Take<T_UserSave>(userInfoSearchParam.PageSize);

        }



        /// <summary>
        /// 多条件搜索分享信息
        /// </summary>
        /// <param name="pps"></param>
        /// <returns></returns>
        public IQueryable<T_UserSave> LoadFenx(Model.SearchParam.UserInfoParam pps)
        {
            short Delflag = (short)DelFlagEnum.Normarl;

            var AllUser = this.GetCurrentDbSession.UserInfoDal.LoadEntities(x => x.ID == pps.MasterID).FirstOrDefault();
            // var Ttemp= AllUser.Select(x => x.T_UserSave).DefaultIfEmpty();
            var temp = this.GetCurrentDbSession.T_UserSaveDal.LoadEntities(x => x.UserID == pps.C_id && x.delflag == Delflag);
            if (!pps.IsMaster)
            {
                temp = this.GetCurrentDbSession.T_UserSaveDal.LoadEntities(x => x.UserInfo.MasterID == pps.C_id && x.delflag == Delflag);
            }
            //开启
            if (AllUser.GeRenSaveOpen == 0) {
                temp = this.GetCurrentDbSession.T_UserSaveDal.LoadEntities(x => x.UserInfo.MasterID == AllUser.ID && x.delflag == Delflag);
            }
            if (!string.IsNullOrEmpty(pps.Items))
            {
                int thiid = int.Parse(pps.Items);
                temp = temp.Where<T_UserSave>(u => u.Items == thiid);
            }
            if (!string.IsNullOrEmpty(pps.Str))
            {
                temp = temp.Where<T_UserSave>(u => u.TextName.Contains(pps.Str) || u.Address.Contains(pps.Str) || u.PresonPhoto.Contains(pps.Str));
            }
            if (!string.IsNullOrEmpty(pps.Zhuanxiu))
            {
                int thiid = int.Parse(pps.Zhuanxiu);
                temp = temp.Where<T_UserSave>(u => u.ZhuanxiuStrID == thiid);
            }
            if (!string.IsNullOrEmpty(pps.Pingmu) && pps.Pingmu.Trim() != "0")
            {
                int thiid = int.Parse(pps.Pingmu);

                temp = temp.Where<T_UserSave>(u => u.MianjiID == thiid);
            }
            if (!string.IsNullOrEmpty(pps.money) && pps.money.Trim() != "0")
            {
                int thiid = int.Parse(pps.money);

                temp = temp.Where<T_UserSave>(u => u.SumMoneyID == thiid);
            }
            if (!string.IsNullOrEmpty(pps.Tingshi) && pps.Tingshi.Trim() != "0")
            {
                int thiid = int.Parse(pps.Tingshi);

                temp = temp.Where<T_UserSave>(u => u.HuxingStrID == thiid);
            }
            pps.TotalCount = temp.Count();
            return temp.OrderByDescending<T_UserSave, DateTime>(u => u.AddTime).Skip<T_UserSave>((pps.PageIndex - 1) * pps.PageSize).Take<T_UserSave>(pps.PageSize);

        }
    }
}
