using BCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Models.Search.Repository
{
    public interface ISearchRepository
    {
        IQueryable<BCModel.Projects.Project> QueryProjects();
        IQueryable<CompanyProfile> QueryCompanyProfiles();
        IQueryable<UserProfile> QueryUserProfile();
        UserProfile FindUserProfile(int id);
        CompanyProfile FindCompanyProfile(int id);
        BCModel.Projects.Project FindProject(int id);

        IQueryable<State> GetStates();
    }
}
