using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.CommonEntities;
using Common.CommonRepo;
using Common.Utility;
using Microsoft.AspNetCore.Identity;
using NHibernate.Linq;

namespace NDR.Web.Authentication
{
    public class UserStore : IUserStore<UserProfile>,
       IUserEmailStore<UserProfile>,
       IUserPhoneNumberStore<UserProfile>,
       IUserTwoFactorStore<UserProfile>,
       IUserPasswordStore<UserProfile>,
       IUserRoleStore<UserProfile>,
       IUserLoginStore<UserProfile>
    {
        readonly UserRepo usersContext;
        public UserStore()
        {
            usersContext = new UserRepo();
        }
        async Task<IdentityResult> IUserStore<UserProfile>.CreateAsync(UserProfile model, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (model == null)
            {
                throw new ArgumentException("Invalid request.");
            }
            if (string.IsNullOrEmpty(model.Password)) throw new ArgumentException("Password cannot empty");
            if (string.IsNullOrEmpty(model.Email)) throw new ArgumentException("Email cannot cannot empty");

            var user = usersContext.RetrieveAllLazily().FirstOrDefault(x => x.UserName == model.UserName);

            var role = await new RoleRepo().FindByNameAsync(model.RoleName);
            var IP = await new IPRepo().RetrieveAsync(model.OrganizationId);
            if (role == null)
            {
                IdentityError identityError = new IdentityError
                {
                    Description = "invalid rolename"
                };
                return IdentityResult.Failed(identityError);
            }
            if (user == null)
            {
                var userObj = new UserProfile
                {
                    Email = model.Email,
                    Password = Utils.GetMd5Hash(model.Password),
                    CreationDate = DateTime.Now,
                    Status = ProfileStatus.Enabled,
                    FullName = model.FullName,
                    ContactPhoneNumber = model.ContactPhoneNumber,
                    RoleName = model.RoleName,
                    UserName = model.UserName,
                    IP = IP,
                    Role = role,
                };
                await usersContext.SaveAsync(userObj);
                await usersContext.CommitChangesAsync();
                return IdentityResult.Success;
            }
            else
            {
                IdentityError identityError = new IdentityError
                {
                    Description = "duplicate username"
                };
                return IdentityResult.Failed(identityError);
            }
        }
        
        async Task<IdentityResult> IUserStore<UserProfile>.DeleteAsync(UserProfile user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await usersContext.DeleteAsync(user);
            await usersContext.CommitChangesAsync();
            return IdentityResult.Success;
        }

        async Task<UserProfile> IUserStore<UserProfile>.FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await usersContext.RetrieveAsync(int.Parse(userId));
            return user;
        }

        async Task<UserProfile> IUserStore<UserProfile>.FindByNameAsync(
            string userName, CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var au = await usersContext.RetrieveAllLazily()
                           .SingleOrDefaultAsync(u => u.UserName == userName);
            return au;
        }


        Task<string> IUserStore<UserProfile>.GetNormalizedUserNameAsync(UserProfile user, CancellationToken cancellationToken)
            => Task.FromResult(user.NormalizedUserName);


        Task<string> IUserStore<UserProfile>.GetUserIdAsync(UserProfile user, CancellationToken cancellationToken)
            => Task.FromResult(user.Id.ToString());

        Task<string> IUserStore<UserProfile>.GetUserNameAsync(UserProfile user, CancellationToken cancellationToken)
            => Task.FromResult(user.UserName);


        Task IUserStore<UserProfile>.SetNormalizedUserNameAsync(
            UserProfile user, string normalizedName, CancellationToken cancellationToken
        )
        {
            return Task.FromResult(0);
        }

