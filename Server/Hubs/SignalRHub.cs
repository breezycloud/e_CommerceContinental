using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace e_CommerceContinental.Server.Hubs;

public class SignalRHub : Hub
{
    public async Task UpdateEntryControl()
    {
        await Clients.All.SendAsync("UpdateControls");
    }    

    public async Task UpdateNotificationsMessage()
    {
        await Clients.All.SendAsync("UpdateNotifications");
    }
    public async Task UpdateCookEntry()
    {
        await Clients.All.SendAsync("EntryEnabled");
    }    

    public async Task UpdateCategoryTable()
    {
        await Clients.All.SendAsync("UpdateCategories");
    }

    public async Task UpdateMenusData() => await Clients.All.SendAsync("LoadMenus");
    public async Task UpdateBranchTable() => await Clients.All.SendAsync("UpdateBranches");
    public async Task UpdateBanksTable() => await Clients.All.SendAsync("UpdateBanks");
    public async Task UpdateDiagram() => await Clients.All.SendAsync("UpdateDiagramImage");
}