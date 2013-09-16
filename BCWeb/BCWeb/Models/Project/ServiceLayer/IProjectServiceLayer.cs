using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCModel;
using BCModel.Projects;
using System.Linq.Expressions;

namespace BCWeb.Models.Project.ServiceLayer
{
    public interface IProjectServiceLayer : IGenericServiceLayer<BCModel.Projects.Project>
    {
        IEnumerable<BuildingType> GetBuildingTypes();
        IEnumerable<ConstructionType> GetConstructionTypes();
        //IEnumerable<ProjectType> GetProjectTypes();
        IEnumerable<State> GetStates();

        CompanyProfile GetCompanyProfile(int id);
        UserProfile GetUserProfile(int id);
        IEnumerable<CompanyProfile> GetCompanyProfiles();
        IEnumerable<CompanyProfile> GetCompanyProfiles(Expression<Func<CompanyProfile, bool>> predicate);
    }
}
