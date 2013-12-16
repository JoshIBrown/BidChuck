using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BCModel;

namespace BCWeb.Models
{
    public class RepositoryBase
    {
        protected BidChuckContext _context;

        public RepositoryBase()
        {
            var iid = HttpContext.Current.User.Identity;
            var ipAddress = HttpContext.Current.Request.UserHostAddress;
            _context = new BidChuckContext(iid.Name, ipAddress);
        }

        public RepositoryBase(string connection)
        {
            var iid = HttpContext.Current.User.Identity;
            var ipAddress = HttpContext.Current.Request.UserHostAddress;
            _context = new BidChuckContext(iid.Name, ipAddress, connection);
        }
    }
}