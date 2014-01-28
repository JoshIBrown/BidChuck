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


        public HttpResponseMessage Get(HttpRequestMessage request, [FromUri]string query, [FromUri]BusinessType[] type)
        {
            CompanySearchResultItem[] result = _service.GetEnumerable(s => s.CompanyName.Contains(query)
                && type.Contains(s.BusinessType))
                .Select(s => new CompanySearchResultItem
                {
                    CompanyId = s.Id,
                    CompanyName = s.CompanyName,
                    LinkPath = Url.Link("Default", new { controller = "Company", action = "Profile", id = s.Id })
                })
                .ToArray();

            return request.CreateResponse(HttpStatusCode.OK, result);
        }

        public HttpResponseMessage Get(
            HttpRequestMessage request,
            [FromUri]string query,
            [FromUri]BusinessType[] type,
            [FromUri]string city,
            [FromUri]string state,
            [FromUri]string postal,
            [FromUri]int? distance,
            [FromUri]int[] scopeId,
            [FromUri]int? projectIdForLocation,
            [FromUri]int? bidPackageIdForScopes)
        {
            CompanySearchResultItem[] result = new CompanySearchResultItem[0];
            List<CompanyProfile> companies = new List<CompanyProfile>();

            if (projectIdForLocation.HasValue && (city != null || city != string.Empty || state != null || state != string.Empty || postal != null || state != string.Empty || distance.HasValue))
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, "cannot use both project and other geo location data for search.  must pick one.  either project, or location fields (city,state,postal,distance)");
            }

            // if just filtering by QUERY string
            if ((query != null && query != string.Empty) &&     // has query string ******
                (type == null || type.Length == 0) &&           // no types
                (city == null || city == string.Empty) &&       // no city
                (postal == null || postal == string.Empty) &&   // no postal code
                (state == null || state == string.Empty) &&     // no state
                !distance.HasValue &&                           // no distance
                (scopeId == null || scopeId.Length == 0) &&     // no scopes
                !projectIdForLocation.HasValue &&               // no project
                !bidPackageIdForScopes.HasValue)                // no bid package
            {
                companies = _service.SearchCompanyProfiles(query).ToList();

            } // if just filtering by BUSINESS TYPE
            else if ((query == null || query == string.Empty) &&    // no query string
                (type != null && type.Length > 0) &&                // has types *****
                (city == null || city == string.Empty) &&           // no city
                (postal == null || postal == string.Empty) &&       // no postal code
                (state == null || state == string.Empty) &&         // no state
                !distance.HasValue &&                               // no distance
                (scopeId == null || scopeId.Length == 0) &&         // no scopes
                !projectIdForLocation.HasValue &&                   // no project
                !bidPackageIdForScopes.HasValue)                    // no bid package
            {
                companies = _service.SearchCompanyProfiles(type).ToList();

            }// if just filtering by SCOPES
            else if ((query == null || query == string.Empty) &&    // no query string
                (type == null || type.Length == 0) &&               // no types
                (city == null || city == string.Empty) &&           // no city
                (postal == null || postal == string.Empty) &&       // no postal code
                (state == null || state == string.Empty) &&         // no state
                !distance.HasValue &&                               // no distance
                (scopeId != null && scopeId.Length > 0) &&          // has scopes ******
                !projectIdForLocation.HasValue &&                   // no project
                !bidPackageIdForScopes.HasValue)                    // no bid package
            {
                companies = _service.SearchCompanyProfiles(scopeId).ToList();

            }// if just filtering by BUSINESS TYPE and SCOPES
            else if ((query == null || query == string.Empty) &&    // no query string
                (type != null && type.Length > 0) &&                // has types *****
                (city == null || city == string.Empty) &&           // no city
                (postal == null || postal == string.Empty) &&       // no postal code
                (state == null || state == string.Empty) &&         // no state
                !distance.HasValue &&                               // no distance
                (scopeId != null && scopeId.Length > 0) &&          // has scopes *****
                !projectIdForLocation.HasValue &&                   // no project
                !bidPackageIdForScopes.HasValue)                    // no bid package
            {
                companies = _service.SearchCompanyProfiles(type).ToList();

            } // if just filtering by PROJECT for location
            if ((query == null || query == string.Empty) &&     // no query string
                (type == null || type.Length == 0) &&           // no types
                (city == null || city == string.Empty) &&       // no city
                (postal == null || postal == string.Empty) &&   // no postal code
                (state == null || state == string.Empty) &&     // no state
                !distance.HasValue &&                           // no distance
                (scopeId == null || scopeId.Length == 0) &&     // no scopes
                projectIdForLocation.HasValue &&                // has project ******
                !bidPackageIdForScopes.HasValue)                // no bid package
            {
                companies = _service.SearchCompanyProfiles(projectIdForLocation.Value).ToList();

            }// if just filtering by PROJECT and BID PACKAGE
            else if ((query == null || query == string.Empty) &&    // no query string
                (type == null || type.Length == 0) &&               // no types
                (city == null || city == string.Empty) &&           // no city
                (postal == null || postal == string.Empty) &&       // no postal code
                (state == null || state == string.Empty) &&         // no state
                !distance.HasValue &&                               // no distance
                (scopeId == null || scopeId.Length == 0) &&         // no scopes
                projectIdForLocation.HasValue &&                    // has project *****
                bidPackageIdForScopes.HasValue)                     // has bid package ******
            {
                companies = _service.SearchCompanyProfiles(projectIdForLocation.Value, bidPackageIdForScopes.Value).ToList();

            }// if searching by QUERY string and BUSINESS TYPE
            else if ((query != null && query != string.Empty) &&    // has query string *****
                (type != null && type.Length > 0) &&                // has business types ******
                (city == null || city == string.Empty) &&           // no city
                (postal == null || postal == string.Empty) &&       // no postal code
                (state == null || state == string.Empty) &&         // no state
                !distance.HasValue &&                               // no distance
                (scopeId == null || scopeId.Length == 0) &&         // no scopes
                !projectIdForLocation.HasValue &&                   // no project
                !bidPackageIdForScopes.HasValue)                    // no bid package
            {
                companies = _service.SearchCompanyProfiles(query, type).ToList();

            }// if searching by QUERY string and BUSINESS TYPE and SCOPES
            else if ((query != null && query != string.Empty) &&    // has query string *******
                (type != null && type.Length > 0) &&                // has business types ******
                (city == null || city == string.Empty) &&           // no city
                (postal == null || postal == string.Empty) &&       // no postal code
                (state == null || state == string.Empty) &&         // no state
                !distance.HasValue &&                               // no distance
                (scopeId != null && scopeId.Length != 0) &&         // has scopes ******
                !projectIdForLocation.HasValue &&                   // no project
                !bidPackageIdForScopes.HasValue)                    // no bid package
            {
                companies = _service.SearchCompanyProfiles(query, type).ToList();

            }// if searching by BUSINESS TYPE and PROJECT
            else if ((query == null || query == string.Empty) &&    // no query string
                (type != null && type.Length > 0) &&                // has business types ******
                (city == null || city == string.Empty) &&           // no city
                (postal == null || postal == string.Empty) &&       // no postal code
                (state == null || state == string.Empty) &&         // no state
                !distance.HasValue &&                               // no distance
                (scopeId == null || scopeId.Length == 0) &&         // no scopes
                projectIdForLocation.HasValue &&                    // has project *****
                !bidPackageIdForScopes.HasValue)                    // no bid package
            {
                companies = _service.SearchCompanyProfiles(type, projectIdForLocation.Value).ToList();

            }// if searching by BUSINESS TYPE and PROJECT and BID PACKAGE
            else if ((query == null || query == string.Empty) &&    // no query string
                (type != null && type.Length > 0) &&                // has business types  ********
                (city == null || city == string.Empty) &&           // no city
                (postal == null || postal == string.Empty) &&       // no postal code
                (state == null || state == string.Empty) &&         // no state
                !distance.HasValue &&                               // no distance
                (scopeId == null || scopeId.Length == 0) &&         // no scopes
                projectIdForLocation.HasValue &&                    // no project *******
                bidPackageIdForScopes.HasValue)                     // no bid package ********
            {
                companies = _service.SearchCompanyProfiles(type, projectIdForLocation.Value, bidPackageIdForScopes.Value).ToList();

            }// if searching by ADDRESS and DISTANCE
            else if ((query == null || query == string.Empty) &&    // no query string
                (type == null || type.Length == 0) &&               // no business types
                ((city != null && city != string.Empty) ||          // has city ****
                (postal != null && postal != string.Empty) ||       // has postal code ****
                (state != null && state != string.Empty) ||         // has state ****
                distance.HasValue) &&                               // has distance ****
                (scopeId == null || scopeId.Length == 0) &&         // no scopes
                !projectIdForLocation.HasValue &&                   // no project
                !bidPackageIdForScopes.HasValue)                    // no bid package
            {
                companies = _service.SearchCompanyProfiles(city, state, postal, distance.Value).ToList();

            }// if searching by BUSINESS TYPE and ADDRESS and DISTANCE
            else if ((query == null || query == string.Empty) &&    // no query string
                (type != null && type.Length > 0) &&                // has business types ****
                ((city != null && city != string.Empty) ||          // has city ****
                (postal != null && postal != string.Empty) ||       // has postal code ****
                (state != null && state != string.Empty) ||         // has state **** 
                distance.HasValue) &&                               // has distance *****
                (scopeId == null || scopeId.Length == 0) &&         // no scopes
                !projectIdForLocation.HasValue &&                   // no project
                !bidPackageIdForScopes.HasValue)                    // no bid package
            {
                companies = _service.SearchCompanyProfiles(city, state, postal, distance.Value).ToList();

            }// if searching by QUERY and ADDRESS and DISTANCE
            else if ((query != null && query != string.Empty) &&    // has query string *****
                (type == null || type.Length == 0) &&               // no business types
                ((city != null && city != string.Empty) ||          // has city ****
                (postal != null && postal != string.Empty) ||       // has postal code ****
                (state != null && state != string.Empty) ||         // has state
                distance.HasValue) &&                               // has distance
                (scopeId == null || scopeId.Length == 0) &&         // no scopes
                !projectIdForLocation.HasValue &&                   // no project
                !bidPackageIdForScopes.HasValue)                    // no bid package
            {
                companies = _service.SearchCompanyProfiles(query, city, state, postal, distance.Value).ToList();

            }// if searching by QUERY and BUSINESS TYPE and ADDRESS and DISTANCE 
            else if ((query != null && query != string.Empty) &&    // has query string ****
                (type != null && type.Length != 0) &&               // has business types ****
                ((city != null && city != string.Empty) ||          // has city ****
                (postal != null && postal != string.Empty) ||       // has postal code ****
                (state != null && state != string.Empty) ||         // has state ****
                distance.HasValue) &&                               // has distance ****
                (scopeId == null || scopeId.Length == 0) &&         // no scopes
                !projectIdForLocation.HasValue &&                   // no project
                !bidPackageIdForScopes.HasValue)                    // no bid package
            {
                companies = _service.SearchCompanyProfiles(query, type, city, state, postal, distance.Value).ToList();

            }// if searching by ADDRESS and DISTANCE and SCOPES
            else if ((query == null || query == string.Empty) &&    // no query string 
                (type == null || type.Length == 0) &&               // no business types
                ((city != null && city != string.Empty) ||          // has city ****
                (postal != null && postal != string.Empty) ||       // has postal code ****
                (state != null && state != string.Empty) ||         // has state ****
                distance.HasValue) &&                               // has distance ****
                (scopeId != null && scopeId.Length > 0) &&          // has scopes *****
                !projectIdForLocation.HasValue &&                   // no project
                !bidPackageIdForScopes.HasValue)                    // no bid package
            {
                companies = _service.SearchCompanyProfiles(city, state, postal, distance.Value, scopeId).ToList();

            }// if searching by BUSINESS TYPE and ADDRESS and DISTANCE and SCOPES
            else if ((query == null || query == string.Empty) &&    // no query string 
                (type != null && type.Length > 0) &&                // has business types *****
                ((city != null && city != string.Empty) ||          // has city ****
                (postal != null && postal != string.Empty) ||       // has postal code ****
                (state != null && state != string.Empty) ||         // has state ****
                distance.HasValue) &&                               // has distance ****
                (scopeId != null && scopeId.Length > 0) &&          // has scopes *****
                !projectIdForLocation.HasValue &&                   // no project
                !bidPackageIdForScopes.HasValue)                    // no bid package
            {
                companies = _service.SearchCompanyProfiles(city, state, postal, distance.Value, scopeId).ToList();

            }// if searching by QUERY and ADDRESS and DISTANCE and SCOPES
            else if ((query != null && query != string.Empty) &&    // has query string *******
                (type == null || type.Length == 0) &&               // no business types
                ((city != null && city != string.Empty) ||          // has city ****
                (postal != null && postal != string.Empty) ||       // has postal code ****
                (state != null && state != string.Empty) ||         // has state
                distance.HasValue) &&                               // has distance
                (scopeId != null && scopeId.Length > 0) &&          // has scopes *****
                !projectIdForLocation.HasValue &&                   // no project
                !bidPackageIdForScopes.HasValue)                    // no bid package
            {
                companies = _service.SearchCompanyProfiles(query, city, state, postal, distance.Value).ToList();

            }// if searching by QUERY and BUSINESS TYPES and ADDRESS and DISTANCE and SCOPES
            else if ((query != null && query != string.Empty) &&    // has query string *******
                (type != null && type.Length > 0) &&                // has business types
                ((city != null && city != string.Empty) ||          // has city ****
                (postal != null && postal != string.Empty) ||       // has postal code ****
                (state != null && state != string.Empty) ||         // has state
                distance.HasValue) &&                               // has distance
                (scopeId != null && scopeId.Length > 0) &&          // has scopes *****
                !projectIdForLocation.HasValue &&                   // no project
                !bidPackageIdForScopes.HasValue)                    // no bid package
            {
                companies = _service.SearchCompanyProfiles(query, city, state, postal, distance.Value).ToList();
            }
            else
            {
                companies = _service.GetEnumerable().ToList();
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
