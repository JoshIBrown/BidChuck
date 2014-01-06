
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCWeb.Areas.Admin.Controllers
{
    [Authorize(Roles="Administrator")]
    public class BuildingTypeController : Controller
    {
        //
        // GET: /Admin/Builkding/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Edit(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase bizTypesFile)
        {
            if (Path.GetExtension(bizTypesFile.FileName) != ".csv")
            {
                ModelState.AddModelError("bizTypesFile", "invalid file type");
            }

            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileNameWithoutExtension(bizTypesFile.FileName) + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + Path.GetExtension(bizTypesFile.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/admin/uploads"), fileName);
                bizTypesFile.SaveAs(path);

                
            }

            return View();
        }
    }
}
