namespace TaskManager.AppUI.Services;

public sealed class NavigateService : INavigateService
{
    public Task GoToProjectDetailsAsync(int projectId)
    {
        return Shell.Current.GoToAsync($"{nameof(ProjectDetailsPage)}?projectId={projectId}");
    }

    public Task GoToTaskDetailsAsync(int taskId)
    {
        return Shell.Current.GoToAsync($"{nameof(TaskDetailsPage)}?taskId={taskId}");
    }

    public Task GoBackAsync()
    {
        return Shell.Current.GoToAsync("..");
    }
}