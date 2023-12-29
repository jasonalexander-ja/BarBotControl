using Microsoft.AspNetCore.Components;
using BarBotControl.Services;
using BarBotControl.Config;
using Blazored.LocalStorage;
using BarBotControl.Exceptions.SudoUser;
using MudBlazor;

namespace BarBotControl.Shared;

public class AuthedPageBase : ComponentBase
{
    [Inject]
    private SessionService SessionService { get; set; }
    [Inject]
    private ILocalStorageService LocalStorage { get; set; }
    [Inject]
    private NavigationManager Navigation { get; set; }
    [Inject]
    private ISnackbar Snackbar { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        string token = await LocalStorage.GetItemAsStringAsync(SessionKeys.SessionTokenKey) ?? "";

        var isAuthed = false;
        try
        {
            isAuthed = SessionService.CheckSession(token);
        }
        catch (TimedOutException)
        {
            Snackbar.Add("Session timed out. ", Severity.Warning);
            Navigation.NavigateTo("SudoIn");
            return;
        }
        if (!isAuthed)
        {
            Navigation.NavigateTo("SudoIn");
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