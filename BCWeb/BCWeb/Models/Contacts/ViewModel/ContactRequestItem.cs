using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Contacts.ViewModel
{
    public class ContactRequestItem
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public ContactRequestType Type { get; set; }
    }

    public enum ContactRequestType
    {
        sent, recvd
    }
}