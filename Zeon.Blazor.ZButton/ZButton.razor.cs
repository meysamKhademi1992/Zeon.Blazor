using Microsoft.AspNetCore.Components;

namespace Zeon.Blazor.ZButton
{
    public partial class ZButton : ComponentBase
    {
        private string _disabled = "";
        private bool _isDisabled = false;

        [Parameter]
        public string? Id { get; set; }

        [Parameter]
        public bool IsWaiting { get; set; } = false;

        [Parameter]
        public bool IsDisabled { get => _isDisabled; set => SetIsDisabled(value); }

        [Parameter]
        public string? Text { get; set; }

        [Parameter]
        public string CssClass { get; set; } = "btn-outline-primary";

        [Parameter]
        public string CssIcon { get; set; } = "zf zf-check";

        [Parameter]
        public string Display { get; set; } = "flex";

        [Parameter]
        public string Width { get; set; } = "100%";

        [Parameter]
        public string Height { get; set; } = "auto";

        [Parameter, EditorRequired]
        public EventCallback<string?> Onclick { get; set; }

        private void SetIsDisabled(bool value)
        {
            _isDisabled = value;
            _disabled = value ? " disabled " : "";
        }

        private async void OnClick()
        {
            await Onclick.InvokeAsync(Id);
        }
    }
}
