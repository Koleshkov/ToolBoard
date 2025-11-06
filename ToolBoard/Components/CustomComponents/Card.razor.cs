using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using ToolBoard.Data;
using ToolBoard.Data.Entities;

namespace ToolBoard.Components.CustomComponents
{
    public partial class Card
    {
        [Inject]
        public IDbContextFactory<AppDbContext> DbContextFactory { get; set; } = default!;
        [Parameter]
        public Job Job { get; set; } = new();
        [Parameter]
        public EventCallback<Job> OnDeleteJob { get; set; }
        [Parameter]
        public EventCallback<Job> OnEditJob { get; set; }

        [Parameter]
        public EventCallback OnAddToolSticker { get; set; }

        [Parameter]
        public EventCallback OnAddSurfaceSticker { get; set; }

        [Parameter]
        public EventCallback OnUpdateJobList { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        public async Task OnDeleteHandler() =>
                await OnDeleteJob.InvokeAsync();

        public async Task OnEditHandler() {
            await OnEditJob.InvokeAsync();
            StateHasChanged();
        }
               

        public async Task OnAddToolHandler() =>
                await OnAddToolSticker.InvokeAsync();

        public async Task OnAddSurfaceHandler() =>
                await OnAddSurfaceSticker.InvokeAsync();

        private async Task Update(Job job)
        {
            using (var context = await DbContextFactory.CreateDbContextAsync())
            {
                context.Jobs.Update(Job);
                await context.SaveChangesAsync();

                await OnUpdateJobList.InvokeAsync();
            }
        }
    }
}
