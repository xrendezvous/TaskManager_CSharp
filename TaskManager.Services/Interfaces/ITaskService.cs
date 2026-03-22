using TaskManager.Services.DTO;
namespace TaskManager.Services.Interfaces
{
    /// <summary>
    /// defines methods for preparing task data for the UI layer
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// gets task data for displaying in the project details list
        /// </summary>
        /// <param name="projectId"/>
        /// <returns>list of task DTO objects for the list view</returns>
        List<TaskListDto> GetTasksForProject(int projectId);
        /// <summary>
        /// gets detailed info about a task
        /// </summary>
        /// <param name="taskId"/>
        /// <returns>DTO object containing detailed task info</returns>
        TaskDetailsDto GetTaskDetails(int taskId);
    }
}
