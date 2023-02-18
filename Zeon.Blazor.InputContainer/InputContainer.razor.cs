﻿using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Zeon.Blazor.InputContainer
{
    public partial class InputContainer<TModel> : ComponentBase
    {
        private string DisplayMember => GetDisplayName(For);

        /// <summary>
        /// Lable For Id
        /// </summary>
        [Parameter]
        public string Id { get; set; } = null!;

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public bool? IsLoading { get; set; }

        [Parameter] public string For { get; set; } = null!;

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
        public string Height { get; set; } = "100%";

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
