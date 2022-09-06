using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Client.Services;

public interface IRoleService
{
    ValueTask<Role[]> GetRoles();
    ValueTask<Role[]> GetRolesExcludingMaster();
    ValueTask<Role> GetRole(Guid id);
    ValueTask<bool> AddRole(Role role);
    ValueTask<bool> EditRole(Guid id, Role role);
    ValueTask<bool> DeleteRole(Guid id);
}