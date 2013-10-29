using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Project.ViewModel
{
    public class DuplicatesViewModel
    {
        public IEnumerable<ProjectListViewModel> Projects { get; set; }
        public string btn { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public int ArchitectId { get; set; }
    }
}