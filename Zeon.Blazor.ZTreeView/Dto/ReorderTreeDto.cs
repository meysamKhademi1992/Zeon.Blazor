namespace Zeon.Blazor.ZTreeView.Dto
{
    public class ReorderTree
    {
        public ReorderTree()
        {
            ChangedNode = new TreeViewModel();
            OldParent = new TreeViewModel();
            NextParent = new TreeViewModel();
        }

        public TreeViewModel ChangedNode { get; set; }
        public TreeViewModel? OldParent { get; set; }
        public TreeViewModel? NextParent { get; set; }
    }

}
