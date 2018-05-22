using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.IO;
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
        IBLL.ITtextImageService TtextImageService { get; set; }
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
        //删除记事本
        public ActionResult DelText() {
            long id = Convert.ToInt64(Request["id"]);
            
            try {
                var dtt = TtextImageService.LoadEntities(x => x.TextID == id).DefaultIfEmpty();
                if (dtt.ToList().Count > 0)
                {
                    if (dtt.ToList()[0] != null)
                    {
                        foreach (var tts in dtt)
                        {
                           // return Json(new { ret = Directory.Exists(Path.GetDirectoryName(Request.MapPath(tts.ImageStr))), ss = Request.MapPath(tts.ImageStr) }, JsonRequestBehavior.AllowGet);
                            if (Directory.Exists(Path.GetDirectoryName(Request.MapPath(tts.ImageStr))))
                            {
                                System.IO.File.Delete(Request.MapPath(tts.ImageStr));
                            }
                        }                       
                    }
                    
                }
                // return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                if (TtextService.Delimage(id))
                {
                    return Json(new { ret = "ok" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { ret = "删除失败！" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e) {
                return Json(new { ret = e.Message }, JsonRequestBehavior.AllowGet);
            }
            
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
                    var temp= TtextService.AddEntity(tt);
                    return Json(new { ret = "ok",id=temp.Id }, JsonRequestBehavior.AllowGet);
                }
                catch {
                    return Json(new { ret = "新增数据出现错误！" }, JsonRequestBehavior.AllowGet);
                }                
            }  
        }
        //上传记事本图片
        public ActionResult AddTextFlie(HttpPostedFileWrapper file) {
            var ID = Request["IDs"];
            try
            {             
                
                string filename = Path.GetFileName(file.FileName);//获取上传的文件名
                string fileExt = Path.GetExtension(filename);//获取扩展名  
                string dir = "/UserSaveImage/WxImage/" + LoginUser.UName + "/" + MvcApplication.GetT_time().ToString("yyyy-MM-dd") + "/";
                Directory.CreateDirectory(Path.GetDirectoryName(Request.MapPath(dir)));
                string filenewName = Guid.NewGuid().ToString();
                string fulldir = dir + filenewName + fileExt;
                file.SaveAs(Request.MapPath(fulldir));


                TtextImage Tige = new TtextImage();
                Tige.TextID = Convert.ToInt64(ID);
                Tige.ImageStr = fulldir;
                TtextImageService.AddEntity(Tige);
                return Json(new { ret = "ok" ,id= ID }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e){
                return Json(new { ret = e.Message, id = ID }, JsonRequestBehavior.AllowGet);
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
                           ID = a.Id, a.Text, a.AddTime, a.AddUser                            
                       };
            List<Showimage> lse = new List<Showimage>();
            foreach (var t in temp)
            {
                Showimage sig = new Showimage();
                sig.Id = t.ID;
                sig.Text = t.Text;
                sig.AddTime = t.AddTime;
                sig.Image = new List<ImageItem>();
                var Imm = TtextImageService.LoadEntities(x => x.TextID == t.ID).DefaultIfEmpty().ToList();
                if (Imm[0] != null)
                {
                    foreach (var ti in Imm)
                    {
                        ImageItem iim = new ImageItem();
                        iim.ImageID = ti.ID;
                        iim.Imagestr = ti.ImageStr;
                        sig.Image.Add(iim);
                    }
                }
                
                lse.Add(sig);
            }
            return Json(new { rows = lse, total = totalcount }, JsonRequestBehavior.AllowGet);
        }
    }
}
