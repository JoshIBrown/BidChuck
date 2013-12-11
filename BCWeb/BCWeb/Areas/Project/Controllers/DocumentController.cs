using BCWeb.Areas.Project.Models.Documents.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Project.Controllers
{
    public class DocumentController : Controller
    {

        IProjectDocServiceLayer _service;

        public DocumentController(IProjectDocServiceLayer service)
        {
            _service = service;
        }

        //
        // GET: /Project/Document/

        public ActionResult Index()
        {
            return View();
        }

    }
}
