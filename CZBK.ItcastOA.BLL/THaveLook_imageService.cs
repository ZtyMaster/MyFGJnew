using CZBK.ItcastOA.IBLL;
using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.BLL
{
    public partial class THaveLook_imageService : BaseService<THaveLook_image>, ITHaveLook_imageService
    {
        public bool THavelookDelImage(long ID)
        {


            var CityID = this.GetCurrentDbSession.THavelookDal.LoadEntities(x => x.ID == ID).FirstOrDefault();
            if (CityID != null)
            {
                var Temp = this.GetCurrentDbSession.THaveLook_imageDal.LoadEntities(x => x.HaveLook_ID == CityID.ID).DefaultIfEmpty().ToList();
                if (Temp != null)
                {
                    if (Temp[0] != null)
                    {
                        foreach (var t in Temp)
                        {
                            this.GetCurrentDbSession.THaveLook_imageDal.DeleteEntity(t);
                        }
                    }
                }
                this.GetCurrentDbSession.THavelookDal.DeleteEntity(CityID);

                return (this.GetCurrentDbSession.SaveChanges());
            }
            else
            {
                return false;
            }
        }
    }
}