        Task IUserStore<UserProfile>.SetUserNameAsync(UserProfile user, string userName, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        async Task<IdentityResult> IUserStore<UserProfile>.UpdateAsync(UserProfile user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var au = await usersContext.RetrieveAsync(user.Id);

            await usersContext.UpdateAsync(user);
            await usersContext.CommitChangesAsync();

            return IdentityResult.Success;
        }

        Task IUserEmailStore<UserProfile>.SetEmailAsync(UserProfile user, string email, CancellationToken cancellationToken)
        {
            //user.SetEmail(email);
            return Task.FromResult(0);
        }

        Task<string> IUserEmailStore<UserProfile>.GetEmailAsync(UserProfile user, CancellationToken cancellationToken)
            => Task.FromResult(user.Email);

        Task<bool> IUserEmailStore<UserProfile>.GetEmailConfirmedAsync(
            UserProfile user, CancellationToken cancellationToken
        ) => Task.FromResult(user.EmailConfirmed);

        Task IUserEmailStore<UserProfile>.SetEmailConfirmedAsync(
            UserProfile user, bool confirmed, CancellationToken cancellationToken
        )
        {
            // user.SetEmailConfirmed(confirmed);
            return Task.FromResult(0);
        }

        async Task<UserProfile> IUserEmailStore<UserProfile>.FindByEmailAsync(
            string email, CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var au = await usersContext.RetrieveAllLazily()
                           .SingleOrDefaultAsync(u => u.Email == email);
            return au;
        }

        Task<string> IUserEmailStore<UserProfile>.GetNormalizedEmailAsync(UserProfile user, CancellationToken cancellationToken)
            => Task.FromResult(user.NormalizedEmail);

        Task IUserEmailStore<UserProfile>.SetNormalizedEmailAsync(
            UserProfile user, string normalizedEmail, CancellationToken cancellationToken
        )
        {
            user.Email = normalizedEmail;
            return Task.FromResult(0);
        }

        Task IUserPhoneNumberStore<UserProfile>.SetPhoneNumberAsync(
            UserProfile user, string phoneNumber, CancellationToken cancellationToken
        )
        {
            user.ContactPhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        Task<string> IUserPhoneNumberStore<UserProfile>.GetPhoneNumberAsync(UserProfile user, CancellationToken cancellationToken)
            => Task.FromResult(user.PhoneNumber);


        Task<bool> IUserPhoneNumberStore<UserProfile>.GetPhoneNumberConfirmedAsync(
            UserProfile user, CancellationToken cancellationToken
        ) => Task.FromResult(user.PhoneNumberConfirmed);

        Task IUserPhoneNumberStore<UserProfile>.SetPhoneNumberConfirmedAsync(
            UserProfile user, bool confirmed, CancellationToken cancellationToken
        )
        {
            return Task.FromResult(0);
        }

        Task IUserTwoFactorStore<UserProfile>.SetTwoFactorEnabledAsync(
            UserProfile user, bool enabled, CancellationToken cancellationToken
        )
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        Task<bool> IUserTwoFactorStore<UserProfile>.GetTwoFactorEnabledAsync(UserProfile user, CancellationToken cancellationToken)
            => Task.FromResult(user.TwoFactorEnabled);


        Task IUserPasswordStore<UserProfile>.SetPasswordHashAsync(
            UserProfile user, string passwordHash, CancellationToken cancellationToken
        )
        {
            user.Password = Utils.GetMd5Hash(passwordHash);
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        Task<string> IUserPasswordStore<UserProfile>.GetPasswordHashAsync(UserProfile user, CancellationToken cancellationToken)
            => Task.FromResult(user.PasswordHash);


        Task<bool> IUserPasswordStore<UserProfile>.HasPasswordAsync(UserProfile user, CancellationToken cancellationToken)
            => Task.FromResult(user.PasswordHash != null);

        async Task IUserRoleStore<UserProfile>.AddToRoleAsync(UserProfile user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var roleByName =
                    await usersContext.RetrieveAllLazily()
                    .SingleOrDefaultAsync(u => u.RoleName == roleName);

            //if (roleByName == null)
            //{
            //    roleByName = new Role(roleName);
            //    usersContext.Persist(roleByName);
            //}

            //var userGot = await usersContext.RetrieveAsync(user.Id);
            //userGot.AddRole(roleByName);

            await usersContext.CommitChangesAsync();
        }

        async Task IUserRoleStore<UserProfile>.RemoveFromRoleAsync(
            UserProfile user, string roleName, CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            var userLoaded = await usersContext.RetrieveAsync(user.Id);

            // await userLoaded.RemoveRoleAsync(roleName);

            await usersContext.CommitChangesAsync();
        }

        async Task<IList<string>> IUserRoleStore<UserProfile>.GetRolesAsync(UserProfile user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userGot = await usersContext.GetUserAsync(user.UserName);
            if(userGot.Role != null)
            {
                return new List<string> { userGot.Role.Name };
            }
            return new List<string> { userGot.RoleName }; 
        }

        async Task<bool> IUserRoleStore<UserProfile>.IsInRoleAsync(
            UserProfile user, string roleName, CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var status = await usersContext.IsUserInRoleAsync(user.UserName, roleName);
            return status;
            //return await userGot.IsInRole(roleName);
        }

        async Task<IList<UserProfile>> IUserRoleStore<UserProfile>.GetUsersInRoleAsync(
            string roleName, CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            string normalizedRoleName = roleName.ToUpper();

            //var usersList = await Role.GetUsersByRoleNameAsync(usersContext.Query<UserProfile>(), normalizedRoleName);
            //return usersList;
            return new List<UserProfile>();
        }

        async Task IUserLoginStore<UserProfile>.AddLoginAsync(
            UserProfile user, UserLoginInfo login, CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (login == null)
                throw new ArgumentNullException(nameof(login));

            var au = await usersContext.RetrieveAsync(user.Id);

            //au.AddExternalLogin(login.LoginProvider, login.ProviderKey, login.ProviderDisplayName);

            await usersContext.CommitChangesAsync();

        }

        async Task<UserProfile> IUserLoginStore<UserProfile>.FindByLoginAsync(
            string loginProvider, string providerKey, CancellationToken cancellationToken
        )
        {
            //var user = await UserProfile.FindByLoginAsync(usersContext.Query<UserProfile>(), loginProvider, providerKey);
            //return user;
            return null;
        }

        async Task<IList<UserLoginInfo>> IUserLoginStore<UserProfile>.GetLoginsAsync(
            UserProfile user, CancellationToken cancellationToken
        )
        {
            var au = await usersContext.RetrieveAsync(user.Id);
            //var list = au.GetUserLoginInfoList();
            //return list;
            return null;
        }

        async Task IUserLoginStore<UserProfile>.RemoveLoginAsync(
            UserProfile user, string loginProvider, string providerKey, CancellationToken cancellationToken
        )
        {
            var au = await usersContext.RetrieveAsync(user.Id);
            //await au.RemoveExternalLoginAsync(loginProvider, providerKey);
            await usersContext.CommitChangesAsync();
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }

    }
}
