using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Plk.Blazor.DragDrop;
using ToolBoard.Components;
using ToolBoard.Data;

namespace ToolBoard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;
            var configurations = builder.Configuration;

            // Add services to the container.
            services.AddRazorComponents()
                .AddInteractiveServerComponents();

            services.AddDbContextFactory<AppDbContext>((opt) => opt.UseSqlite(configurations.GetConnectionString("SqlLite")));

            services.AddBlazorDragDrop();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
