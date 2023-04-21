using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection;
using Zeon.Blazor.JSRuntime;

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
        public bool AutoCheckedChildren { get; set; }

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

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
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
            }
            base.OnAfterRender(firstRender);
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

        public string GetDataJson()
        {
            foreach (var item in Data)
            {
                //GenericListChangeValue<TValue>(DataSource,)
                //GenericListChangeValue<TValue>(DataSource,)
                //GenericListChangeValue<TValue>(DataSource,)
            }
            return System.Text.Json.JsonSerializer.Serialize(DataSource);
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

            if (AutoCheckedChildren)
            {
                var childData = Data.Where(q => q.ParentId == id).ToList();
                CheckedChildren(childData, Data, isChecked);
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
    }

}
