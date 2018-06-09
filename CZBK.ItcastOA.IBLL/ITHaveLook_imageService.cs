using CZBK.ItcastOA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.IBLL
{
   
    public partial interface ITHaveLook_imageService : IBaseService<THaveLook_image>
    {
        bool THavelookDelImage(long lgid);
    }
}
