using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection;
using Zeon.Blazor.JSRuntime;

namespace Zeon.Blazor.ZTreeView
{
    public partial class ZTreeView<TValue> : ComponentBase
    {
        [Inject]
        protected JSRuntime.ElementHelper ElementHelper { get; set; } = null!;

        public ZTreeView()
        {
            DataSource = new List<TValue>();
            _data = new List<Model>();
        }

        [Parameter]
        public string Name { get; set; } = "TreeView1";

        [Parameter]
        public IEnumerable<TValue> DataSource { get; set; }

        [Parameter]
        public RenderFragment? FieldsSettings { get; set; }

        private IEnumerable<Model> _data;
        private IEnumerable<Model> Data { get => MapData(); set => _data = value; }
        private bool _dataIsMapped = false;

        private string GetItemId(int id)
        {
            var elementId = string.Concat("zeon", Name, "Item") + id;
            return elementId;
        }

        private IEnumerable<Model> MapData()
        {
            if (_dataIsMapped)
                return _data;
            else
            {
                _data = new Mapper<TValue>().CreateMap(DataSource);
                _dataIsMapped = true;
                return _data;
            }
        }
        //private RenderFragment BuildTree(IEnumerable<Model> dataSource) => builder =>
        //{
        //    int sequence = 0;
        //    foreach (var item in dataSource.Where(q => q.ParentId == null).ToList())
        //    {
        //        builder.OpenElement(sequence++, "ul");
        //        builder.OpenElement(sequence++, "li");
        //        builder.AddAttribute(sequence++, "class", " zeon-tree-view-item " + (item.IsChecked ? "zeon-tree-view-item-checked-box" : "zeon-tree-view-item-box"));
        //        builder.AddAttribute(sequence++, "onclick:stopPropagation", true);
        //        builder.AddAttribute(sequence++, "onclick", EventCallback.Factory.Create(dataSource, () => OnClickEvent(item.Id)));
        //        builder.AddContent(sequence++, item.Text);
        //        BuildChildrenTree(builder, ref sequence, dataSource.Where(q => q.ParentId == item.Id).AsEnumerable(), dataSource);
        //        builder.CloseElement();
        //        builder.CloseElement();
        //    }
        //};

        private void CheckedOnClick(int id)
        {
            var isChecked = !(Data.Where(q => q.Id == id).First().IsChecked);
            Data.Where(q => q.Id == id).First().IsChecked = isChecked;

            if (true)
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

        //private static void GenericListChangeValue<T>(IEnumerable<T> dataSource, string idName, object idValue, string propertyName, object value)
        //{
        //    PropertyInfo idProp = typeof(T).GetProperty(idName)!;
        //    T model = dataSource.Where(q => idProp.GetValue(q)!.Equals(idValue)).First();
        //    PropertyInfo[] array = model!.GetType().GetProperties();
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        PropertyInfo? item = array[i];
        //        if (item.Name == propertyName && item.PropertyType == value.GetType() && item.CanWrite)
        //        {
        //            item.SetValue(model, value);
        //            break;
        //        }
        //    }
        //}
        private void CheckedChildren(IEnumerable<Model> data, IEnumerable<Model> dataSource, bool isChecked)
        {
            Model[] array = data.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                Model? item = array[i];
                item.IsChecked = isChecked;

                CheckedChildren(dataSource.Where(q => q.ParentId == item.Id), dataSource, isChecked);
            }
        }

        //private void BuildChildrenTree(RenderTreeBuilder builder, ref int sequence, IEnumerable<Model> children, IEnumerable<Model> dataSource)
        //{
        //    foreach (var item in children)
        //    {
        //        builder.OpenElement(sequence++, "ul");
        //        builder.OpenElement(sequence++, "li");
        //        builder.AddAttribute(sequence++, "class", " zeon-tree-view-item " + (item.IsChecked ? "zeon-tree-view-item-checked-box" : "zeon-tree-view-item-box"));
        //        builder.AddAttribute(sequence++, "onclick:stopPropagation", true);
        //        builder.AddAttribute(sequence++, "onclick", EventCallback.Factory.Create(dataSource, () => OnClickEvent(item.Id)));
        //        builder.AddContent(sequence++, item.Text);
        //        BuildChildrenTree(builder, ref sequence, dataSource.Where(q => q.ParentId == item.Id).ToList(), dataSource);
        //        builder.CloseElement();
        //        builder.CloseElement();
        //    }
        //}
    }
    public class Model
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
        public bool Expanded { get; set; }
    }



}
