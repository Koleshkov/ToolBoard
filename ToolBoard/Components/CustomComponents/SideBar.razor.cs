using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using ToolBoard.Data;
using ToolBoard.Data.Entities;

namespace ToolBoard.Components.CustomComponents
{
    public partial class SideBar
    {

        [Inject]
        private IDbContextFactory<AppDbContext> DbContextFactory { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public EventCallback<bool> IsVisibleChanged { get; set; }

        [Parameter]
        public string SelectedBoardId { get; set; } = "";

        [Parameter]
        public EventCallback<string> SelectedBoardIdChanged { get; set; }

        [Parameter]
        public EventCallback OnUpdateJobList { get; set; }

        bool IsVisibleBoardForm;

        List<Board> boardList = new();

        Board newBoard = new();

        private async Task Close()
        {
            IsVisible = false;
            await IsVisibleChanged.InvokeAsync(IsVisible);
        }

        protected async override Task OnInitializedAsync()
        {
            await UpdateBoardList();
            await base.OnInitializedAsync();
        }

        private async Task UpdateBoardList()
        {
            using (var context = await DbContextFactory.CreateDbContextAsync())
            {
                boardList = await context.Boards.ToListAsync();
            }
        }

        private async Task RemoveBoard(Guid id) 
        {
            using (var context = await DbContextFactory.CreateDbContextAsync())
            {
                var tempBoard = await context.Boards.FirstOrDefaultAsync(b => b.Id == id);

                if (tempBoard == null) return;

                context.Boards.Remove(tempBoard);

                await context.SaveChangesAsync();

            }
            await UpdateBoardList();
            await OnUpdateJobList.InvokeAsync();
        }

    }
}
