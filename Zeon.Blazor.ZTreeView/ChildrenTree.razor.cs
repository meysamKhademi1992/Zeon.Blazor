using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Zeon.Blazor.ZTreeView.Constants;

namespace Zeon.Blazor.ZTreeView
{
    public partial class ChildrenTree : ComponentBase
    {

        [Parameter]
        public IEnumerable<TreeViewModel> Data { get; set; } = null!;

        [Parameter]
        public IEnumerable<TreeViewModel> Items { get; set; } = null!;

        [Parameter]
        public Func<TreeViewModel, bool> Filter { get; set; } = null!;

        [Parameter]
        public Func<int, string> GetItemId { get; set; } = null!;

        [Parameter]
        public Func<IEnumerable<TreeViewModel>, TreeViewModel, string> GetCheckBoxClassMode { get; set; } = null!;

        [Parameter]
        public Func<IEnumerable<TreeViewModel>, TreeViewModel, string> GetExpandedClassMode { get; set; } = null!;

        [Parameter]
        public Action<DragEventArgs, TreeViewModel> OnDragStart { get; set; } = null!;

        [Parameter]
        public Action<DragEventArgs, TreeViewModel> OnDragEnd { get; set; } = null!;

        [Parameter]
        public Action<DragEventArgs, DragToPosition, TreeViewModel> OnDropStart { get; set; } = null!;

        [Parameter]
        public Func<int, string> GetSelectedClassMode { get; set; } = null!;

        [Parameter]
        public Func<TreeViewModel, string> GetDragOnEnterTopClass { get; set; } = null!;

        [Parameter]
        public Func<TreeViewModel, string> GetDragOnEnterBottomClass { get; set; } = null!;

        [Parameter]
        public Func<TreeViewModel, string> GetDragOnEnterIntoClass { get; set; } = null!;

        [Parameter]
        public Func<int?, string> GetDragOnEnterGroupClass { get; set; } = null!;

        [Parameter]
        public EventCallback<int> CheckedOnClick { get; set; }

        [Parameter]
        public EventCallback<int> ExpandedOnClick { get; set; }

        [Parameter]
        public EventCallback<int> SelectedOnClick { get; set; }

        [Parameter]
        public EventCallback<(DragEventArgs, DragToPosition, TreeViewModel)> HandleOnDragEnter { get; set; }

        [Parameter]
        public EventCallback<(DragEventArgs, DragToPosition, TreeViewModel)> HandleOnDragLeave { get; set; }

        [Parameter]
        public Action<TreeViewModel, string, ChangeState> OnDataChanged { get; set; } = null!;

        [Parameter]
        public Action<TreeViewModel, ChangeState> DataChangeOnClick { get; set; } = null!;

        [Parameter]
        public EventCallback<TreeViewModel> RemoveItemOnClick { get; set; }

        [Parameter]
        public (int id, ChangeState state) ChangeItemState { get; set; }

        [Parameter]
        public int ItemId { get; set; } = -1;

        [Parameter]
        public string DropElementsDisplay { get; set; } = null!;

        [Parameter]
        public bool ShowCheckedBox { get; set; } = true;
    }
}
