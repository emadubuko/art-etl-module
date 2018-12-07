using Common.CommonEntities;
using Common.DBFasade;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.CommonRepo
{
    public class RoleRepo : BaseDAO<Role, int>
    {
        public async Task<Role> FindByNameAsync(string roleName)
        {
            var role =
                    await RetrieveAllLazily()
                    .SingleOrDefaultAsync(r => r.Name == roleName);

            return role;
        }
    }
}
