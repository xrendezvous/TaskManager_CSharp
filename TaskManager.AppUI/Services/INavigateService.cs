namespace TaskManager.AppUI.Services;

/// <summary>
/// Defines navigation methods used by the UI view models.
/// </summary>
public interface INavigateService
{
    /// <summary>
    /// Navigates to the project details page.
    /// </summary>
    /// <param name="projectId">The project identifier.</param>
    /// <returns>A task that represents the asynchronous navigation operation.</returns>
    Task GoToProjectDetailsAsync(int projectId);
    /// <summary>
    /// Navigates to the task details page.
    /// </summary>
    /// <param name="taskId">The task identifier.</param>
    /// <returns>A task that represents the asynchronous navigation operation.</returns>
    Task GoToTaskDetailsAsync(int taskId);
    /// <summary>
    /// Navigates back to the previous page.
    /// </summary>
    /// <returns>A task that represents the asynchronous navigation operation.</returns>
    Task GoBackAsync();
}