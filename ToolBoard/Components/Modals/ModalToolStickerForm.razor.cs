using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToolBoard.Data;
using ToolBoard.Data.Entities;

namespace ToolBoard.Components.Modals
{
    public partial class ModalToolStickerForm
    {
        [Inject]
        public IDbContextFactory<AppDbContext> DbContextFactory { get; set; } = default!;

        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public EventCallback<bool> IsVisibleChanged { get; set; }
        
        [Parameter]
        public EventCallback OnUpdateJobList { get; set; }

        [Parameter]
        public Job Job { get; set; } = new();

        [Parameter]
        public EventCallback<Job> JobChanged { get; set; }

        [Parameter]
        public ToolSticker ToolSticker { get; set; } = new();

        [Parameter]
        public EventCallback<ToolSticker> ToolStickerChanged { get; set; }

        private async Task Close()
        {
            IsVisible = false;
            await IsVisibleChanged.InvokeAsync(IsVisible);
        }


        private async Task AddToolStickerAsync()
        {
            if (ToolSticker.Id.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                using (var context = await DbContextFactory.CreateDbContextAsync())
                {
                        Job.ToolStickers.Add(ToolSticker);

                        context.Jobs.Update(Job);

                        await context.SaveChangesAsync();
                }
            }
            else
            {
                using (var context = await DbContextFactory.CreateDbContextAsync())
                {
                    context.Jobs.Update(ToolSticker.Job);

                    await context.SaveChangesAsync();
                }
            }

            await Close();

            await OnUpdateJobList.InvokeAsync();
        }
    }
}
