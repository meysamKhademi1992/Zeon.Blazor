using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Zeon.Blazor.ZTreeView
{
    public partial class ChildrenTree : ComponentBase
    {
        [Parameter]
        public Func<int, string> GetItemId { get; set; }

        [Parameter]
        public EventCallback<int> CheckedOnClick { get; set; }

        [Parameter]
        public EventCallback<int> ExpandedOnClick { get; set; }

        [Parameter]
        public IEnumerable<TreeViewModel> DataSource { get; set; }

        [Parameter]
        public IEnumerable<TreeViewModel> Items { get; set; }

    }
}
