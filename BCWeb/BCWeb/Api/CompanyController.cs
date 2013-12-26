using BCModel;
using BCWeb.Areas.Account.Models.Company.ServiceLayer;
using BCWeb.Areas.Account.Models.Company.ViewModel;
using BCWeb.Areas.Admin.Models.Companies;
using BCWeb.Helpers;
using BCWeb.Models;
using BCWeb.Models.Company.ViewModel;
using BCWeb.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BCWeb.Api
{
    [Authorize]
    public class CompanyController : ApiController
    {
        private ICompanyProfileServiceLayer _service;
        private IWebSecurityWrapper _security;

        public CompanyController(ICompanyProfileServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }

        public IEnumerable<CompanySearchResult> GetSearch(string query)
        {
            var companies = _service.GetEnumerable(x => x.Published && x.CompanyName.Contains(query))
                .Select(x => new CompanySearchResult { Id = x.Id, BusinessType = x.BusinessType.ToDescription(), City = x.City, CompanyName = x.CompanyName, State = x.State.Abbr })
                .ToArray();

            return companies;
        }


        public KeyValuePair<int, string>[] GetArchitects(string query)
        {
            Dictionary<int, string> archs = _service.GetEnumerable(s => s.CompanyName.Contains(query)
                && s.BusinessType == BusinessType.Architect)
                .ToDictionary(i => i.Id, i => i.CompanyName);
            return archs.ToArray();
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

            Expression<Func<CompanyProfile, bool>> predicate = m => true;
            if (sSearch != null && sSearch.Length > 0)
            {
                predicate.And(m => m.CompanyName.Contains(sSearch));
            }

            Func<CompanyProfileListItem, IComparable> orderBy;

            switch (iSortCol_0)
            {
                case 0: // id
                    orderBy = o => o.Id;
                    break;
                case 1: // name
                    orderBy = o => o.CompanyName;
                    break;
                case 2: // type
                    orderBy = o => o.BusinessType;
                    break;
                case 3: // manager
                    orderBy = o => o.Manager;
                    break;
                case 4: // state
                    orderBy = o => o.State;
                    break;
                case 5: // postal code
                    orderBy = o => o.PostalCode;
                    break;
                case 6: // published
                    orderBy = o => o.Published;
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

            response.iTotalDisplayRecords = _service.GetEnumerable(predicate).Count();
            response.iTotalRecords = _service.GetEnumerable().Count();

            CompanyProfileListItem[] data;

            switch (sSortDir_0)
            {
                case "asc":
                    data = _service.GetEnumerable(predicate)
                .Select(s => new CompanyProfileListItem
                {
                    BusinessType = s.BusinessType.ToDescription(),
                    CompanyName = s.CompanyName,
                    Id = s.Id,
                    PostalCode = s.PostalCode == null ? "not set" : s.PostalCode,
                    Published = s.Published,
                    State = s.State == null ? "not set" : s.State.Abbr,
                    Manager = s.Users.Where(u => _security.IsUserInRole(u.Email, "Manager")).Select(u => u.LastName + ", " + u.FirstName).FirstOrDefault()
                })
                .OrderBy(orderBy)
                .Skip(iDisplayStart)
                .Take(iDisplayLength)
                .ToArray();
                    break;
                case "desc":
                    data = _service.GetEnumerable(predicate)
                .Select(s => new CompanyProfileListItem
                {
                    BusinessType = s.BusinessType.ToDescription(),
                    CompanyName = s.CompanyName,
                    Id = s.Id,
                    PostalCode = s.PostalCode,
                    Published = s.Published,
                    State = s.State.Abbr,
                    Manager = s.Users.Where(u => _security.IsUserInRole(u.Email, "Manager")).Select(u => u.LastName + ", " + u.FirstName).FirstOrDefault()
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
