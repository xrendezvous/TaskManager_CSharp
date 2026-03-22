namespace TaskManager.AppUI.Services;

public interface INavigateService
{
    /// <summary>
    /// navigates to the project details page
    /// </summary>
    /// <param name="projectId"/>
    Task GoToProjectDetailsAsync(int projectId);
    /// <summary>
    /// navigates to the task details page
    /// </summary>
    /// <param name="taskId"/>
    Task GoToTaskDetailsAsync(int taskId);
    /// <summary>
    /// navigates back to the previous page
    /// </summary>
    Task GoBackAsync();
}