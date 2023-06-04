namespace Zeon.Blazor.ZTreeView.Dto
{
    public class ReorderTree
    {
        public ReorderTree()
        {
            ChangedNode = new TreeViewModel();
        }

        public TreeViewModel ChangedNode { get; set; }
        public Node? OldParent { get; set; }
        public Node? NewParent { get; set; }
    }

    public class Node
    {
        public Node()
        {
            Children = new List<TreeViewModel>();
        }
        public TreeViewModel Item { get; set; } = null!;

        public List<TreeViewModel> Children { get; set; }

    }

}
