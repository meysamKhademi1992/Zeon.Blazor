using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection;
using Zeon.Blazor.ZTreeView.Constants;

namespace Zeon.Blazor.ZTreeView
{
    public partial class ZTreeView<TValue> : ComponentBase
    {
        private const string DISPLAY_BLOCK = "block";
        private const string DISPLAY_NONE = "none";
        private readonly Dictionary<string, string> _fieldsSetting;
        private bool _dataIsMapped = false;
        private int _selectedId = 0;
        private int _dragEnteredToTopItemId = 0;
        private int _dragEnteredToBottomItemId = 0;
        private int _dragEnteredToIntoItemId = 0;
        private int? _dragEnteredToGroupParentId = -1;
        private string _dropElementsDisplay = DISPLAY_NONE;
        private TreeViewModel? _draggedItem = null;
        private List<int> _draggedItemdChildIds = new();
        private Func<TreeViewModel, bool> _filter = q => true;

        [Inject]
        protected JSRuntime.ElementHelper ElementHelper { get; set; } = null!;


        [Parameter, EditorRequired]
        public string Name { get; set; } = "TreeView1";

        [Parameter, EditorRequired]
        public IEnumerable<TValue> DataSource { get; set; }

        [Parameter]
        public bool AutoCheckChildren { get; set; } = true;

        [Parameter]
        public FieldsMapSettings? FieldsMapSettings { get; set; }

        [Parameter]
        public string BackgroundColor { get; set; } = "whiteSmoke";

        [Parameter]
        public bool ShowCheckedBox { get; set; } = true;

        public ZTreeView()
        {
            DataSource = new List<TValue>();
            _data = new List<TreeViewModel>();
            _fieldsSetting = new Dictionary<string, string>();
        }

        protected override void OnInitialized()
        {
            if (FieldsMapSettings is not null)
            {
                if (false == string.IsNullOrEmpty(FieldsMapSettings.Id))
                    _fieldsSetting.Add(nameof(FieldsMapSettings.Id), FieldsMapSettings.Id);
                if (false == string.IsNullOrEmpty(FieldsMapSettings.ParentId))
                    _fieldsSetting.Add(nameof(FieldsMapSettings.ParentId), FieldsMapSettings.ParentId);
                if (false == string.IsNullOrEmpty(FieldsMapSettings.Text))
                    _fieldsSetting.Add(nameof(FieldsMapSettings.Text), FieldsMapSettings.Text);
                if (false == string.IsNullOrEmpty(FieldsMapSettings.IsChecked))
                    _fieldsSetting.Add(nameof(FieldsMapSettings.IsChecked), FieldsMapSettings.IsChecked);
                if (false == string.IsNullOrEmpty(FieldsMapSettings.Expanded))
                    _fieldsSetting.Add(nameof(FieldsMapSettings.Expanded), FieldsMapSettings.Expanded);

            }

            base.OnInitialized();
        }

        public int[] GetCheckedItems()
        {
            return Data.Where(q => q.IsChecked).Select(q => q.Id).ToArray();
        }

        public int[] GetExpandedItems()
        {
            return Data.Where(q => q.Expanded).Select(q => q.Id).ToArray();
        }

        public void SetCheckedItems(int[] checkedIds)
        {
            Data.Where(q => !checkedIds.Contains(q.Id) && q.IsChecked == true).ToList().ForEach(q => q.IsChecked = false);
            Data.Where(q => checkedIds.Contains(q.Id) && q.IsChecked == false).ToList().ForEach(q => q.IsChecked = true);
        }

        public void SetExpandedItems(int[] expandedIds)
        {
            Data.Where(q => !expandedIds.Contains(q.Id) && q.Expanded == true).ToList().ForEach(q => q.Expanded = false);
            Data.Where(q => expandedIds.Contains(q.Id) && q.Expanded == false).ToList().ForEach(q => q.Expanded = true);
        }

