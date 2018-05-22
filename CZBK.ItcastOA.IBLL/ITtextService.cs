using System;
using CZBK.ItcastOA.Model;
using System.Collections.Generic;
using System.Linq;
using CZBK.ItcastOA.Model.SearchParam;
using System.Text;

namespace CZBK.ItcastOA.IBLL
{
    
    public partial interface ITtextService : IBaseService<Ttext>
    {
        bool Delimage(long lgid);
    }
}
