using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;
using Plk.Blazor.DragDrop;
using ToolBoard.Data;
using ToolBoard.Data.Entities;

namespace ToolBoard.Components.Pages
{
    public partial class Main
    {
        [Inject]
        public IDbContextFactory<AppDbContext> DbContextFactory { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        bool showJobModal;

        bool showToolStickerModal;

        bool showSurfaceStickerModal;

        bool showSettingsModal;

        bool showSideBar;

        string searchText="";

        Job newJob = new();

        ToolSticker newToolSticker = new();

        SurfaceSticker newSurfaceSticker = new();

        List<Job> jobList = new();

        List<Job> filteredJobs = new();

        [Parameter]
        public string? BoardId { get; set; }
        protected override async Task OnInitializedAsync()
        {

            NavigationManager.LocationChanged += HandleLocationChanged;
            await UpdateJobListAsync();
            await base.OnInitializedAsync();

        }

        private async void HandleLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            showSideBar = false;
            await UpdateJobListAsync();
            
        }

        protected override void OnParametersSet()
        {
            FilterItems();
        }

        private void FilterItems()
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                filteredJobs=jobList;
                return;
            }

            filteredJobs = jobList
                .Where(job => job.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private async Task UpdateJobListAsync()
        {
            using (var context = await DbContextFactory.CreateDbContextAsync())
            {
                var fff = Guid.Empty.ToString();


                jobList = await context.Jobs
                    .Where(j => j.BoardId == new Guid(BoardId ?? Guid.Empty.ToString()))
                    .Include(j => j.ToolStickers.OrderBy(s => s.Name))
                    .Include(j => j.SurfaceStickers.OrderBy(s => s.Name))
                    .Include(j => j.Board) // Bring data to client first
                    .OrderBy(s => s.Name)
                    .ToListAsync();

                //jobList = (await context.Jobs
                //    .Where(j => j.BoardId == new Guid(BoardId ?? Guid.Empty.ToString()))
                //    .Include(j => j.ToolStickers.OrderBy(s => s.Name))
                //    .Include(j => j.SurfaceStickers.OrderBy(s => s.Name))
                //    .Include(j => j.Board)
                //    .ToListAsync()) // Bring data to client first
                //    .OrderBy(s => int.TryParse(s.FieldTeam, out var num) ? num : int.MaxValue)
                //    .ToList();

                FilterItems();
            }  

            newToolSticker = new();
            newSurfaceSticker = new();

            newJob = new();

            StateHasChanged();
        }


        private async Task RemoveJobAsync(Guid jobId)
        {
            using (var context = await DbContextFactory.CreateDbContextAsync())
            {
                var tempjob = await context.Jobs.FirstOrDefaultAsync(j => j.Id == jobId);
                if (tempjob == null) return;
                context.Jobs.Remove(tempjob);
                await context.SaveChangesAsync();
                await UpdateJobListAsync();
            }
        }

        private async Task RemoveToolStickerAsync(Job job, ToolSticker sticker)
        {
            using (var context = await DbContextFactory.CreateDbContextAsync())
            {
                var dbJob = await context.Jobs
                    .Include(j => j.ToolStickers)
                    .FirstOrDefaultAsync(j => j.Id == job.Id);

                if (dbJob != null)
                {
                    var stickerToRemove = dbJob.ToolStickers.FirstOrDefault(s => s.Id == sticker.Id);

                    if (stickerToRemove != null)
                    {
                        dbJob.ToolStickers.Remove(stickerToRemove);

                        await context.SaveChangesAsync();
                    }
                }
            }


            await UpdateJobListAsync();
        }

        private async Task ReplacedToolStickerDrop(ToolSticker sticker)
        {
            using (var context = await DbContextFactory.CreateDbContextAsync())
            {
                var tempJob = jobList.FirstOrDefault(p => p.ToolStickers.Contains(sticker));

                if (tempJob == null) return;

                sticker.Job = tempJob;
                sticker.JobId = tempJob.Id;

                context.ToolStickers.Update(sticker);

                await context.SaveChangesAsync();
            }
            await UpdateJobListAsync(); 
        }

        private async Task RemoveSurfaceStickerAsync(Job job, SurfaceSticker sticker)
        {
            using (var context = await DbContextFactory.CreateDbContextAsync())
            {
                var dbJob = await context.Jobs
                    .Include(j => j.SurfaceStickers)
                    .FirstOrDefaultAsync(j => j.Id == job.Id);

                if (dbJob != null)
                {
                    var stickerToRemove = dbJob.SurfaceStickers.FirstOrDefault(s => s.Id == sticker.Id);

                    if (stickerToRemove != null)
                    {
                        dbJob.SurfaceStickers.Remove(stickerToRemove);

                        await context.SaveChangesAsync();
                    }
                }
            }


            await UpdateJobListAsync();
        }

        private async Task ReplacedSurfaceStickerDrop(SurfaceSticker sticker)
        {
            using (var context = await DbContextFactory.CreateDbContextAsync())
            {
                var tempJob = jobList.FirstOrDefault(p => p.SurfaceStickers.Contains(sticker));

                if (tempJob == null) return;

                sticker.Job = tempJob;
                sticker.JobId = tempJob.Id;

                context.SurfaceStickers.Update(sticker);

                await context.SaveChangesAsync();
            }
            await UpdateJobListAsync();
        }
    }
}
