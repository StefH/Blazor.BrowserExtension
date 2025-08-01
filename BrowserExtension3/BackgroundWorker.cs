using Blazor.BrowserExtension;
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

    [BackgroundWorkerMain]
    public override void Main()
    {
        WebExtensions.Runtime.OnInstalled.AddListener(OnInstalled);

        WebExtensions.Runtime.OnStartup.AddListener(OnStartup);
    }

    async Task OnInstalled()
    {
        var indexPageUrl = WebExtensions.Runtime.GetURL("index.html");
        await WebExtensions.Tabs.Create(new()
        {
            Url = indexPageUrl
        });
    }

    void OnStartup()
    {
        WebExtensions.ContextMenus.Create(Extension3);

        WebExtensions.ContextMenus.OnClicked.AddListener(OnContextMenuClicked);
    }

    private async void OnContextMenuClicked(OnClickData onClickData, Tab tab)
    {
        if (!tab.WindowId.HasValue)
        {
            return;
        }

        if (onClickData.MenuItemId.Value as string == Extension3.Id && !string.IsNullOrWhiteSpace(onClickData.SelectionText))
        {
            await CreateNewTabAsync(onClickData.SelectionText);
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