namespace TaskManager.AppUI.Services;

/// <summary>
/// Provides page navigation using the MAUI Shell navigation system.
/// </summary>
public sealed class NavigateService : INavigateService
{
    /// <summary>
    /// Navigates to the project details page.
    /// </summary>
    /// <param name="projectId">The project identifier.</param>
    /// <returns>A task that represents the asynchronous navigation operation.</returns>
    public Task GoToProjectDetailsAsync(int projectId)
    {
        return Shell.Current.GoToAsync($"{nameof(ProjectDetailsPage)}?projectId={projectId}");
    }

    /// <summary>
    /// Navigates to the task details page.
    /// </summary>
    /// <param name="taskId">The task identifier.</param>
    /// <returns>A task that represents the asynchronous navigation operation.</returns>
    public Task GoToTaskDetailsAsync(int taskId)
    {
        return Shell.Current.GoToAsync($"{nameof(TaskDetailsPage)}?taskId={taskId}");
    }

    /// <summary>
    /// Navigates back to the previous page.
    /// </summary>
    /// <returns>A task that represents the asynchronous navigation operation.</returns>
    public Task GoBackAsync()
    {
        return Shell.Current.GoToAsync("..");
    }
}