        public string GetData()
        {
            var idName = _fieldsSetting.Where(q => q.Key == nameof(TreeViewModel.Id)).FirstOrDefault().Value ?? nameof(TreeViewModel.Id);
            var isCheckedName = _fieldsSetting.Where(q => q.Key == nameof(TreeViewModel.IsChecked)).FirstOrDefault().Value ?? nameof(TreeViewModel.IsChecked);
            var expandedName = _fieldsSetting.Where(q => q.Key == nameof(TreeViewModel.Expanded)).FirstOrDefault().Value ?? nameof(TreeViewModel.Expanded);
            foreach (var item in Data)
            {
                GenericListChangeValue<TValue>(DataSource, idName, item.Id, isCheckedName, item.IsChecked);
                GenericListChangeValue<TValue>(DataSource, idName, item.Id, expandedName, item.Expanded);
            }
            return System.Text.Json.JsonSerializer.Serialize(DataSource);
        }

        public void SetFilter(Func<TValue, bool>? value)
        {
            if (value is not null)
            {
                var idName = _fieldsSetting.Where(q => q.Key == nameof(TreeViewModel.Id)).FirstOrDefault().Value ?? nameof(TreeViewModel.Id);
                PropertyInfo idProp = typeof(TValue).GetProperty(idName)!;

                List<int> filterIds = DataSource.Where(value).Select(q => int.Parse(idProp.GetValue(q)!.ToString()!)).ToList();
                List<int> parentIds = new();
                List<int> childIds = new();

                for (int i = 0; i < filterIds.Count; i++)
                {
                    GetParentIds(ref parentIds, Data, Data.First(q => q.Id == filterIds[i]).ParentId);
                    GetChildIds(ref childIds, Data, Data.Where(q => q.ParentId == filterIds[i]).Select(q => q.Id).ToList());
                }

                List<int> filterIdsWithChildAndParent = new();
                filterIdsWithChildAndParent.AddRange(filterIds);
                filterIdsWithChildAndParent.AddRange(parentIds);
                filterIdsWithChildAndParent.AddRange(childIds);

                var distinctFilterIds = filterIdsWithChildAndParent.Distinct().ToList();

                _filter = q => distinctFilterIds.Contains(q.Id);
                Refresh();
            }
            else
            {
                _filter = q => true;
            }
        }


        private IEnumerable<TreeViewModel> _data;
        private IEnumerable<TreeViewModel> Data { get => MapData(); set => _data = value; }

        private IEnumerable<TreeViewModel> MapData()
        {
            if (_dataIsMapped)
                return _data.OrderBy(q => q.Order).ToList();
            else
            {
                _data = new Mapper<TValue>(_fieldsSetting).CreateMap(DataSource);
                _dataIsMapped = true;
                return _data;
            }
        }

        private string GetItemId(int id)
        {
            var elementId = string.Concat("zeon", Name, "Item") + id;
            return elementId;
        }

        private string GetCheckBoxClassMode(IEnumerable<TreeViewModel> data, TreeViewModel item)
        {
            if (AutoCheckChildren)
            {
                return item.IsChecked && AllChildrenIsChecked(data, item) ? "zeon-tree-view-item-checked" : ChildrenHasCheckedItem(data, item) ? "zeon-tree-view-item-children-checked-any" : "zeon-tree-view-item-box";
            }
            else
            {
                return item.IsChecked ? "zeon-tree-view-item-checked" : "zeon-tree-view-item-box";
            }
        }

        private string GetExpandedClassMode(IEnumerable<TreeViewModel> data, TreeViewModel item)
        {
            return Data.Where(q => q.ParentId == item.Id).Any() ? item.Expanded ? "zeon-tree-view-item-collapsible" : "zeon-tree-view-item-expandable" : "zeon-tree-view-item-noChildren";
        }

        private string GetSelectedClassMode(int id)
        {
            return id == _selectedId ? "zeon-tree-view-item-selected" : string.Empty;
        }

