using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Documents.ViewModel
{
    public class ProjectDocViewModel
    {
        public int Id { get; set; }
        
        public int ProjectId { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }
        public string Notes { get; set; }

        public string ProjectTitle { get; set; }
    }
}