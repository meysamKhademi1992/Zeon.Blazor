
namespace Zeon.Blazor.ZTreeView
{
    public class TreeViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
        public bool Expanded { get; set; }
        public int Order { get; set; }
    }
}
