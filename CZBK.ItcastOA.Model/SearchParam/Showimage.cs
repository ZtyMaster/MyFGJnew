using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZBK.ItcastOA.Model.SearchParam
{
    public class Showimage:Ttext
    {
        public List<ImageItem> Image { get; set; }
    }
    public class ImageItem {
        public long ImageID { get; set; }
        public string Imagestr { get; set; }
    }
}
