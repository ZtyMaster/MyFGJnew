using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZBK.ItcastOA.WebApp.Controllers
{
    public class TestController : BaseController
    {
        //
        // GET: /Test/
        IBLL.ITtextService TtextService { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Test()
        {

            int a = 2;
            int b = 0;
            int c = a / b;
            return Content(c.ToString());
        }
        //记事本数据操作
        public ActionResult AddText(Ttext tt) {
            if (tt.Id > 0)
            {
                var ThisTT= TtextService.LoadEntities(x => x.Id == tt.Id).FirstOrDefault();
                ThisTT.Text = tt.Text;
                if (TtextService.EditEntity(ThisTT))
                {
                    return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                }
                else {
                    return Json(new { ret = "修改数据出现错误！" }, JsonRequestBehavior.AllowGet);
                }
            }
            else {
                tt.AddUser = LoginUser.ID;
                tt.AddTime = DateTime.Now;
                try
                {
                    TtextService.AddEntity(tt);
                    return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                }
                catch {
                    return Json(new { ret = "新增数据出现错误！" }, JsonRequestBehavior.AllowGet);
                }                
            }  
        }
        //获取技术部数据
        public ActionResult LoadTtext() {
            int pageIdex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 35;
            int totalcount = int.MaxValue;
            var mydata = TtextService.LoadPageEntities(pageIdex, pageSize, out totalcount, x => x.AddUser == LoginUser.ID, x => x.AddTime, false);
            var temp = from a in mydata
                       select new
                       {
                           ID = a.Id,a.Text,a.AddTime,a.AddUser                            
                       };

            return Json(new { rows = temp, total = totalcount }, JsonRequestBehavior.AllowGet);
        }
    }
}