        private string GetDragOnEnterTopClass(TreeViewModel droppedItem)
        {
            var droppingIsAllowed = _draggedItem is null || DroppingIsAllowed(_draggedItem, droppedItem, DragToPosition.Top);
            return _dragEnteredToTopItemId == droppedItem.Id && droppingIsAllowed ? "zeon-tree-view-item-drag-enter-top" : droppingIsAllowed == false ? "zeon-tree-view-item-drag-and-drop-not-allowed" : string.Empty;
        }
        private string GetDragOnEnterBottomClass(TreeViewModel droppedItem)
        {
            var droppingIsAllowed = _draggedItem is null || DroppingIsAllowed(_draggedItem, droppedItem, DragToPosition.Bottom);
            return _dragEnteredToBottomItemId == droppedItem.Id && droppingIsAllowed ? "zeon-tree-view-item-drag-enter-bottom" : droppingIsAllowed == false ? "zeon-tree-view-item-drag-and-drop-not-allowed" : string.Empty;
        }
        private string GetDragOnEnterIntoClass(TreeViewModel droppedItem)
        {
            var droppingIsAllowed = _draggedItem is null || DroppingIsAllowed(_draggedItem, droppedItem, DragToPosition.Into);
            return _dragEnteredToIntoItemId == droppedItem.Id && droppingIsAllowed ? "zeon-tree-view-item-drag-enter-into" : droppingIsAllowed == false ? "zeon-tree-view-item-drag-and-drop-not-allowed" : string.Empty;
        }
        private string GetDragOnEnterGroupClass(int? parentId)
        {
            return _dragEnteredToGroupParentId == parentId ? "zeon-tree-view-item-drop-group" : string.Empty;
        }

        private bool ChildrenHasCheckedItem(IEnumerable<TreeViewModel> data, TreeViewModel item)
        {
            foreach (var childItem in data.Where(q => q.ParentId == item.Id).ToList())
            {
                if (childItem.IsChecked == true)
                    return true;
                else
                   if (ChildrenHasCheckedItem(data, childItem))
                    return true;
            }
            return false;
        }

        private bool AllChildrenIsChecked(IEnumerable<TreeViewModel> data, TreeViewModel item)
        {
            foreach (var childItem in data.Where(q => q.ParentId == item.Id).ToList())
            {
                if (childItem.IsChecked == false)
                    return false;
                else
                    if (false == AllChildrenIsChecked(data, childItem))
                    return false;
            }
            return true;
        }

        private static void GenericListChangeValue<T>(IEnumerable<T> dataSource, string idName, object idValue, string propertyName, object value)
        {
            PropertyInfo idProp = typeof(T).GetProperty(idName)!;
            T model = dataSource.Where(q => idProp.GetValue(q)!.Equals(idValue)).First();
            PropertyInfo[] array = model!.GetType().GetProperties();
            for (int i = 0; i < array.Length; i++)
            {
                PropertyInfo? item = array[i];
                if (item.Name == propertyName && item.PropertyType == value.GetType() && item.CanWrite)
                {
                    item.SetValue(model, value);
                    break;
                }
            }
        }

        private void CheckedOnClick(int id)
        {
            var isChecked = !(Data.Where(q => q.Id == id).First().IsChecked);
            Data.Where(q => q.Id == id).First().IsChecked = isChecked;

            if (AutoCheckChildren)
            {
                var childData = Data.Where(q => q.ParentId == id).ToList();
                CheckedChildren(childData, Data, isChecked);

                var parentId = Data.Where(q => q.Id == id).First().ParentId;
                if (isChecked)
                    CheckedParent(Data, parentId, isChecked);
                else
                    UnCheckedParent(Data, parentId, isChecked);
            }
        }

        private void SelectedOnClick(int id) => _selectedId = id;

        private void UnCheckedParent(IEnumerable<TreeViewModel> data, int? parentId, bool isChecked)
        {
            if (parentId.HasValue)
            {
                var item = data.Where(q => q.Id == parentId).First();
                if (false == ChildrenHasCheckedItem(data, item))
                    item.IsChecked = isChecked;

                UnCheckedParent(data, item.ParentId, isChecked);
            }
        }

        private void CheckedParent(IEnumerable<TreeViewModel> data, int? parentId, bool isChecked)
        {
            if (parentId.HasValue)
            {
                var item = data.Where(q => q.Id == parentId).First();
                item.IsChecked = isChecked;
                CheckedParent(data, item.ParentId, isChecked);
            }
        }

        private void GetParentIds(ref List<int> ids, IEnumerable<TreeViewModel> data, int? parentId)
        {
            if (parentId.HasValue)
            {
                ids.Add(parentId.Value);
                var item = data.Where(q => q.Id == parentId).First();
                GetParentIds(ref ids, data, item.ParentId);
            }
        }

