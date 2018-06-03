using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.Enum;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class MyhtmlInfoController : BaseController
    {
        //
        // GET: /MyhtmlInfo/
        IBLL.IT_CityService T_CityService { get; set; }
        IBLL.IT_SaveHtmlDataService T_SaveHtmlDataService { get; set; }
        IBLL.IT_FGJHtmlDataService T_FGJHtmlDataService { get; set; }
        IBLL.IUserInfo_CityService UserInfo_CityService { get; set; }
        IBLL.IT_BiaoJiInfoService T_BiaoJiInfoService { get; set; }
        IBLL.IT_ItemsService T_ItemsService { get; set; }
        IBLL.IUserInfoService UserInfoService { get; set; }
        short Delflag = (short)DelFlagEnum.Normarl;
        short GongG = (short)GongGEnum.Show;
        public ActionResult Index()
        {

            var CityID = UserInfo_CityService.LoadEntities(x => x.UserInfo_ID == LoginUser.ID).FirstOrDefault();
            var City = T_CityService.LoadEntities(x => x.ID == CityID.T_City_ID).FirstOrDefault().T_Quyu.ToList();
            var Items = T_ItemsService.LoadEntities(x => x.DelFlag == null).ToList();
            var ItemsCount = Items != null ? Items.Distinct(new FastPropertyComparer<T_Items>("Bakstr")).ToList() : null;
            ViewBag.ItemsCount = ItemsCount.Count;
            ViewBag.City = City;
            ViewBag.Items = Items;




            var BijiaoList = T_BiaoJiInfoService.LoadEntities(u => u.DelFlag == Delflag).ToList();

            ViewBag.Bj = BijiaoList;
            return View();
        }
        public ActionResult GetHref()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 15;
            var totalCount = int.MaxValue;
            var CityID = UserInfo_CityService.LoadEntities(x => x.UserInfo_ID == LoginUser.ID).FirstOrDefault();
            string Str = Request["Str"];
            bool GeOr = Request["GeOr"] == null ? false : Convert.ToBoolean(Request["GeOr"]);
            //构建搜索条件          
            UserInfoParam userInfoParam = new UserInfoParam()
            {
                C_id = LoginUser.ID,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                Str = Str,
                Tval = Request["Tval"],
                Isee = GeOr,
                MasterID = (int)LoginUser.MasterID
            };
            SetInfoParam(userInfoParam, T_ItemsService);
            //找到所有该人员信息
            var savelist = T_SaveHtmlDataService.LoadPageEntities<DateTime>(pageIndex, pageSize, out totalCount, x => x.UserID == LoginUser.ID && x.GongGong == GongG && x.DelFlag == Delflag, x => x.SaveTime, false).DefaultIfEmpty();
            var ActionInfoList = T_FGJHtmlDataService.LoadSearchFrist(userInfoParam, false, true);

            var temp = GeOr ? from a in ActionInfoList
                              select new ThisMyclass
                              {
                                  ID = a.ID,
                                  HLName = a.HLName,
                                  Image_str = (a.Image_str.IndexOf("有") >= 0 ? "有" : "无"),
                                  FbTime = a.FbTime,
                                  PersonName = a.PersonName,
                                  Address = a.Address,
                                  photo = a.photo,
                                  Laiyuan = a.Laiyuan,
                                  FwSumMoney = a.FwSumMoney,
                                  FwHuXing = a.FwHuXing,
                                  FwLoucheng = a.FwLoucheng,
                                  FwZhuangxiu = a.FwZhuangxiu,
                                  FwChaoxiang = a.FwChaoxiang,
                                  FwNianxian = a.FwNianxian,
                                  FwMianji = a.FwMianji,
                                  FwBiaoJi = (int?)0,
                                  FwBiaoJiID = (long)0,
                                  FwColors = "",
                                  Fwpingmi = a.Pingmi_int,
                                  FwPingmiMoney = a.Money_int,
                                  Fmimage = a.Image_str,

                              } : from a in ActionInfoList
                                  from b in savelist
                                  where b.HtmldataID == a.ID
                                  select new ThisMyclass
                                  {
                                      ID = a.ID,
                                      HLName = a.HLName,
                                      Image_str = (a.Image_str.IndexOf("有") >= 0 ? "有" : "无"),
                                      FbTime = a.FbTime,
                                      PersonName = a.PersonName,
                                      Address = a.Address,
                                      photo = a.photo,
                                      Laiyuan = a.Laiyuan,
                                      FwSumMoney = a.FwSumMoney,
                                      FwHuXing = a.FwHuXing,
                                      FwLoucheng = a.FwLoucheng,
                                      FwZhuangxiu = a.FwZhuangxiu,
                                      FwChaoxiang = a.FwChaoxiang,
                                      FwNianxian = a.FwNianxian,
                                      FwMianji = a.FwMianji,
                                      FwBiaoJi = b.BiaoJiId,
                                      FwBiaoJiID = b.ID,
                                      FwColors = b.T_BiaoJiInfo.Colors,
                                      Fwpingmi = a.Pingmi_int,
                                      FwPingmiMoney = a.Money_int,
                                      Fmimage = a.Image_str
                                  };

            List<ThisMyclass> ltm = new List<ThisMyclass>();
            if (GeOr)
            {
                var Masters = UserInfoService.LoadEntities(x => x.MasterID == LoginUser.MasterID).Select(x=>x.T_SaveHtmlData);
                foreach (var p in temp)
                {
                    var pp = Masters.Select(x => x.Where(z => z.HtmldataID == p.ID).DefaultIfEmpty());                   
                    ThisMyclass tmc = new ThisMyclass();
                    tmc = p;
                    foreach (var tmp in pp)
                    {
                        if (tmp.First() != null)
                        {
                            tmc.UserID = tmp.First().UserID;
                            tmc.UserName = tmp.First().UserInfo.UName;
                        }
                    }
                    tmc.MyID = LoginUser.ID;
                    if (tmc.MyID != tmc.UserID) {
                        ltm.Add(tmc);
                    }
                    
                }
            }
            else {
                ltm = temp.ToList();
            }
            return Json(new { rows = ltm, total = userInfoParam.TotalCount }, JsonRequestBehavior.AllowGet);
        }

        public static void SetInfoParam(UserInfoParam userInfoParam, IBLL.IT_ItemsService T_ItemsService)
        {
            if (userInfoParam.Tval != null)
            {
                #region MyRegion
                string str = userInfoParam.Tval.ToString();
                string[] list = str.Split(',');
                for (int i = 0; i < list.Length; i++)
                {
                    string[] ti = list[i].Split('>');
                    int cc = int.Parse(ti[0]);
                    var this_i = T_ItemsService.LoadEntities(x => x.ID == cc).FirstOrDefault();
                    switch (this_i.Icons)
                    {
                        case 0:
                            userInfoParam.money = ti[1].Trim() == "0" ? null : this_i.StrID.ToString();
                            break;
                        case 1:
                            userInfoParam.Pingmu = ti[1].Trim() == "0" ? null : this_i.StrID.ToString();
                            break;
                        case 2:
                            userInfoParam.Tingshi = ti[1].Trim() == "0" ? null : this_i.StrID.ToString();
                            break;
                        case 3:
                            userInfoParam.Zhuanxiu = ti[1].Trim() == "0" ? null : ti[1].ToString().Trim() == "装修" ? null : ti[1];
                            break;
                    }
                }
                #endregion
            }
        }

        public ActionResult EditBiaoJi()
        {
            string spik = Request["strId"].ToString();
            string[] thisS = spik.Split(',');
            int BiaojiInfoID = int.Parse(Request["BiaojiInfoID"]);
            foreach (string s in thisS)
            {
                int id = int.Parse(s);
                T_SaveHtmlData thd = T_SaveHtmlDataService.LoadEntities(x => x.ID == id).FirstOrDefault();
                thd.BiaoJiId = BiaojiInfoID;
                thd.BiaoJiTime = DateTime.Now;
                T_SaveHtmlDataService.EditEntity(thd);

            }
            return Content("ok");
        }
        #region 修改单价
        public ActionResult EditMoney()
        {
            var id = Request["ID"] != null ? Convert.ToInt64(Request["ID"]) : 0;
            var EditSumMoney = Convert.ToDecimal(Request["EditSumMoney"]);
            var Tfg = T_FGJHtmlDataService.LoadEntities(x => x.ID == id).FirstOrDefault();
            Tfg.FwSumMoney = (EditSumMoney / 10000).ToString() + "万元";
            Tfg.SumMoneyID = GetMoney(Tfg.FwSumMoney);
            Tfg.Money_int = Convert.ToDecimal(Math.Round(Convert.ToDouble(EditSumMoney / Tfg.Pingmi_int), 2));
            if (T_FGJHtmlDataService.EditEntity(Tfg))
            {
                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            else
            { return Json("no", JsonRequestBehavior.AllowGet); }


        }
        #endregion
        private static int GetMoney(string da)
        {
            if (da.IndexOf("万") < 0)
            {
                return 0;
            }
            string str = da.Remove(da.IndexOf("万"));
            str = str.Replace("万", string.Empty);
            str = str.Replace("以下", string.Empty);
            str = str.Replace("以上", string.Empty);
            double mj = Convert.ToDouble(str);
            if (mj < 30)
            {
                return 1;
            }
            else if (mj >= 30 && mj < 40)
            {
                return 2;
            }
            else if (mj >= 40 && mj < 50)
            {
                return 3;
            }
            else if (mj >= 50 && mj < 60)
            {
                return 4;
            }
            else if (mj >= 60 && mj < 80)
            {
                return 5;
            }
            else if (mj >= 80 && mj < 100)
            {
                return 6;
            }
            else if (mj >= 100 && mj < 120)
            {
                return 7;
            }
            else if (mj >= 120 && mj < 160)
            {
                return 8;
            }
            else if (mj >= 160 && mj < 200)
            {
                return 9;
            }
            else
            {
                return 10;
            }
        }


    }

    public class ThisMyclass{
      public long ID { get; set; }
        public string HLName { get; set; }
        public string Image_str { get; set; }
        public DateTime FbTime { get; set; }
        public string PersonName { get; set; }
        public string Address { get; set; }
        public string photo { get; set; }
        public string Laiyuan { get; set; }
        public string FwSumMoney { get; set; }
        public string FwHuXing { get; set; }
        public string FwLoucheng { get; set; }
        public string FwZhuangxiu { get; set; }
        public string FwChaoxiang { get; set; }
        public string FwNianxian { get; set; }
        public string FwMianji { get; set; }
        public int? FwBiaoJi { get; set; }
        public long FwBiaoJiID { get; set; }
        public string FwColors { get; set; }
        public decimal? Fwpingmi { get; set; }
        public decimal? FwPingmiMoney { get; set; }
        public string Fmimage { get; set; }
        public int UserID { get; set; }
        public int MyID { get; set; }
        public string UserName { get; set; }

    }
}
