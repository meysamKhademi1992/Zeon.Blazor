using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Zeon.Blazor.ZTreeView
{
    public partial class FieldsSettings : ComponentBase
    {
        [Parameter]
        public string? Id { get; set; }

        public Dictionary<string, string> GetMapping()
        {
            return new Dictionary<string, string>
            {
                { "Id", Id??"" },
            };
        }
    }
}