        private void GetChildIds(ref List<int> ids, IEnumerable<TreeViewModel> data, List<int> childIds)
        {
            if (childIds.Any())
            {
                ids.AddRange(childIds);
                for (int i = 0; i < childIds.Count; i++)
                {
                    GetChildIds(ref ids, data, Data.Where(q => q.ParentId == childIds[i]).Select(q => q.Id).ToList());
                }
            }
        }

        private void ExpandedOnClick(int id)
        {
            var expanded = !(Data.Where(q => q.Id == id).First().Expanded);
            Data.Where(q => q.Id == id).First().Expanded = expanded;
        }

        private void CheckedChildren(IEnumerable<TreeViewModel> data, IEnumerable<TreeViewModel> dataSource, bool isChecked)
        {
            TreeViewModel[] array = data.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                TreeViewModel? item = array[i];
                item.IsChecked = isChecked;

                CheckedChildren(dataSource.Where(q => q.ParentId == item.Id), dataSource, isChecked);
            }
        }

        private bool DroppingIsAllowed(TreeViewModel draggedItem, TreeViewModel droppedItem, DragToPosition position)
        {
            switch (position)
            {
                case DragToPosition.Top:
                case DragToPosition.Bottom:
                    {
                        return !_draggedItemdChildIds.Contains(droppedItem.Id);
                    }
                case DragToPosition.Into:
                    {
                        return (draggedItem.Id != droppedItem.Id && !_draggedItemdChildIds.Contains(droppedItem.Id));

                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(DragToPosition));
            }
        }

        private void OnDrop(DragEventArgs e, DragToPosition position, TreeViewModel droppedItem)
        {
            if (_draggedItem is not null)
            {
                if (false == DroppingIsAllowed(_draggedItem, droppedItem, position))
                    return;

                switch (position)
                {
                    case DragToPosition.Top:
                        {
                            SetDropTop(_draggedItem, droppedItem);
                            break;
                        }
                    case DragToPosition.Bottom:
                        {
                            SetDropBottom(_draggedItem, droppedItem);
                            break;
                        }
                    case DragToPosition.Into:
                        {
                            SetDropInto(_draggedItem, droppedItem);
                            break;
                        }
                    default:
                        break;
                }
                _draggedItem = null;
            }
            Refresh();
        }

        private void SetDropTop(TreeViewModel draggedItem, TreeViewModel droppedItem)
        {
            var parentLevelItems = _data.Where(q => q.ParentId == droppedItem.ParentId).OrderBy(o => o.Order).ToList();
            var index = parentLevelItems.IndexOf(droppedItem);

            var itemsInTopDroppedItem = parentLevelItems.Where(q => q.Id != draggedItem.Id).TakeWhile(item => item != droppedItem).ToList();
            var itemsInBottomDroppedItem = parentLevelItems.Where(q => q.Id != draggedItem.Id).SkipWhile(item => item != droppedItem).Skip(1).ToList();

            for (int i = itemsInTopDroppedItem.Count; i > 0; i--)
            {
                var indexItem = (i - 1);
                TreeViewModel? item = itemsInTopDroppedItem[indexItem];
                item.Order = (index - 1) - (itemsInTopDroppedItem.Count - i);
            }

            for (int i = 0; i < itemsInBottomDroppedItem.Count; i++)
            {
                TreeViewModel? item = itemsInBottomDroppedItem[i];
                item.Order = (index + 2) + i;
            }

            draggedItem.Order = index;
            draggedItem.ParentId = droppedItem.ParentId;
            droppedItem.Order = index + 1;

            if (draggedItem.ParentId != droppedItem.ParentId)
            {
                ReSortOrderNumberInParentLevel(draggedItem);
            }
        }

