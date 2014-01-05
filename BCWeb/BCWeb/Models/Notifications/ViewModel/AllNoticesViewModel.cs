using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Notifications.ViewModel
{
    public class AllNoticesViewModel
    {
        public NoticeDatePool[] DatePool { get; set; }
    }

    public class NoticeDatePool
    {
        public string Date { get; set; }
        public NoticeListItem[] Notices { get; set; }
    }
}