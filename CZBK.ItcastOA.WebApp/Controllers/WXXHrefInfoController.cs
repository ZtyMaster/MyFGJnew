using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class WXXHrefInfoController : Controller
    {
        //
        // GET: /WXXHrefInfo/
        IBLL.IT_FGJHtmlDataService T_FGJHtmlDataService { get; set; }

        IBLL.IUserInfo_CityService UserInfo_CityService { get; set; }
        IBLL.IT_SaveHtmlDataService T_SaveHtmlDataService { get; set; }
        IBLL.IT_ItemsService T_ItemsService { get; set; }
        IBLL.IT_CityService T_CityService { get; set; }
        IBLL.IActionInfoService ActionInfoService { get; set; }
        IBLL.IT_SeeClickPhotoService T_SeeClickPhotoService { get; set; }
        IBLL.IT_UserClickService T_UserClickService { get; set; }
        IBLL.IUserInfoService UserInfoService { get; set; }
        IBLL.IT_BoolItemService T_BoolItemService { get; set; }
        IBLL.IT_ZhuaiJiaBakService T_ZhuaiJiaBakService { get; set; }
        IBLL.IScrherSAVEService ScrherSAVEService { get; set; }
        IBLL.IT_ScehMiShuService T_ScehMiShuService { get; set; }
        IBLL.IGongGaoService GongGaoService { get; set; }
        IBLL.IT_QiuZhuQiuGouService T_QiuZhuQiuGouService { get; set; }
        IBLL.IT_ChuZhuInfoService T_ChuZhuInfoService { get; set; }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetHrefForCZ()
        {
            var cityStr = Request["cityStr"];
            var CityID = T_CityService.LoadEntities(x => x.City_str == cityStr).FirstOrDefault();
            UserInfoParam uip = new UserInfoParam();
            uip.PageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            uip.PageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            uip.TotalCount = 0;
            uip.CityID = CityID.ID;
            uip.Str = Request["Str"] != null ? Request["Str"] : null;
            uip.Isee = Request["Isee"] != null ? Convert.ToBoolean(Request["Isee"]) : false;
            uip.quyu = Request["IsAPPsche"] != null ? Request["IsAPPsche"].ToString() : "no";
            var temp = T_ChuZhuInfoService.LoadSearchEntities(uip);
            var Rtemo = from a in temp
                        select new
                        {
                            ID = a.ID,
                            TextName = a.ChuZhuName,
                            Pername = a.LianXiPerson,
                            Photo = a.LianXiPhoto,
                            Address = a.Addess,
                            Fbtime = a.FbTime,
                            Money = a.Money,
                            PinMi = a.PingMi,
                            Bak = a.Bak,
                            Images = a.Images,
                            ClickCount = a.SeeQzCzs.Count(),
                            Laiyuan = a.LaiYuan,
                            IsQZ = "CZ",
                            url = a.ChuZhuHref,
                        };

            return Json(new { rows = Rtemo, total = uip.TotalCount }, JsonRequestBehavior.AllowGet);
    }
        public ActionResult GetHrefForCS()
        {
            var cityStr = Request["cityStr"];
            var CityID = T_CityService.LoadEntities(x => x.City_str == cityStr).FirstOrDefault();
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 35;

                string Str = Request["Str"];
                var totalCount = int.MaxValue;
                //构建搜索条件          
                UserInfoParam userInfoParam = new UserInfoParam()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Str = Str,
                    C_id = CityID.ID
                };
                if (Request["Tval"] != null)
                {
                    #region MyRegion
                    string str = Request["Tval"];
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
                var temp = GetJson(userInfoParam);
                return Json(new { rows = temp, total = userInfoParam.TotalCount }, JsonRequestBehavior.AllowGet);
            }

        public object GetJson(UserInfoParam userInfoParam)
        {
            var actioninfolist = T_FGJHtmlDataService.LoadSearchFrist(userInfoParam, true, true);
            #region 基础信息进行查询
            if (!string.IsNullOrEmpty(userInfoParam.Zhuanxiu))
            {
                actioninfolist = actioninfolist.Where<T_FGJHtmlData>(u => u.FwZhuangxiu.Contains(userInfoParam.Zhuanxiu));
            }
            if (!string.IsNullOrEmpty(userInfoParam.Pingmu) && userInfoParam.Pingmu.Trim() != "0")
            {
                int thiid = int.Parse(userInfoParam.Pingmu);

                actioninfolist = actioninfolist.Where<T_FGJHtmlData>(u => u.MianjiID == thiid);
            }
            if (!string.IsNullOrEmpty(userInfoParam.money) && userInfoParam.money.Trim() != "0")
            {
                int thiid = int.Parse(userInfoParam.money);

                actioninfolist = actioninfolist.Where<T_FGJHtmlData>(u => u.SumMoneyID == thiid);
            }
            if (!string.IsNullOrEmpty(userInfoParam.Tingshi) && userInfoParam.Tingshi.Trim() != "0")
            {
                int thiid = int.Parse(userInfoParam.Tingshi);

                actioninfolist = actioninfolist.Where<T_FGJHtmlData>(u => u.HuXingID == thiid);
            }
            #endregion
            #region 根据详细信息进行查询
            //if (!string.IsNullOrEmpty(userInfoParam.Mon1.ToString()))
            //{
            //    int thiid = int.Parse(userInfoParam.Tingshi);

            //    actioninfolist = actioninfolist.Where<T_FGJHtmlData>(u => u.HuXingID == thiid);
            //}
            //if (!string.IsNullOrEmpty(userInfoParam.Mon2.ToString()))
            //{
            //    int thiid = int.Parse(userInfoParam.Tingshi);

            //    actioninfolist = actioninfolist.Where<T_FGJHtmlData>(u => u.HuXingID == thiid);
            //}
            //if (!string.IsNullOrEmpty(userInfoParam.Pm1.ToString()))
            //{
            //    int thiid = int.Parse(userInfoParam.Tingshi);

            //    actioninfolist = actioninfolist.Where<T_FGJHtmlData>(u => u.HuXingID == thiid);
            //}
            //if (!string.IsNullOrEmpty(userInfoParam.Pm.ToString()))
            //{
            //    int thiid = int.Parse(userInfoParam.Tingshi);

            //    actioninfolist = actioninfolist.Where<T_FGJHtmlData>(u => u.HuXingID == thiid);
            //}
            #endregion
            var temp = from a in actioninfolist
                       select new
                       {
                           ID = a.ID,
                           HLName = a.HLName,
                           Image_str = (a.Image_str.IndexOf("有") >= 0 ? "有" : "无"),
                           FbTime = a.FbTime,
                           PersonName = a.PersonName,
                           Address = a.Address,
                           Laiyuan = a.Laiyuan,
                           FwSumMoney = a.FwSumMoney,
                           FwHuXing = a.FwHuXing,
                           FwLoucheng = a.FwLoucheng,
                           FwZhuangxiu = a.FwZhuangxiu,
                           FwChaoxiang = a.FwChaoxiang,
                           FwNianxian = a.FwNianxian,
                           FwMianji = a.FwMianji,
                           Fmimage = a.Image_str
                       };
            return temp;
        }

        public ActionResult GundongGG()
        {
            var GongG = GongGaoService.LoadEntities(x => x.Items == 8).DefaultIfEmpty();
            var temp = from a in GongG
                       select new
                       {
                           a.text,
                           a.Addtime,
                           a.ID
                       };
            temp = temp.OrderByDescending(x => x.Addtime);
            return Json(new { ret = temp }, JsonRequestBehavior.AllowGet);

        }
        #region 查看图片
        public ActionResult SeeImage()
        {
            int id = int.Parse(Request["id"]);
            var temp = T_FGJHtmlDataService.LoadEntities(x => x.ID == id).FirstOrDefault();
            string imageSTR = temp.Image_str;
            string Masimage = imageSTR.Replace("有---", string.Empty);
            Masimage = Masimage.Replace("w=242&h=150&", "w=700&h=480&");
            if (temp != null)
            {
                return Content(Common.SerializerHelper.SerializeToString(new { serverData = Masimage, msg = "ok" }));
            }
            else
            {
                return Content(Common.SerializerHelper.SerializeToString(new { msg = "no" }));
            }
        }
        #endregion
        #region 查客服电话
        public ActionResult GetKFphone()
        {
            var adminPhone = GongGaoService.LoadEntities(x => x.Items == 2).FirstOrDefault();
            string phoneNum = adminPhone.text;
            return Json(new { ret = "ok", phoneNum = phoneNum }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

}
