using Microsoft.AspNetCore.Components;
using BarBotControl.Services;
using BarBotControl.Config;
using Blazored.LocalStorage;

namespace BarBotControl.Shared;

public class AuthedPageBase : ComponentBase
{
    [Inject]
    private SessionService SessionService { get; set; }
    [Inject]
    private ILocalStorageService LocalStorage { get; set; }
    [Inject]
    private NavigationManager Navigation { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        string token = await LocalStorage.GetItemAsStringAsync(SessionKeys.SessionTokenKey) ?? "";

        var isAuthed = SessionService.CheckSession(token);
        if (!isAuthed)
        {
            Navigation.NavigateTo("sudoin");
            return;
        }

        if (firstRender)
            await GetData();
        
        await base.OnAfterRenderAsync(firstRender);
    }

    protected virtual async Task GetData()
    {
        
    }
}