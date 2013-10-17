using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Project.ViewModel
{
    public class ProjectScopeListItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int? parentId { get; set; }
    }
}