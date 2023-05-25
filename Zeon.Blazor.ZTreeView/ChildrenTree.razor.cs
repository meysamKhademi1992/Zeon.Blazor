using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Zeon.Blazor.ZTreeView.Constants;

namespace Zeon.Blazor.ZTreeView
{
    public partial class ChildrenTree : ComponentBase
    {
        [Parameter]
        public Func<TreeViewModel, bool> Filter { get; set; } = null!;

        [Parameter]
        public Func<int, string> GetItemId { get; set; } = null!;

        [Parameter]
        public Func<IEnumerable<TreeViewModel>, TreeViewModel, string> GetCheckBoxClassMode { get; set; } = null!;

        [Parameter]
        public Func<IEnumerable<TreeViewModel>, TreeViewModel, string> GetExpandedClassMode { get; set; } = null!;

        [Parameter]
        public Action<DragEventArgs, TreeViewModel> OnDrag { get; set; } = null!;

        [Parameter]
        public Action<DragEventArgs, TreeViewModel> OnDrop { get; set; } = null!;

        [Parameter]
        public Func<int, string> GetSelectedClassMode { get; set; } = null!;

        [Parameter]
        public Func<int, string> GetDragOnEnterTopClass { get; set; } = null!;

        [Parameter]
        public Func<int, string> GetDragOnEnterBottomClass { get; set; } = null!;

        [Parameter]
        public EventCallback<int> CheckedOnClick { get; set; }

        [Parameter]
        public EventCallback<int> ExpandedOnClick { get; set; }

        [Parameter]
        public EventCallback<int> SelectedOnClick { get; set; }

        [Parameter]
        public EventCallback<(DragEventArgs, DragToPosation, int)> HandleOnDragEnter { get; set; }

        [Parameter]
        public EventCallback HandleOnDragLeave { get; set; }

        [Parameter]
        public IEnumerable<TreeViewModel> Data { get; set; } = null!;

        [Parameter]
        public IEnumerable<TreeViewModel> Items { get; set; } = null!;

        [Parameter]
        public bool ShowCheckedBox { get; set; } = true;
    }
}
