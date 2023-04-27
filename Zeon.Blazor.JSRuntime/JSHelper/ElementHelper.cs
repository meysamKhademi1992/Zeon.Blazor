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
            await _jsRuntime.InvokeVoidAsync("zeonFocusElementById", id);
        }
        public async Task ScrollToElementById(string elementId)
        {
            await _jsRuntime.InvokeVoidAsync("zeonScrollToElementById", elementId);
        }

        public async Task ScrollToElementById(string elementId, string itemId)
        {
            await _jsRuntime.InvokeVoidAsync("zeonScrollToElementById", elementId, itemId);
        }

        public async Task AddClassById(string elementId, string className)
        {
            await _jsRuntime.InvokeVoidAsync("zeonAddClassById", elementId, className);
        }
        public async Task RemoveClassById(string elementId, string className)
        {
            await _jsRuntime.InvokeVoidAsync("zeonRemoveClassById", elementId, className);
        }
        public async Task ToggleClassById(string elementId, string className)
        {
            await _jsRuntime.InvokeVoidAsync("zeonToggleClassById", elementId, className);
        }
    }
}
