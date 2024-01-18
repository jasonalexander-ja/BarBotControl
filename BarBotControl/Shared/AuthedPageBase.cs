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
    private SessionService _sessionService { get; set; }
    [Inject]
    private ILocalStorageService _localStorage { get; set; }
    [Inject]
    private NavigationManager _navigation { get; set; }
    [Inject]
    private ISnackbar _snackbar { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        string token = await _localStorage.GetItemAsStringAsync(SessionKeys.SessionTokenKey) ?? "";

        var isAuthed = false;
        try
        {
            isAuthed = _sessionService.CheckSession(token);
        }
        catch (TimedOutException)
        {
            _snackbar.Add("Session timed out. ", Severity.Warning);
            _navigation.NavigateTo("SudoIn");
            return;
        }
        if (!isAuthed)
        {
            _navigation.NavigateTo("SudoIn");
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