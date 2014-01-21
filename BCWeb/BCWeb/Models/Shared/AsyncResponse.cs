using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Shared
{
    public class AsyncResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}