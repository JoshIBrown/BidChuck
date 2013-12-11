using BCModel;
using BCModel.Projects;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Project.Models.Documents.ServiceLayer
{
    public interface IProjectDocServiceLayer : IGenericServiceLayer<ProjectDocument>
    {
        BCModel.Projects.Project GetProject(int id);
        CompanyProfile GetCompany(int id);
    }
}
