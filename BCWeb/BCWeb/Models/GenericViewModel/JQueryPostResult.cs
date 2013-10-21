using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.GenericViewModel
{
    public class JQueryPostResult
    {
        public string message { get; set; }
        public bool success { get; set; }
        public object data { get; set; }
    }
}