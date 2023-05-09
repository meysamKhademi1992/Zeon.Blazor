using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Zeon.Blazor.ZTreeView
{
    public partial class ZTreeView<TValue> : ComponentBase
    {
        private readonly Dictionary<string, string> _fieldsSetting;
        private bool _dataIsMapped = false;

        [Inject]
        protected JSRuntime.ElementHelper ElementHelper { get; set; } = null!;


        public ZTreeView()
        {
            DataSource = new List<TValue>();
            _data = new List<TreeViewModel>();
            _fieldsSetting = new Dictionary<string, string>();
        }


        [Parameter]
        public string Name { get; set; } = "TreeView1";

        [Parameter]
        public IEnumerable<TValue> DataSource { get; set; }

        [Parameter]
        public bool AutoCheckChildren { get; set; } = true;

        [Parameter]
        public FieldsMapSettings? FieldsMapSettings { get; set; }


        private IEnumerable<TreeViewModel> _data;

        private IEnumerable<TreeViewModel> Data { get => MapData(); set => _data = value; }

        private IEnumerable<TreeViewModel> MapData()
        {
            if (_dataIsMapped)
                return _data;
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

    }

}
