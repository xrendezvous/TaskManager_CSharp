using Microsoft.Extensions.Logging;
using TaskManager.Services;
using TaskManager.UI;

namespace TaskManager.AppUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>();

        builder.Services.AddSingleton<RepositoryService>();
        /*
        builder.Services.AddTransient<ProjectsPage>();
        builder.Services.AddTransient<ProjectDetailsPage>();
        builder.Services.AddTransient<TaskDetailsPage>();
        */

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}