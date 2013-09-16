using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCModel;
using BCModel.Projects;

namespace BCWeb.Models.Project.Repository
{
    public interface IProjectRepository : IGenericRepository<BCModel.Projects.Project>
    {
        IQueryable<ConstructionType> QueryConstructionType();
        IQueryable<BuildingType> QueryBuildingType();
        //IQueryable<ProjectType> QueryProjectType();
        IQueryable<State> QueryStates();
    }
}
