using Microsoft.Extensions.Logging;
using TaskManager.AppUI.Services;
using TaskManager.AppUI.ViewModels;
using TaskManager.Repositories.Interfaces;
using TaskManager.Repositories.Repositories;
using TaskManager.Repositories.Storage;
using TaskManager.Services.Interfaces;
using TaskManager.Services.Services;

namespace TaskManager.AppUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>();

        builder.Services.AddSingleton<IStorageContext, JsonStorageContext>();
        builder.Services.AddSingleton<IProjectRepository, ProjectRepository>();
        builder.Services.AddSingleton<ITaskRepository, TaskRepository>();
        builder.Services.AddSingleton<IProjectService, ProjectService>();
        builder.Services.AddSingleton<ITaskService, TaskService>();
        builder.Services.AddSingleton<INavigateService, NavigateService>();

        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddTransient<ProjectsViewModel>();
        builder.Services.AddTransient<ProjectDetailsViewModel>();
        builder.Services.AddTransient<TaskDetailsViewModel>();
        builder.Services.AddTransient<ProjectsPage>();
        builder.Services.AddTransient<ProjectDetailsPage>();
        builder.Services.AddTransient<TaskDetailsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}