using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Controllers
{
    public class AngularTemplateController : Controller
    {
        //
        // GET: /AngularTemplate/

        public PartialViewResult ScopePicker()
        {
            return PartialView();
        }

    }
}
