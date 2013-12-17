using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Documents.ViewModel
{
    public class ProjectDocEditModel
    {
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public string Name { get; set; }
        [Url, Required]
        public string Url { get; set; }

        public string Notes { get; set; }

        public string ProjectTitle { get; set; }
    }
}