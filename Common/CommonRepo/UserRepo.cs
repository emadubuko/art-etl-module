using System;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using Common.DBFasade;
using Common.CommonEntities;
using System.Threading.Tasks;
using NHibernate.Linq;
using System.Linq;
using Common.Utility;

namespace Common.CommonRepo
{
    public class UserRepo : BaseDAO<UserProfile, int>
    {
        public async Task<UserProfile> GetUserAsync(string username)
        {
            var profile = await RetrieveAllLazily()
                .SingleOrDefaultAsync(x => x.UserName == username);
            return profile;
        }

        public async Task<bool> IsUserInRoleAsync(string username, string roleName)
        {
            var user = await RetrieveAllLazily()
                .SingleOrDefaultAsync(x => x.UserName == username
                && x.Role.Name == roleName);

            //ICriteria criteria = BuildSession()
            //   .CreateCriteria<UserProfile>("usr")
            //   .CreateAlias("usr.Role", "rol")
            //   .Add(Restrictions.Eq("usr.Username", username))
            //   .Add(Restrictions.Eq("rol.RoleName", roleName));
            //var user = await criteria.UniqueResultAsync<UserProfile>();
            return user != null;
        }


        public async Task<UserProfile> LoginAsync(string username, string password)
        {
            string pwd = Utils.GetMd5Hash(password);
            var profile = await RetrieveAllLazily()
               .SingleOrDefaultAsync(x => x.UserName == username && x.Password == pwd);
            return profile;
        }

        public async Task<IList<UserProfile>> RetrieveuserListByIPAsync(string IP)
        {
            var users = await RetrieveAllLazily()
                .Where(x => x.IP.ShortName == IP).ToListAsync();
            return users;
        }

        public UserProfile GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}