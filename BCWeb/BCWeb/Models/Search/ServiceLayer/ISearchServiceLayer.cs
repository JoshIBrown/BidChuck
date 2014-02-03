using BCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Models.Search.ServiceLayer
{
    public interface ISearchServiceLayer
    {
        IEnumerable<BCModel.Projects.Project> SearchProjects(string query);
        IEnumerable<State> GetStates();
    }
}
