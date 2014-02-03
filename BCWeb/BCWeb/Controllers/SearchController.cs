using BCModel;
using BCWeb.Models.Search.ServiceLayer;
using BCWeb.Models.Search.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BCWeb.Helpers;

namespace BCWeb.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private ISearchServiceLayer _service;
        private int[] operatingDistance = new int[] { 25, 50, 100, 150, 200, 300, 400, 500 };
        public SearchController(ISearchServiceLayer service)
        {
            _service = service;
        }

        //
        // GET: /Search/

        public ActionResult Index()
        {
            SearchFormViewModel viewModel = new SearchFormViewModel();
            viewModel.OperatingDistances = operatingDistance.Select(s => new SelectListItem { Value = (Convert.ToDouble(s)).ToString(), Text = s.ToString() + " miles" });
            viewModel.States = _service.GetStates().Select(s => new SelectListItem { Value = s.Abbr, Text = s.Abbr });
            viewModel.BusinessTypes = (from Enum e in Enum.GetValues(typeof(BusinessType))
                                       select new Tuple<string, string>(e.ToString(), e.ToDescription())).ToArray();

            return View(viewModel);
        }
    }
}
