using Microsoft.AspNetCore.Components;

namespace Zeon.Blazor.ZAlertDialog
{
    public partial class ZAlertDialog : ComponentBase
    {
        private string _modalDisplay = "none";
        private string _modalClass = "";
        private bool _showBackdrop = false;
        private readonly Dictionary<string, string> _buttonsSize;

        public ZAlertDialog()
        {
            _buttonsSize = new Dictionary<string, string>()
            {
                {"BtnOkWidth","130px" },
                {"BtnOkHeight","auto" },
                {"BtnYesWidth","130px" },
                {"BtnYesHeight","auto" },
                {"BtnNoWidth","130px" },
                {"BtnNoHeight","auto" },
                {"BtnCancelWidth","130px" },
                {"BtnCancelHeight","auto" },
            };
        }

        [Parameter]
        public string? Caption { get; set; }

        [Parameter]
        public string? Text { get; set; }

        [Parameter]
        public string? AlertDialogCssIcon { get; set; } = "zf zf-info";

        [Parameter]
        public string? BtnOkText { get; set; } = "تایید";

        [Parameter]
        public string? BtnOkCssIcon { get; set; } = "zf zf-check";

        [Parameter]
        public string? BtnCancelText { get; set; } = "لغو";

        [Parameter]
        public string? BtnCancelCssIcon { get; set; } = "zf zf-anti-clockwise-dir-semicircular-arrow";

        [Parameter]
        public string? BtnYesText { get; set; } = "بله";

        [Parameter]
        public string? BtnYesCssIcon { get; set; } = "zf zf-check";

        [Parameter]
        public string? BtnNoText { get; set; } = "خیر";
        [Parameter]
        public string? BtnNoCssIcon { get; set; } = "zf zf-ballot";

        [Parameter]
        public Dictionary<string, string> ButtonsSize { get => _buttonsSize; set => OverrideButtonSize(value); }

        [Parameter]
        public Constants.AlertDialogButtons AlertDialogButtons { get; set; }

        [Parameter]
        public EventCallback<Constants.DialogResult> BtnOnclick { get; set; }

        public Constants.DialogResult DialogResult { get; set; }

        private void OverrideButtonSize(Dictionary<string, string> sizes)
        {
            foreach (var item in sizes)
            {
                if (_buttonsSize.Keys.Any(q => q.Equals(item.Key)))
                    _buttonsSize[item.Key] = item.Value;
            }
        }
        public async Task<Constants.DialogResult> ShowDialogAsync()
        {
            DialogResult = Constants.DialogResult.None;
            _showBackdrop = true;
            StateHasChanged();
            while (_showBackdrop)
                await Task.Delay(250);

            return DialogResult;
        }

        public void CloseDialog()
        {
            _showBackdrop = false;
            StateHasChanged();
        }

        private async void Close_OnClick()
        {
            await InvokeBtnClickedAndClose(Constants.DialogResult.Cancel);
        }

        private async Task Ok_OnClick()
        {
            await InvokeBtnClickedAndClose(Constants.DialogResult.Ok);
        }
        private async Task Cancel_OnClick()
        {
            await InvokeBtnClickedAndClose(Constants.DialogResult.Cancel);
        }
        private async Task Yes_OnClick()
        {
            await InvokeBtnClickedAndClose(Constants.DialogResult.Yes);
        }
        private async Task No_OnClick()
        {
            await InvokeBtnClickedAndClose(Constants.DialogResult.No);
        }

        private async Task InvokeBtnClickedAndClose(Constants.DialogResult dialogResult)
        {
            DialogResult = dialogResult;

            if (BtnOnclick.HasDelegate)
                await BtnOnclick.InvokeAsync(DialogResult);

            CloseDialog();
        }

    }
}
