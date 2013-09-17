using BCModel;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCWeb.Areas.Account.Models.Users.Repository
{
    public interface IUserProfileRepository : IGenericRepository<UserProfile>
    {
        CompanyProfile GetCompany(int id);
    }
}
