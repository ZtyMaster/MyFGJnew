﻿using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        IBLL.IT_QuyuService T_QuyuService { get; set; }
        IBLL.IT_YxPersonService T_YxPersonService { set; get; }

        IBLL.IWxUserService WxUserService { get; set; }
        IBLL.ITHaveLook_imageService THaveLook_imageService { get; set; }
        IBLL.ITHavelookService THavelookService { get; set; }
        IBLL.ITHaveLookBannerService THaveLookBannerService { get; set; }

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
            if (Request["Tval"] != null)
            {
                string str = Request["Tval"];
                string[] list = str.Split(',');
                for (int i = 0; i < list.Length; i++)
                {
                    string[] ti = list[i].Split('>');
                    if (i == 0)
                    {
                        uip.money = ti[0];
                    } else if (i == 1)
                    {
                        uip.Pingmu = ti[0];
                    }
                }
            }
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
        public ActionResult GetKFInfo()
        {
            KFinfo kfinfo = new KFinfo();
            var adminPhone = GongGaoService.LoadEntities(x => x.Items == 2).FirstOrDefault();
            kfinfo.kfPersonTel = adminPhone.text;
            var kfPersonQQ = GongGaoService.LoadEntities(x => x.Items == 4).FirstOrDefault();
            kfinfo.kfPersonQQ = kfPersonQQ.text;
            var temp = GongGaoService.LoadEntities(x => x.Items == 3).DefaultIfEmpty().ToList();
            foreach (var a in temp)
            {
                switch (a.bak)
                {
                    case "客服姓名":
                        kfinfo.kfPersonName = a.text;
                        break;
                    case "客服微信号":
                        kfinfo.kfPersonVx = a.text;
                        break;
                    case "客服联系地址":
                        kfinfo.kfPersonAddress = a.text;
                        break;
                    case "客服备注":
                        kfinfo.kfPersonBZ = a.text;
                        break;
                }
            }
            return Json(new { ret = "ok", kfInfo = kfinfo }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 获取选项卡条件数据
        public ActionResult GetTabInfo()
        {
            var temp = T_ItemsService.LoadEntities(x => (x.Icons == 0 || x.Icons == 1 || x.Icons == 2 || x.Icons == 4) && x.Str_val != "0").DefaultIfEmpty().ToList();
            List<TabInfo> tiMoney = new List<TabInfo>();
            List<TabInfo> tiMianji = new List<TabInfo>();
            List<TabInfo> tiHuxing = new List<TabInfo>();
            List<TabInfo> tiZujin = new List<TabInfo>();
            foreach (var a in temp)
            {
                switch (a.Icons)
                {
                    case 0:
                        TabInfo money = new TabInfo();
                        money.name = a.Str;
                        money.id = a.ID;
                        money.nameStr = a.Str_val;
                        tiMoney.Add(money);
                        break;
                    case 1:
                        TabInfo mianji = new TabInfo();
                        mianji.name = a.Str;
                        mianji.id = a.ID;
                        mianji.nameStr = a.Str_val;
                        tiMianji.Add(mianji);
                        break;
                    case 2:
                        TabInfo huxing = new TabInfo();
                        huxing.name = a.Str;
                        huxing.id = a.ID;
                        huxing.nameStr = a.Str_val;
                        tiHuxing.Add(huxing);
                        break;
                    case 4:
                        TabInfo zujin = new TabInfo();
                        zujin.name = a.Str;
                        zujin.id = a.ID;
                        zujin.nameStr = a.Str_val;
                        tiZujin.Add(zujin);
                        break;
                }
            }
            var cityStr = Request["cityStr"];
            var rtmp = T_QuyuService.LoadEntities(x => x.T_City.City_str == cityStr).DefaultIfEmpty().ToList();
            if (rtmp != null & rtmp[0] != null)
            {
                var tiArea = from a in rtmp
                             select new
                             {
                                 id = a.ID,
                                 name = a.QY_connet
                             };
                return Json(new { ret = "okAll", tiMoney = tiMoney, tiMianji = tiMianji, tiArea = tiArea, tiHuxing = tiHuxing, tiZujin = tiZujin }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ret = "ok", tiMoney = tiMoney, tiMianji = tiMianji, tiHuxing = tiHuxing, tiZujin = tiZujin }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        public ActionResult GetAllCityList()
        {
            var temp = T_CityService.LoadEntities(x => x.DelFlag == 0).DefaultIfEmpty().ToList();
            if (temp != null && temp[0] != null)
            {
                List<TabInfo> tiList = new List<TabInfo>();
                foreach (var a in temp)
                {
                    TabInfo ti = new TabInfo();
                    ti.id = a.ID;
                    ti.name = a.City;
                    ti.nameStr = a.City_str;
                    tiList.Add(ti);
                }
                return Json(new { ret = "ok", rows = tiList }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ret = "no", msg = "数据表中无数据！" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllZhuangXiuList()
        {
            var temp = T_ItemsService.LoadEntities(x => x.Icons == 3 && x.Str_val != "0").DefaultIfEmpty().ToList();
            if (temp != null && temp[0] != null)
            {
                List<TabInfo> tiList = new List<TabInfo>();
                foreach (var a in temp)
                {
                    TabInfo ti = new TabInfo();
                    ti.id = a.ID;
                    ti.name = a.Str;
                    tiList.Add(ti);
                }
                return Json(new { ret = "ok", rows = tiList }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ret = "no", msg = "数据表中无数据！" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetKfPhoneNum()
        {
            var adminPhone = GongGaoService.LoadEntities(x => x.Items == 2).FirstOrDefault();
            if (adminPhone != null)
            {
                string phoneNum = adminPhone.text;
                return Json(new { ret = "ok", phoneNum = phoneNum }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
        }


        #region 绑定推荐人
        public ActionResult BandPerson()
        {
            //   Person 推进人名称   uid  微信表ID
            if (Request["Person"] == null || Request["Person"].Length <= 0)
            {
                return Json(new { ret = "没有给予推荐人" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string Person = Request["Person"].Trim();
                var personlist = T_YxPersonService.LoadEntities(x => x.PersonName == Person && x.DEL == 0).FirstOrDefault();
                if (personlist != null)
                {
                    string uid = Request["uid"];

                    var iwx = WxUserService.LoadEntities(x => x.Wx_id == uid).FirstOrDefault();
                    if (iwx != null)
                    {
                        iwx.YxPerson_Id = personlist.ID;
                        WxUserService.EditEntity(iwx);
                        return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { ret = "系统不存在该名微信用户！" }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new { ret = "系统不存在该名推荐人，请重新确认！" }, JsonRequestBehavior.AllowGet);
                }

            }

        }

        #endregion
        #region 获取绑定人纪录
        public ActionResult BandPersonYesOrNo()
        {
            string uid = Request["uid"];
            var iwx = WxUserService.LoadEntities(x => x.Wx_id == uid).FirstOrDefault();
            if (iwx != null)
            {
                if (iwx.YxPerson_Id != null)
                {
                    return Json(new { ret = "ok", PersonName = iwx.T_YxPerson.PersonName }, JsonRequestBehavior.AllowGet);
                } else
                {
                    return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
                }
            } else
            {
                return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region 获取登录人身份（个人或中介）
        public ActionResult PersonZJorGR()
        {
            string uid = Request["uid"];
            var iwx = WxUserService.LoadEntities(x => x.Wx_id == uid).FirstOrDefault();
            if (iwx != null)
            {
                if (iwx.ZjOrGr != null)
                {
                    return Json(new { ret = "ok", ziOrGr = iwx.ZjOrGr }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 查看图片
        public ActionResult SeeImageCZ()
        {
            int id = int.Parse(Request["id"]);
            var temp = T_ChuZhuInfoService.LoadEntities(x => x.ID == id).FirstOrDefault();
            string imageSTR = temp.Images;
            string Masimage = imageSTR.Replace("有---", string.Empty);
            if (temp != null)
            {
                return Content(Common.SerializerHelper.SerializeToString(new { sData = Masimage, msg = "ok" }));
            }
            else
            {
                return Content(Common.SerializerHelper.SerializeToString(new { msg = "no" }));
            }
        }
        #endregion
        #region 检查登陆Sessioid
        public ActionResult JianChaSessionID() {
            string Sessionid = Request["sessionId"];
            long ID = Convert.ToInt64(Request["Uid"]);
            var Uis = UserInfoService.LoadEntities(x => x.ID == ID).FirstOrDefault();
            if (Uis != null)
            {
                if (Uis.Login_now == Sessionid)
                {
                    return Json(new { ret = "no" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                }
            }
            else {
                return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion
        #region 微信小程序公告
        public ActionResult updataGG()
        {
            var itemid = Convert.ToInt32(Request["item_s"]);
            var bol = Convert.ToBoolean(Request["bl"]);
            var GongG = GongGaoService.LoadEntities(x => x.Items == itemid).DefaultIfEmpty();
            if (!bol) { GongG = GongG.OrderByDescending(x => x.Addtime).Skip(0).Take(5); }          
            var temp = from a in GongG
                       select new
                       {
                           a.text,
                           a.Addtime,
                           a.ID,
                           a.bak
                       };            
            List<Rtupgg> sgg = new List<Rtupgg>();
            
            foreach (var tm in temp) {
                Rtupgg rt = new Rtupgg();
                rt.Addtime = tm.Addtime;
                rt.Text = tm.text;
                rt.ID = tm.ID;
                rt.Bak = tm.bak;
                var istext= rt.Text.Split(new string[] { "+-" }, StringSplitOptions.None);
                
                rt.St = istext.ToList();
                sgg.Add(rt);
            }

            return Json(new { ret = sgg }, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region 看看数据处理
        //软删除
        public ActionResult RuanDel() {

            long ID = Convert.ToInt64(Request["ID"]);
            var temp = THavelookService.LoadEntities(x => x.ID == ID).FirstOrDefault();
            temp.del = 1;
            if (THavelookService.EditEntity(temp)) {
                return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ret = "删除失败！" }, JsonRequestBehavior.AllowGet);
            }
        }
        //删除看看图片
        public ActionResult DelHaveLook()
        {
            long HavelookID = Convert.ToInt64(Request["HavelookID"]);

            try
            {
                var dtt = THaveLook_imageService.LoadEntities(x => x.HaveLook_ID == HavelookID).DefaultIfEmpty();
                if (dtt.ToList().Count > 0)
                {
                    if (dtt.ToList()[0] != null)
                    {
                        foreach (var tts in dtt)
                        {
                            // return Json(new { ret = Directory.Exists(Path.GetDirectoryName(Request.MapPath(tts.ImageStr))), ss = Request.MapPath(tts.ImageStr) }, JsonRequestBehavior.AllowGet);
                            if (Directory.Exists(Path.GetDirectoryName(Request.MapPath(tts.Str_image))))
                            {
                                System.IO.File.Delete(Request.MapPath(tts.Str_image));
                            }
                        }
                    }

                }
                // return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                if (THaveLook_imageService.THavelookDelImage(HavelookID))
                {
                    return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { ret = "删除失败！" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { ret = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        //记事本数据操作
        public ActionResult AddHaveLook(THavelook tt)
        {
            if (tt.ID > 0)
            {
                var ThisTT = THavelookService.LoadEntities(x => x.ID == tt.ID).FirstOrDefault();
                ThisTT.TextValue = tt.TextValue;
                ThisTT.TopName = tt.TopName;
                ThisTT.Photo = tt.Photo;
                ThisTT.Items_ID = tt.Items_ID;
                ThisTT.Personname = tt.Personname;
                if (THavelookService.EditEntity(ThisTT))
                {
                    return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { ret = "修改数据出现错误！" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                tt.Addtime = DateTime.Now;
                try
                {
                    var temp = THavelookService.AddEntity(tt);
                    return Json(new { ret = "ok", id = temp.ID }, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(new { ret = "新增数据出现错误！" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //上传记事本图片
        public ActionResult AddHavelookFlie(HttpPostedFileWrapper file)
        {
            var ID = Request["IDs"];
            try
            {

                string filename = Path.GetFileName(file.FileName);//获取上传的文件名
                string fileExt = Path.GetExtension(filename);//获取扩展名  
                string dir = "/HaveLookSaveImage/SaveImage/" + MvcApplication.GetT_time().ToString("yyyy-MM-dd") + "/";
                Directory.CreateDirectory(Path.GetDirectoryName(Request.MapPath(dir)));
                string filenewName = Guid.NewGuid().ToString();
                string fulldir = dir + filenewName + fileExt;
                file.SaveAs(Request.MapPath(fulldir));


                THaveLook_image Tige = new THaveLook_image();
                Tige.HaveLook_ID = Convert.ToInt64(ID);
                Tige.Str_image = fulldir;
                Tige.del = 0;
                THaveLook_imageService.AddEntity(Tige);
                return Json(new { ret = "ok", id = ID }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { ret = e.Message, id = ID }, JsonRequestBehavior.AllowGet);
            }
        }
        //获取技术部数据
        public ActionResult LoadTtext()
        {
            int pageIdex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 35;
            int city = Request["cityid"] != null ? int.Parse(Request["cityid"]) : 0;
            int itemid = Request["itemid"] != null ? int.Parse(Request["itemid"]) : 0;
            var val = Request["val"];
            int totalcount = int.MaxValue;
            var temp= THavelookService.LoadSearchEntities(new UserInfoParam() { PageIndex = pageIdex, PageSize = pageSize, CityID = city, C_id = itemid, Str = val, TotalCount = totalcount });
            
            return Json(new { rows = temp, total = totalcount }, JsonRequestBehavior.AllowGet);
        }
        //后台看看banner操作
        public ActionResult LoadHaveLookBanner() {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 35;

            string Str = Request["Str"];
            var totalCount = int.MaxValue;
            var temp = THaveLookBannerService.LoadPageEntities(pageIndex, pageSize, out totalCount, z => z.del == 0, s => s.Items, false);
            var ret = from a in temp
                      select new {
                          a.ID,
                          a.del,
                          a.Imagestr,
                          a.Items
                      };
            return Json(new { rows = ret, total = totalCount }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddHaveLookBanner(THaveLookBanner tlb)
        {
            if (tlb.ID > 0)
            {
                var ThisTT = THaveLookBannerService.LoadEntities(x => x.ID == tlb.ID).FirstOrDefault();
                ThisTT.del = tlb.del;
                ThisTT.Imagestr = tlb.Imagestr;
                ThisTT.Items = tlb.Items;
                if (THaveLookBannerService.EditEntity(ThisTT))
                {
                    return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { ret = "修改数据出现错误！" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                
                try
                {
                    var temp = THaveLookBannerService.AddEntity(tlb);
                    return Json(new { ret = "ok", id = temp.ID }, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(new { ret = "新增数据出现错误！" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //上传后台图片

        public ActionResult AddHavelookBannerFlie()
        {


            HttpPostedFileBase file = Request.Files["fileIconUp"];
            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);//获取上传的文件名
                string fileExt = Path.GetExtension(filename);//获取扩展名
                if (fileExt == ".jpg" || fileExt == ".png")
                {
                    string dir = "/HaveLookSaveImage/BannerImage/" + MvcApplication.GetT_time().ToString("yyyy-MM-dd") + "/";
                    Directory.CreateDirectory(Path.GetDirectoryName(Request.MapPath(dir)));
                    string filenewName = Guid.NewGuid().ToString();
                    string fulldir = dir + filenewName + fileExt;
                    file.SaveAs(Request.MapPath(fulldir));
                    return Content("yes:" + fulldir);
                }
                else
                {
                    return Content("no:文件类型错误，文件扩展名错误！");
                }
            }
            else
            {
                return Content("no:请上传图片文件");
            }            
        }
        #endregion
    }
    public class Rtupgg {
        public DateTime? Addtime { get; set; }
        public int ID { get; set; }
        public string Bak { get; set; }
        public string Text { get; set; }
        public List<string> St { get; set; }
    }
    
    public class KFinfo
    {
        public string kfPersonName { get; set; }
        public string kfPersonTel { get; set; }
        public string kfPersonQQ { get; set; }
        public string kfPersonVx { get; set; }
        public string kfPersonAddress { get; set; }
        public string kfPersonBZ { get; set; }
    }
    public class TabInfo
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string nameStr { get; set; }
    }
}
