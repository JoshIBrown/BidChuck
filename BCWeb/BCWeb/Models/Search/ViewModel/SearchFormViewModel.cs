using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Models.Search.ViewModel
{
    public class SearchFormViewModel
    {
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> OperatingDistances { get; set; }
        public Tuple<string,string>[] BusinessTypes { get; set; }
    }
}