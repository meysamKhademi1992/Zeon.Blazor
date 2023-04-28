using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Zeon.Blazor.ZInputContainer
{
    public partial class ZInputContainer<TModel> : ComponentBase where TModel : class
    {
        private string DisplayMember => GetDisplayName(For);

        /// <summary>
        /// Lable For Id
        /// </summary>
        [Parameter]
        public string? Id { get; set; } = null;

        [Parameter]
        public string? CssIcon { get; set; }

        [Parameter, EditorRequired]
        public string For { get; set; } = null!;

        /// <summary>
        /// Display Mode block Or flex Default = block 
        /// </summary>
        [Parameter]
        public string? Display { get; set; } = "block";


        /// <summary>
        ///  Default = 100% 
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "100%";

        /// <summary>
        ///  Default = 100% 
        /// </summary>
        [Parameter]
        public string Height { get; set; } = "auto";

        [Parameter]
        public RenderFragment ChildContent { get; set; } = null!;

        private string GetDisplayName(string propertyName)
        {
            try
            {
                MemberInfo? myProperty = typeof(TModel).GetProperty(propertyName) as MemberInfo;
                var attribute = myProperty?.GetCustomAttribute(typeof(System.ComponentModel.DisplayNameAttribute)) as System.ComponentModel.DisplayNameAttribute;
                return attribute?.DisplayName ?? propertyName;
            }
            catch
            {
                return propertyName;
            }

        }
    }
}
