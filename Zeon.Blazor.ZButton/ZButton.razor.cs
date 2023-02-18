using Microsoft.AspNetCore.Components;

namespace Zeon.Blazor.ZButton
{
    public partial class ZButton : ComponentBase
    {
        private string _disabled = "";
        private bool _isDisabled = false;

        [Parameter]
        public string? ButtonId { get; set; }

        [Parameter]
        public bool IsWaiting { get; set; } = false;

        [Parameter]
        public bool IsDisabled { get => _isDisabled; set => SetIsDisabled(value); }

        private void SetIsDisabled(bool value)
        {
            _isDisabled = value;
            _disabled = value ? " disabled " : "";
        }

        [Parameter]
        public string? Text { get; set; }

        [Parameter]
        public string ButtonClass { get; set; } = "btn-outline-primary";

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public string? Display { get; set; } = "flex";

        [Parameter]
        public string Width { get; set; } = "120px";

        [Parameter]
        public string Height { get; set; } = "auto";

        [Parameter]
        public EventCallback<string?> Onclick { get; set; }

        private async void OnClick()
        {
            await Onclick.InvokeAsync(ButtonId);
        }
    }
}
