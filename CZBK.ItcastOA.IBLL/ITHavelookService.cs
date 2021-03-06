﻿using CZBK.ItcastOA.Model;
using CZBK.ItcastOA.Model.SearchParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.IBLL
{
    public partial interface ITHavelookService : IBaseService<THavelook>
    {
        List<THavelook> LoadSearchEntities(UserInfoParam uip);
    }
    
}
