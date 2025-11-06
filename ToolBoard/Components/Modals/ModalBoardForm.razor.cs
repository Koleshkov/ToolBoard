using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using ToolBoard.Data;
using ToolBoard.Data.Entities;

namespace ToolBoard.Components.Modals
{
    public partial class ModalBoardForm
    {
        [Inject]
        public IDbContextFactory<AppDbContext> DbContextFactory { get; set; } = default!;

        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public EventCallback<bool> IsVisibleChanged { get; set; }

        [Parameter]
        public Board Board { get; set; } = new();

        [Parameter]
        public EventCallback<Board> BoardChanged { get; set; }

        [Parameter]
        public EventCallback OnUpdateBoardList { get; set; }

        private async Task Close()
        {
            IsVisible = false;
            await IsVisibleChanged.InvokeAsync(IsVisible);
        }

        private async Task AddBoard()
        {
            if (Board.Id.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                using (var context = await DbContextFactory.CreateDbContextAsync())
                {
                    await context.Boards.AddAsync(Board);
                    await context.SaveChangesAsync();
                }
            }
            else
            {
                using (var context = await DbContextFactory.CreateDbContextAsync())
                {
                    context.Boards.Update(Board);
                    await context.SaveChangesAsync();
                }
            }

            await OnUpdateBoardList.InvokeAsync();
            await Close();
        }
    }
}
