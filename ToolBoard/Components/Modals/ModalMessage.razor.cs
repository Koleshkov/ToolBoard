using Microsoft.AspNetCore.Components;

namespace ToolBoard.Components.Modals
{
    public partial class ModalMessage
    {
        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public EventCallback<bool> IsVisibleChanged { get; set; }

        [Parameter]
        public string MessageTitle { get; set; } = "";

        [Parameter]
        public EventCallback<string> MessageTitleChanged { get; set; }

        [Parameter]
        public string Message { get; set; } = "";

        [Parameter]
        public EventCallback<string> MessageChanged { get; set; } 
        private async Task Close()
        {
            IsVisible = false;
            await IsVisibleChanged.InvokeAsync(IsVisible);
        }
    }
}
