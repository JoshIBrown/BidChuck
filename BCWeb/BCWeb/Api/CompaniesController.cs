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
    public class CompaniesController : ApiController
    {
        private ICompanyProfileServiceLayer _service;
        private IWebSecurityWrapper _security;

        public CompaniesController(ICompanyProfileServiceLayer service, IWebSecurityWrapper security)
        {
            _service = service;
            _security = security;
        }

        private CompanySearchResultItem companySearchResultItemMapper(CompanyProfile company)
        {
            CompanySearchResultItem result = new CompanySearchResultItem
            {
                CompanyId = company.Id,
                CompanyName = company.CompanyName,
                LinkPath = Url.Link("Default", new { controller = "Company", action = "Profile", id = company.Id }),
                BusinessType = company.BusinessType.ToDescription(),
                Area = company.City != null ? company.City + ", " + company.State.Abbr : "",
                ScopesOfWork = company.Scopes == null || company.Scopes.Count == 0 ? default(Dictionary<int, string>) : company.Scopes.Where(c => c.Scope.Children == null || c.Scope.Children.Count == 0).Select(s => s.Scope).ToDictionary(s => s.Id, s => s.CsiNumber + " " + s.Description)
            };
            return result;
        }

        public HttpResponseMessage GetForProject(HttpRequestMessage request, [FromUri]BusinessType[] type, [FromUri]int projectIdForLocation)
        {
            if (type == null || type.Length == 0)
            {
                var companies = _service.SearchCompanyProfiles(projectIdForLocation, ProjectEntityType.project).Select(c => companySearchResultItemMapper(c)).ToArray();
                return request.CreateResponse(HttpStatusCode.OK, companies);
            }
            else
            {
                var companies = _service.SearchCompanyProfiles(type, projectIdForLocation, ProjectEntityType.project).Select(c => companySearchResultItemMapper(c)).ToArray();
                return request.CreateResponse(HttpStatusCode.OK, companies);
            }

        }

        public HttpResponseMessage GetForBidPackage(HttpRequestMessage request, [FromUri]BusinessType[] type, [FromUri]int bidPackageIdForScopes)
        {
            if (type == null || type.Length == 0)
            {
                var companies = _service.SearchCompanyProfiles(bidPackageIdForScopes, ProjectEntityType.bidPackage).Select(c => companySearchResultItemMapper(c)).ToArray();
                return request.CreateResponse(HttpStatusCode.OK, companies);
            }
            else
            {
                var companies = _service.SearchCompanyProfiles(type, bidPackageIdForScopes, ProjectEntityType.bidPackage).Select(c => companySearchResultItemMapper(c)).ToArray();
                return request.CreateResponse(HttpStatusCode.OK, companies);
            }

        }

        public HttpResponseMessage Get(
            HttpRequestMessage request,
            [FromUri]string query,
            [FromUri]BusinessType[] type,
            [FromUri]string city,
            [FromUri]string state,
            [FromUri]string postal,
            [FromUri]int? distance,
            [FromUri]int[] scopeId)
        {
            CompanySearchResultItem[] result = new CompanySearchResultItem[0];
            List<CompanyProfile> companies = new List<CompanyProfile>();


            if ((type != null && type.Length > 0) &&
                distance.HasValue &&
                (city != null || state != null || postal != null) &&
                (scopeId != null && scopeId.Length > 0))
            {
                companies = _service.SearchCompanyProfiles(
                    query == null ? "" : query,
                    type,
                    city == null ? "" : city,
                    state == null ? "" : state,
                    postal == null ? "" : postal,
                    Convert.ToDouble(distance.Value),
                    scopeId
                    ).ToList();
            }
            else if ((type != null && type.Length > 0) &&
               distance.HasValue &&
               (city != null || state != null || postal != null) &&
               (scopeId == null || scopeId.Length == 0))
            {
                companies = _service.SearchCompanyProfiles(
                    query == null ? "" : query,
                    type,
                    city == null ? "" : city,
                    state == null ? "" : state,
                    postal == null ? "" : postal,
                    Convert.ToDouble(distance.Value)
                    ).ToList();
            }
            else if ((type == null || type.Length == 0) &&
               distance.HasValue &&
               (city != null || state != null || postal != null) &&
               (scopeId != null && scopeId.Length > 0))
            {
                companies = _service.SearchCompanyProfiles(
                    query == null ? "" : query,
                    city == null ? "" : city,
                    state == null ? "" : state,
                    postal == null ? "" : postal,
                    Convert.ToDouble(distance.Value),
                    scopeId
                    ).ToList();
            }
            else if ((type == null || type.Length == 0) &&
              distance.HasValue &&
              (city != null || state != null || postal != null) &&
              (scopeId == null || scopeId.Length == 0))
            {
                companies = _service.SearchCompanyProfiles(
                    query == null ? "" : query,
                    city == null ? "" : city,
                    state == null ? "" : state,
                    postal == null ? "" : postal,
                    Convert.ToDouble(distance.Value)
                    ).ToList();
            }
            else if ((type != null && type.Length > 0) &&
                       !distance.HasValue &&
                       (scopeId != null && scopeId.Length > 0))
            {
                companies = _service.SearchCompanyProfiles(
                    query == null ? "" : query,
                    type,
                    scopeId
                    ).ToList();
            }
            else if ((type != null && type.Length > 0) &&
                       !distance.HasValue &&
                       (scopeId == null || scopeId.Length == 0))
            {
                companies = _service.SearchCompanyProfiles(
                    query == null ? "" : query,
                    type
                    ).ToList();
            }
            else if ((type == null || type.Length == 0) &&
                           !distance.HasValue &&
                           (scopeId != null && scopeId.Length > 0))
            {
                companies = _service.SearchCompanyProfiles(
                    query == null ? "" : query,
                    scopeId
                    ).ToList();
            }
            else
            {
                companies = _service.SearchCompanyProfiles(query == null ? "" : query).ToList();
            }


            result = companies.Select(s => new CompanySearchResultItem
                {
                    CompanyId = s.Id,
                    CompanyName = s.CompanyName,
                    LinkPath = Url.Link("Default", new { controller = "Company", action = "Profile", id = s.Id }),
                    BusinessType = s.BusinessType.ToDescription(),
                    ScopesOfWork = s.Scopes.Select(c => c.Scope).ToDictionary(x => x.Id, x => x.CsiNumber + " " + x.Description)
                })
                .ToArray();

            return request.CreateResponse(HttpStatusCode.OK, result);
        }

        public DataTablesResponse Get(HttpRequestMessage request,
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
                    Subscribed = s.SubscriptionStatus.ToString(),
                    State = s.State == null ? "not set" : s.State.Abbr,
                    Manager = s.Users.Where(u => _security.IsUserInRole(u.Email, "Manager")).Count() == 0 ? "not set" : s.Users.Where(u => _security.IsUserInRole(u.Email, "Manager")).Select(u => u.LastName + ", " + u.FirstName).FirstOrDefault()
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
                    Subscribed = s.SubscriptionStatus.ToString(),
                    State = s.State.Abbr,
                    Manager = s.Users.Where(u => _security.IsUserInRole(u.Email, "Manager")).Count() == 0 ? "not set" : s.Users.Where(u => _security.IsUserInRole(u.Email, "Manager")).Select(u => u.LastName + ", " + u.FirstName).FirstOrDefault()
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
