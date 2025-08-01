using Blazor.BrowserExtension;
using WebExtensions.Net.Menus;
using WebExtensions.Net.Tabs;
using MenuCreateProperties = WebExtensions.Net.Menus.CreateProperties;

namespace BrowserExtension2;

public partial class BackgroundWorker : BackgroundWorkerBase
{
    private static readonly MenuCreateProperties CopyText = new()
    {
        Id = nameof(CopyText),
        Title = "Copy",
        Contexts = [ContextType.Selection]
    };

    [BackgroundWorkerMain]
    public override void Main()
    {
        WebExtensions.Runtime.OnInstalled.AddListener(OnInstalled);
    }

    async Task OnInstalled()
    {
        var indexPageUrl = WebExtensions.Runtime.GetURL("index.html");
        await WebExtensions.Tabs.Create(new()
        {
            Url = indexPageUrl
        });

        WebExtensions.ContextMenus.Create(CopyText);

        WebExtensions.ContextMenus.OnClicked.AddListener(OnContextMenuClicked);
    }

    private async void OnContextMenuClicked(OnClickData onClickData, Tab tab)
    {
        if (!tab.WindowId.HasValue)
        {
            return;
        }

        if (onClickData.MenuItemId.Value as string == CopyText.Id && !string.IsNullOrWhiteSpace(onClickData.SelectionText))
        {
            await CreateNewTabAsync(onClickData.SelectionText);
        }
    }

    private async Task CreateNewTabAsync(string selectionText)
    {
        _ = await WebExtensions.Tabs.Create(new()
        {
            Url = "https://mingyaulee.github.io/Blazor.BrowserExtension"
        });

        Console.WriteLine(selectionText);
    }
}