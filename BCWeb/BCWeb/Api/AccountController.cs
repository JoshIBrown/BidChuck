using BCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCWeb.Api
{
    public class AccountController : ApiController
    {
        public IEnumerable<object> GetStates()
        {
            IEnumerable<object> output;
            using (BidChuckContext context = new BidChuckContext())
            {
                output = context.State.OrderBy(x => x.Abbr).Select(x => new { Id = x.Id, Value = x.Abbr }).ToArray();
            }
            return output;
        }

        public IEnumerable<object> GetCounties(int stateId)
        {
            IEnumerable<object> output;
            using (BidChuckContext context = new BidChuckContext())
            {
                output = context.Counties
                        .Where(x => x.StateId == stateId)
                        .OrderBy(x => x.Name)
                        .Select(x => new { Id = x.Id, Value = x.Name}).ToArray();
            }
            return output;
        }
    }
}
