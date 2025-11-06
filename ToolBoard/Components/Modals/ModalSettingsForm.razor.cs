using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ToolBoard.Data;
using ToolBoard.Data.Entities;
using ToolBoard.Models;

namespace ToolBoard.Components.Modals
{
    public partial class ModalSettingsForm
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        public IDbContextFactory<AppDbContext> DbContextFactory { get; set; } = default!;

        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public EventCallback<bool> IsVisibleChanged { get; set; }


        [Parameter]
        public string BoardId { get; set; } = "";

        [Parameter]
        public EventCallback<string> BoardIdChanged { get; set; }

        [Parameter]
        public EventCallback OnUpdateJobList { get; set; }

        string messageTitle = "";
        string message = "";
        bool isVisibleMessage;

        private async Task Close()
        {
            IsVisible = false;
            await IsVisibleChanged.InvokeAsync(IsVisible);
        }

        private async Task LoadFile(InputFileChangeEventArgs e)
        {
            try
            {
                using var stream = e.File.OpenReadStream();
                using var reader = new StreamReader(stream);
                var jsonData = await reader.ReadToEndAsync();

                // Если нужно десериализовать в объект:
                var deserializedJobs = JsonSerializer.Deserialize<List<JobJson>>(jsonData);

                if (deserializedJobs != null)
                {
                    using (var context = await DbContextFactory.CreateDbContextAsync())
                    {
                        Guid id = new Guid(BoardId ?? "00000000-0000-0000-0000-000000000000");

                        var tempBoard = await context.Boards.FirstOrDefaultAsync(b => b.Id == id);

                        if (tempBoard == null) return;

                        context.Jobs.RemoveRange(context.Jobs.Where(j => j.BoardId == id));

                        await context.SaveChangesAsync();


                        foreach (var job in deserializedJobs)
                        {
                            await context.Jobs.AddAsync(new Job { Name = $"{job.Field} {job!.Pad}", Type = job.Type ?? "", FieldTeam = job.FieldTeam ?? "", Phone = job.Phone ?? "", Board = tempBoard });

                        }
                        await context.SaveChangesAsync();

                        await OnUpdateJobList.InvokeAsync();
                    }
                }

                messageTitle = "Успех";
                message = "Данные загружены!";
                isVisibleMessage = true;
            }
            catch (Exception ex)
            {
                messageTitle = "Ошибка!";
                isVisibleMessage = true;
                message = ex.Message;
            }
        }

        private async Task SaveToJson()
        {
            try
            {
                using (var context = await DbContextFactory.CreateDbContextAsync())
                {
                    var jobs = await context.Jobs
                        .Where(j=>j.BoardId==new Guid(BoardId))
                        .Include(j => j.ToolStickers)
                        .Include(j => j.SurfaceStickers).ToListAsync();

                    var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };

                    var json = JsonSerializer.Serialize(jobs, options);

                    var fileName = $"Backup_{DateTime.Now:yyyyMMdd}.json";

                    await JSRuntime.InvokeVoidAsync(
                        "saveAsFile",
                        fileName,
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(json)));
                }
            }
            catch (Exception ex)
            {

                messageTitle = "Ошибка!";
                isVisibleMessage = true;
                message = ex.Message;
            }
        }

        private async Task LoadFromJson(InputFileChangeEventArgs e)
        {
            try
            {
                using var stream = e.File.OpenReadStream();
                using var reader = new StreamReader(stream);
                var jsonData = await reader.ReadToEndAsync();

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };


                var deserializedJobs = JsonSerializer.Deserialize<List<Job>>(jsonData, options);

                if (deserializedJobs != null)
                {
                    using (var context = await DbContextFactory.CreateDbContextAsync())
                    {
                        Guid id = new Guid(BoardId ?? "00000000-0000-0000-0000-000000000000");

                        var tempBoard = await context.Boards.FirstOrDefaultAsync(b => b.Id == id);

                        if (tempBoard == null) return;

                        foreach (var job in deserializedJobs)
                        {
                            job.Board = tempBoard;
                        }


                        context.Jobs.RemoveRange(context.Jobs.Where(j => j.BoardId == id));

                        await context.SaveChangesAsync();

                        await context.Jobs.AddRangeAsync(deserializedJobs);

                        await context.SaveChangesAsync();

                        await OnUpdateJobList.InvokeAsync();
                    }
                }

                messageTitle = "Успех";
                message = "Данные загружены!";
                isVisibleMessage = true;
            }
            catch (Exception ex)
            {
                messageTitle = "Ошибка!";
                isVisibleMessage = true;
                message = ex.Message;
            }
        }
    }
}
