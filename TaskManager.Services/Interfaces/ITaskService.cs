using TaskManager.Services.DTO;
namespace TaskManager.Services.Interfaces
{
    /// <summary>
    /// Defines methods for preparing task data for the UI layer.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Gets task data for displaying in the project details list.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>A list of task DTO objects for the list view.</returns>
        List<TaskListDto> GetTasksForProject(int projectId);
        /// <summary>
        /// Gets detailed information about a task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns>A DTO object containing detailed task information.</returns>
        TaskDetailsDto GetTaskDetails(int taskId);
    }
}
