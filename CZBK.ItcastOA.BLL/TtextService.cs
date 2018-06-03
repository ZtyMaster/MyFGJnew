using CZBK.ItcastOA.IBLL;
using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.BLL
{
    public partial class TtextService : BaseService<Ttext>, ITtextService
    {
        public bool Delimage(long ID)
        {


            var CityID = this.GetCurrentDbSession.TtextDal.LoadEntities(x => x.Id == ID).FirstOrDefault();
            if (CityID != null)
            {
                var Temp = this.GetCurrentDbSession.TtextImageDal.LoadEntities(x => x.TextID == CityID.Id).DefaultIfEmpty().ToList();
                if (Temp != null)
                {
                    if (Temp[0] != null)
                    {
                        foreach (var t in Temp)
                        {
                            this.GetCurrentDbSession.TtextImageDal.DeleteEntity(t);
                        }
                    }
                }
                this.GetCurrentDbSession.TtextDal.DeleteEntity(CityID);

                return (this.GetCurrentDbSession.SaveChanges());
            }
            else {
                return false;
            }
        }
    }
}
