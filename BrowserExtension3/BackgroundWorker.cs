using Blazor.BrowserExtension;
using BrowserExtension3.Interop;
using Microsoft.AspNetCore.Components;
using WebExtensions.Net.Menus;
using WebExtensions.Net.Tabs;
using MenuCreateProperties = WebExtensions.Net.Menus.CreateProperties;

namespace BrowserExtension3;

public partial class BackgroundWorker : BackgroundWorkerBase
{
    private static readonly MenuCreateProperties Extension3 = new()
    {
        Id = nameof(Extension3),
        Title = nameof(Extension3),
        Contexts = [ContextType.Selection]
    };
    private static readonly MenuCreateProperties OpenSidePanel = new()
    {
        Id = nameof(OpenSidePanel),
        Title = nameof(OpenSidePanel),
        Contexts = [ContextType.Page]
    };

    [Inject]
    public ChromeSidePanel ChromeSidePanel { get; set; } = null!;

    [BackgroundWorkerMain]
    public override void Main()
    {
        WebExtensions.Runtime.OnInstalled.AddListener(OnInstalled);

        // Adding this here will generate error: `Error: `sidePanel.open()` may only be called in response to a user gesture.`
        WebExtensions.ContextMenus.OnClicked.AddListener(OnContextMenuClicked);
    }

    async Task OnInstalled()
    {
        WebExtensions.ContextMenus.Create(Extension3);
        WebExtensions.ContextMenus.Create(OpenSidePanel);

        var indexPageUrl = WebExtensions.Runtime.GetURL("index.html");
        await WebExtensions.Tabs.Create(new()
        {
            Url = indexPageUrl
        });
    }

    private async void OnContextMenuClicked(OnClickData onClickData, Tab tab)
    {
        if (onClickData.MenuItemId.Value as string == Extension3.Id && !string.IsNullOrWhiteSpace(onClickData.SelectionText))
        {
            await CreateNewTabAsync(onClickData.SelectionText);
        }

        if (onClickData.MenuItemId.Value as string == OpenSidePanel.Id && tab.WindowId.HasValue)
        {
            await ChromeSidePanel.OpenInWindowAsync(tab.WindowId.Value);
        }
    }

    private async Task CreateNewTabAsync(string selectionText)
    {
        _ = await WebExtensions.Tabs.Create(new()
        {
            Url = "https://google.com"
        });

        Console.WriteLine(selectionText);
    }
}