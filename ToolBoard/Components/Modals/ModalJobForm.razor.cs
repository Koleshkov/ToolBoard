using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ToolBoard.Data;
using ToolBoard.Data.Entities;

namespace ToolBoard.Components.Modals
{
    public partial class ModalJobForm
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
        public Job Job { get; set; } = new ();

        [Parameter]
        public EventCallback<Job> JobChanged { get; set; }

        [Parameter]
        public string BoardId { get; set; } = "";

        [Parameter]
        public EventCallback<string> BoardIdChanged { get; set; }

        private async Task Close()
        {
            IsVisible = false;
            await IsVisibleChanged.InvokeAsync(IsVisible);
        }

        private async Task AddJobAsync()
        {
            if (Job.Id.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                using (var context = await DbContextFactory.CreateDbContextAsync())
                {
                    var tempBoard = await context.Boards.FirstOrDefaultAsync(b => b.Id == new Guid(BoardId ?? "00000000-0000-0000-0000-000000000000"));

                    if (tempBoard == null) return;

                    Job.BoardId = tempBoard.Id;
                    Job.Board = tempBoard;

                    await context.Jobs.AddAsync(Job);

                    await context.SaveChangesAsync();
                }
            }
            else
            {
                using (var context = await DbContextFactory.CreateDbContextAsync())
                {
                    context.Jobs.Update(Job);

                    await context.SaveChangesAsync();
                }
            }

            await Close();

            await OnUpdateJobList.InvokeAsync();
        }
    }
}
