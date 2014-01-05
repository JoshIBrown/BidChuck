using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Notifications.ViewModel
{
    public class NewestNoticesViewModel
    {
        public int UnreadCount { get; set; }
        public NoticeListItem[] Notices { get; set; } 
    }
}