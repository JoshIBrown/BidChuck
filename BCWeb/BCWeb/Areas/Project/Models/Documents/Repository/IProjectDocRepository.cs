using BCModel;
using BCModel.Projects;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Project.Models.Documents.Repository
{
    public interface IProjectDocRepository : IGenericRepository<ProjectDocument>
    {
        BCModel.Projects.Project FindProject(int id);
        CompanyProfile FindCompanyProfile(int id);
        UserProfile FindUser(int id);
    }
}