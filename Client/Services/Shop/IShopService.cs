using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Client.Services;

interface IShopService
{
    ValueTask<bool> AddShop(Shop shop);
    ValueTask<bool> EditShop(Shop shop);
    ValueTask<bool> DeleteShop(Guid shop);
    ValueTask<Shop[]> GetShops();
    ValueTask<Shop[]> GetShopsByBranch(Guid BranchID);
    ValueTask<Shop> GetShop(Guid id);

}