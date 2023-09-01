using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModels.Shared
{
    public partial class QueryParams
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public string SearchTerm { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
    }

   
    
}
