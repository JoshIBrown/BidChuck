using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Notifications.ViewModel
{
    public class NoticeListItem
    {
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsRead { get; set; }
        public int ProjectId { get; set; }
        public string Link { get; set; }
    }
}