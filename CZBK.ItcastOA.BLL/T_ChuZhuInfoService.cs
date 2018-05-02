using CZBK.ItcastOA.IBLL;
using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZBK.ItcastOA.Model.Enum;
using System.Threading.Tasks;
using CZBK.ItcastOA.Model.SearchParam;

namespace CZBK.ItcastOA.BLL
{
    public partial class T_ChuZhuInfoService : BaseService<T_ChuZhuInfo>, IT_ChuZhuInfoService
    {
        public IQueryable<T_ChuZhuInfo> LoadSearchEntities(UserInfoParam uip)
        {
            
            var temp = this.GetCurrentDbSession.T_ChuZhuInfoDal.LoadEntities(x => x.CityID == uip.CityID);
            if (uip.quyu != "no")
            {
                temp = temp.Where<T_ChuZhuInfo>(x => x.LaiYuan != "58");
            }
            if (!string.IsNullOrEmpty(uip.money) ) {
                //数据库查询。   ID  400-800
                var id = Convert.ToInt32(uip.money);
                var itemInfo = this.GetCurrentDbSession.T_ItemsDal.LoadEntities(x => x.ID == id).FirstOrDefault();
                if(itemInfo!= null)
                {
                    if (itemInfo.StrID != 0)
                    {
                        if (itemInfo.StrID == 1)
                        {
                            decimal money = Convert.ToDecimal(itemInfo.Str_val);
                            temp = temp.Where<T_ChuZhuInfo>(x => x.Money <= money && x.LaiYuan != "58");
                        }
                        else if (itemInfo.StrID == 10)
                        {
                            decimal money = Convert.ToDecimal(itemInfo.Str_val);
                            temp = temp.Where<T_ChuZhuInfo>(x => x.Money >= money && x.LaiYuan != "58");
                        }
                        else
                        {
                            string[] ary = itemInfo.Str_val.Split('-');
                            decimal min = Convert.ToDecimal(ary[0]), max = Convert.ToDecimal(ary[1]);
                            temp = temp.Where<T_ChuZhuInfo>(x => x.Money >= min && x.Money <= max && x.LaiYuan != "58");
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(uip.Pingmu))
            {
                var id = Convert.ToInt32(uip.Pingmu);
                var itemInfo = this.GetCurrentDbSession.T_ItemsDal.LoadEntities(x => x.ID == id).FirstOrDefault();
                if (itemInfo != null)
                {
                    if (itemInfo.StrID != 0)
                    {
                        if (itemInfo.StrID == 1)
                        {
                            decimal pingmi = Convert.ToDecimal(itemInfo.Str_val);
                            temp = temp.Where<T_ChuZhuInfo>(x => x.PingMi <= pingmi && x.LaiYuan != "58");
                        }
                        else if (itemInfo.StrID == 10)
                        {
                            decimal pingmi = Convert.ToDecimal(itemInfo.Str_val);
                            temp = temp.Where<T_ChuZhuInfo>(x => x.PingMi >= pingmi && x.LaiYuan != "58");
                        }
                        else
                        {
                            string[] ary = itemInfo.Str_val.Split('-');
                            decimal min = Convert.ToDecimal(ary[0]), max = Convert.ToDecimal(ary[1]);
                            temp = temp.Where<T_ChuZhuInfo>(x => x.PingMi >= min && x.PingMi <= max && x.LaiYuan != "58");
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(uip.Str))
            {
                temp = temp.Where<T_ChuZhuInfo>(u => u.Addess.Contains(uip.Str)|| u.ChuZhuName.Contains(uip.Str) || u.Bak.Contains(uip.Str));
            }
            if (uip.Isee) {
                var czdata = this.GetCurrentDbSession.SeeQzCzDal.LoadEntities(x => x.UserID == uip.C_id&&x.ChuZhuID!=null);
                temp = czdata.Select(x => x.T_ChuZhuInfo);
            }

            uip.TotalCount = temp.Count();
            return temp.OrderByDescending<T_ChuZhuInfo, DateTime?>(u => u.FbTime).Skip<T_ChuZhuInfo>((uip.PageIndex - 1) * uip.PageSize).Take<T_ChuZhuInfo>(uip.PageSize);
        }
    }
}
