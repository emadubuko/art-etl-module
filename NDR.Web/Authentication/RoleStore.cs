using Common.CommonEntities;
using Common.CommonRepo;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Linq;


namespace NDR.Web.Authentication
{
    public class RoleStore : IRoleStore<Role>
    {
        RoleRepo roleRepo { get; }

        public RoleStore() => roleRepo = new RoleRepo();

        async Task<IdentityResult> IRoleStore<Role>.CreateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await roleRepo.SaveAsync(role);
            await roleRepo.CommitChangesAsync();

            return IdentityResult.Success;
        }

        async Task<IdentityResult> IRoleStore<Role>.UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var roleGot = await roleRepo.RetrieveAsync(role.Id);
            //roleGot.UpdateFromDetached(role);
            await roleRepo.CommitChangesAsync();
            return IdentityResult.Success;
        }

        async Task<IdentityResult> IRoleStore<Role>.DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await roleRepo.DeleteAsync(role);
            await roleRepo.CommitChangesAsync();

            return IdentityResult.Success;
        }

        Task<string> IRoleStore<Role>.GetRoleIdAsync(Role role, CancellationToken cancellationToken) =>
            Task.FromResult(role.Id.ToString());


        Task<string> IRoleStore<Role>.GetRoleNameAsync(Role role, CancellationToken cancellationToken) =>
            Task.FromResult(role.Name);


        Task IRoleStore<Role>.SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        Task<string> IRoleStore<Role>.GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken) =>
            Task.FromResult(role.NormalizedName);

        Task IRoleStore<Role>.SetNormalizedRoleNameAsync(
            Role role, string normalizedName, CancellationToken cancellationToken
        )
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        async Task<Role> IRoleStore<Role>.FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var role = await roleRepo.RetrieveAsync(int.Parse(roleId));
            return role;
        }

        async Task<Role> IRoleStore<Role>.FindByNameAsync(
            string normalizedRoleName, CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            var role =
                    await roleRepo.RetrieveAllLazily()
                    .SingleOrDefaultAsync(r => r.NormalizedName == normalizedRoleName);

            return role;
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }
    }
}
