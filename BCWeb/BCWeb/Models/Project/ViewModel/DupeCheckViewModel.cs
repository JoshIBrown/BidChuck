using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Project.ViewModel
{
    public class DupeCheckViewModel
    {
        public string Title { get; set; }
        public string Number { get; set; }
        public int? ArchitectId { get; set; }
        public string Architect { get; set; }
    }
}