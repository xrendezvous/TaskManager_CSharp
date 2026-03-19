namespace TaskManager.AppUI.Services;

public interface INavigateService
{
    Task GoToProjectDetailsAsync(int projectId);
    Task GoToTaskDetailsAsync(int taskId);
    Task GoBackAsync();
}