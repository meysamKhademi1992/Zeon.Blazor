using Microsoft.JSInterop;

namespace Zeon.Blazor.JSRuntime
{
    public class ElementHelper
    {
        private readonly IJSRuntime _jsRuntime;

        public ElementHelper(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task FocusElementById(string id)
        {
            await _jsRuntime.InvokeVoidAsync("ZeonFocusElementById", id);
        }

        public async Task ScrollToElementById(string elementId, string itemId)
        {
            await _jsRuntime.InvokeVoidAsync("ZeonScrollToElementById", elementId, itemId);
        }
    }
}
