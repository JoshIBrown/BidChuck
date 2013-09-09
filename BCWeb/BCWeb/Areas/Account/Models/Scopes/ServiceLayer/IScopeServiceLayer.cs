using BCModel;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Account.Models.Scopes.ServiceLayer
{
    public interface IScopeServiceLayer : IGenericServiceLayer<Scope>
    {
        UserProfile GetUser(int id);
        CompanyProfile GetCompany(int id);
    }
}
