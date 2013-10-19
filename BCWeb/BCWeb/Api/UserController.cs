using BCModel;
using BCWeb.Areas.Account.Models.Users.ServiceLayer;
using BCWeb.Areas.Admin.Models.Users;
using BCWeb.Helpers;
using BCWeb.Models;
using BCWeb.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebMatrix.WebData;

namespace BCWeb.Api
{
    [Authorize(Roles = "Administrator")]
    public class UserController : ApiController
    {
        private IUserProfileServiceLayer _service;
        private IWebSecurityWrapper _security;

        public UserController(IUserProfileServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;

        }

        public DataTablesResponse GetDataTable(
            [FromUri]int iDisplayStart,
            [FromUri]int iDisplayLength,
            [FromUri]int iColumns,
            [FromUri]string sSearch,
            [FromUri]int iSortCol_0,
            [FromUri]string sSortDir_0,
            [FromUri]string sEcho)
        {
            DataTablesResponse response = new DataTablesResponse();

            Expression<Func<UserProfile, bool>> search = m => true;

            if (sSearch != null && sSearch.Length > 0)
            {
                search = search.And(m => m.Email.Contains(sSearch) ||
                    m.FirstName.Contains(sSearch) ||
                    m.LastName.Contains(sSearch) ||
                    m.JobTitle.Contains(sSearch));
            }

            Func<UserProfileListItem, IComparable> orderBy;

            switch (iSortCol_0)
            {
                case 0: // id
                    orderBy = o => o.Id;
                    break;
                case 1: // email
                    orderBy = o => o.Email;
                    break;
                case 2: // last name
                    orderBy = o => o.LastName;
                    break;
                case 3: // first name
                    orderBy = o => o.FirstName;
                    break;
                case 4: // job title
                    orderBy = o => o.JobTitle;
                    break;
                case 5: // company id
                    orderBy = o => o.CompanyId;
                    break;
                case 6: // confirmed
                    orderBy = o => o.Confirmed;
                    break;
                default:
                    orderBy = o => o.Id;
                    break;
            }

            int sEchoCheck;
            if (int.TryParse(sEcho, out sEchoCheck))
            {
                response.sEcho = sEcho;
            }
            else
            {
                throw new Exception("sEcho is invalid");
            }

            response.iTotalRecords = _service.GetEnumerable().Count();
            response.iTotalDisplayRecords = _service.GetEnumerable(search).Count();

            UserProfileListItem[] data;

            switch (sSortDir_0)
            {
                case "asc":
                    data = _service.GetEnumerable(search)
                .Select(s => new UserProfileListItem
                {
                    CompanyId = s.CompanyId,
                    Confirmed = _security.IsConfirmed(s.Email),
                    Email = s.Email,
                    FirstName = s.FirstName,
                    Id = s.UserId,
                    LastName = s.LastName,
                    JobTitle = s.JobTitle
                })
                .OrderBy(orderBy)
                .Skip(iDisplayStart)
                .Take(iDisplayLength)
                .ToArray();
                    break;
                case "desc":
                    data = _service.GetEnumerable(search)
                .Select(s => new UserProfileListItem
                {
                    CompanyId = s.CompanyId,
                    Confirmed = _security.IsConfirmed(s.Email),
                    Email = s.Email,
                    FirstName = s.FirstName,
                    Id = s.UserId,
                    LastName = s.LastName,
                    JobTitle = s.JobTitle
                })
                .OrderByDescending(orderBy)
                .Skip(iDisplayStart)
                .Take(iDisplayLength)
                .ToArray();
                    break;
                default:
                    throw new Exception("invalid sort direction");
            }

            response.aaData = data;
            return response;
        }
    }
}
