using CZBK.ItcastOA.IBLL;
using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.BLL
{
    public partial class THavelookService : BaseService<THavelook>, ITHavelookService
    {
        public List<THavelook> LoadSearchEntities(Model.SearchParam.UserInfoParam ump)
        {
            


            var temp = this.GetCurrentDbSession.THavelookDal.LoadEntities(x => x.CityId == ump.CityID&&x.del==0);
            
            if (ump.C_id != 0)
            {
                temp = temp.Where<THavelook>(x => x.Items_ID == ump.C_id);
            }
            if (!string.IsNullOrEmpty(ump.Str))
            {
                temp = temp.Where<THavelook>(u => u.TopName.Contains(ump.Str) || u.TextValue.Contains(ump.Str) );
            }

            ump.TotalCount = temp.Count();
            var tplist= temp.OrderByDescending<THavelook, DateTime>(u => u.Addtime).Skip<THavelook>((ump.PageIndex - 1) * ump.PageSize).Take<THavelook>(ump.PageSize);

            List<THavelook> lse = new List<THavelook>();
            foreach (var t in temp)
            {
                THaveLooks sig = new THaveLooks();
                sig.ID = t.ID;
                sig.Addtime = t.Addtime;
                sig.CityId = t.CityId;
                sig.Items_ID = t.Items_ID;
                sig.Personname = t.Personname;
                sig.Photo = t.Photo;
                sig.TextValue = t.TextValue;
                sig.TopName = t.TopName;
                sig.strimage = new List<ImageItem>();
                var Imm =  this.GetCurrentDbSession.THaveLook_imageDal.LoadEntities(x => x.HaveLook_ID == t.ID).DefaultIfEmpty().ToList();
                if (Imm[0] != null)
                {
                    foreach (var ti in Imm)
                    {
                        ImageItem iim = new ImageItem();
                        iim.ImageID = ti.ID;
                        iim.Imagestr = ti.Str_image;
                        sig.strimage.Add(iim);
                    }
                }

                lse.Add(sig);
            }
            return lse;
        }

    }
}
