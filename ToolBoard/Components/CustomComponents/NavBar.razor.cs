using Microsoft.AspNetCore.Components;

namespace ToolBoard.Components.CustomComponents
{
    public partial class NavBar
    {
        [Parameter]
        public string SearchText { get; set; } = "";
        [Parameter]
        public EventCallback<string> SearchTextChanged { get; set; }

        [Parameter]
        public EventCallback OnClickAddJobButton { get; set; }

        private async Task OnClickAddJobButtonHandler() =>
            await OnClickAddJobButton.InvokeAsync();
    }
}