        private void SetDropBottom(TreeViewModel draggedItem, TreeViewModel droppedItem)
        {
            var parentLevelItems = _data.Where(q => q.ParentId == droppedItem.ParentId).OrderBy(o => o.Order).ToList();
            var index = parentLevelItems.IndexOf(droppedItem);

            var itemsInTopDroppedItem = parentLevelItems.Where(q => q.Id != draggedItem.Id).TakeWhile(item => item != droppedItem).ToList();
            var itemsInBottomDroppedItem = parentLevelItems.Where(q => q.Id != draggedItem.Id).SkipWhile(item => item != droppedItem).Skip(1).ToList();

            for (int i = itemsInTopDroppedItem.Count; i > 0; i--)
            {
                var indexItem = (i - 1);
                TreeViewModel? item = itemsInTopDroppedItem[indexItem];
                item.Order = (index - 1) - (itemsInTopDroppedItem.Count - i);
            }

            for (int i = 0; i < itemsInBottomDroppedItem.Count; i++)
            {
                TreeViewModel? item = itemsInBottomDroppedItem[i];
                item.Order = index + 2 + i;
            }

            draggedItem.Order = index + 1;
            draggedItem.ParentId = droppedItem.ParentId;
            droppedItem.Order = index;

            if (draggedItem.ParentId != droppedItem.ParentId)
            {
                ReSortOrderNumberInParentLevel(draggedItem);
            }
        }

        private void SetDropInto(TreeViewModel draggedItem, TreeViewModel droppedItem)
        {
            var parentLevelItemsCount = _data.Where(q => q.ParentId == droppedItem.Id).OrderBy(o => o.Order).Count();
            draggedItem.Order = parentLevelItemsCount + 1;
            draggedItem.ParentId = droppedItem.Id;
            ReSortOrderNumberInParentLevel(draggedItem);
        }

        private void ReSortOrderNumberInParentLevel(TreeViewModel _draggedItem)
        {
            var draggedItemParentLevelItems = _data.Where(q => q.ParentId == _draggedItem.ParentId).OrderBy(o => o.Order).ToList();
            for (int i = 0; i < draggedItemParentLevelItems.Count; i++)
            {
                TreeViewModel? item = draggedItemParentLevelItems[i];
                item.Order = i + 1;
            }
        }

        private void OnDragStartHandler(DragEventArgs e, TreeViewModel draggedItem)
        {
            _dropElementsDisplay = ZTreeView<TValue>.DISPLAY_BLOCK;
            _draggedItem = draggedItem;
            GetChildIds(ref _draggedItemdChildIds, Data, Data.Where(q => q.ParentId == draggedItem.Id).Select(q => q.Id).ToList());
            Refresh();
        }

        private void OnDragEndHandler(DragEventArgs e, TreeViewModel item)
        {
            _draggedItem = null;
            _dragEnteredToGroupParentId = -1;
            _dragEnteredToTopItemId = 0;
            _dragEnteredToBottomItemId = 0;
            _dragEnteredToIntoItemId = 0;
            _dropElementsDisplay = ZTreeView<TValue>.DISPLAY_NONE;
            _draggedItemdChildIds.Clear();
            Refresh();
        }

        private void OnDragEnterHandler((DragEventArgs e, DragToPosition position, TreeViewModel item) parameters)
        {
            switch (parameters.position)
            {
                case DragToPosition.Top:
                    _dragEnteredToTopItemId = parameters.item.Id;
                    _dragEnteredToGroupParentId = parameters.item.ParentId;
                    break;
                case DragToPosition.Bottom:
                    _dragEnteredToBottomItemId = parameters.item.Id;
                    _dragEnteredToGroupParentId = parameters.item.ParentId;
                    break;
                case DragToPosition.Into:
                    _dragEnteredToIntoItemId = parameters.item.Id;
                    _dragEnteredToGroupParentId = parameters.item.ParentId;
                    break;

                default:
                    break;
            }
        }

        private void OnDragLeaveHandler((DragEventArgs e, DragToPosition position, TreeViewModel item) parameters)
        {
            switch (parameters.position)
            {
                case DragToPosition.Top:
                    {
                        _dragEnteredToTopItemId = 0;
                    }
                    break;
                case DragToPosition.Bottom:
                    {
                        _dragEnteredToBottomItemId = 0;
                    }
                    break;
                case DragToPosition.Into:
                    {
                        _dragEnteredToIntoItemId = 0;
                    }
                    break;

                default:
                    break;
            }
        }

        private void DeleteItemOnClick(TreeViewModel item)
        {

        }

        private void Refresh()
        {
            StateHasChanged();
        }
    }

}
