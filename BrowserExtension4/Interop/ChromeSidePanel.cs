using JsBind.Net;

namespace BrowserExtension4.Interop;

public sealed class ChromeSidePanel : ObjectBindingBase
{
    public ChromeSidePanel(IJsRuntimeAdapter jsRuntime)
    {
        SetAccessPath("chrome.sidePanel");
        Initialize(jsRuntime);
    }

    public ValueTask OpenInTabAsync(int tabId) => InvokeVoidAsync("open", new { tabId });

    public ValueTask OpenInWindowAsync(int windowId) => InvokeVoidAsync("open", new { windowId });
}