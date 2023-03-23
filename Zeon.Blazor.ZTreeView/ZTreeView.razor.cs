using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Zeon.Blazor.ZTreeView
{
    public partial class ZTreeView<TValue> : ComponentBase
    {
        public ZTreeView()
        {
            DataSource = new List<TValue>();
        }

        [Parameter]
        public IEnumerable<TValue> DataSource { get; set; }

        [Parameter]
        public RenderFragment? FieldsSettings { get; set; }

        private IEnumerable<Model> Data { get => GetData(); }

        private IEnumerable<Model> GetData()
        {
            var fieldsMapSettings =  ;
            var list = new Mapper<TValue>(fieldsMapSettings).CreateMap(DataSource);
            return list;
        }
        private RenderFragment BuildTree(IEnumerable<Model> dataSource) => builder =>
        {
            int sequence = 0;
            foreach (var item in dataSource.Where(q => q.ParentId == null).ToList())
            {
                builder.OpenElement(sequence++, "ul");
                builder.OpenElement(sequence++, "li");
                builder.AddContent(sequence++, item.Text);
                BuildChildrenTree(builder, ref sequence, dataSource.Where(q => q.ParentId == item.Id).AsEnumerable(), dataSource);
                builder.CloseElement();
                builder.CloseElement();
            }

        };

        private void BuildChildrenTree(RenderTreeBuilder builder, ref int index, IEnumerable<Model> children, IEnumerable<Model> dataSource)
        {
            foreach (var item in children)
            {
                builder.OpenElement(index++, "ul");
                builder.OpenElement(index++, "li");
                builder.AddContent(index++, item.Text);
                BuildChildrenTree(builder, ref index, dataSource.Where(q => q.ParentId == item.Id).ToList(), dataSource);
                builder.CloseElement();
                builder.CloseElement();
            }
        }
    }
    public class Model
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
        public bool IsCollapsed { get; set; }
    }



}
