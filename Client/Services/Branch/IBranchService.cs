using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Client.Services;

public interface IBranchService
{
    ValueTask<Guid> GetBranchID();
    ValueTask<Branch[]> GetBranches();
    ValueTask<Branch> GetBranch(Guid id);
    ValueTask<bool> AddBranch(Branch branch);
    ValueTask<bool> EditBranch(Guid id, Branch branch);
    ValueTask<bool> DeleteBranch(Guid id);
}