using TaskManager.Services.DTO;

namespace TaskManager.Services.Interfaces
{
    /// <summary>
    /// defines business operations related to tasks
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// gets task list items for a project using the 
        /// provided filter options
        /// </summary>
        /// <param name="projectId">owner project id</param>
        /// <param name="filter">task search, filter and sort options</param>
        /// <returns>read-only collection of task list DTO objects</returns>
        Task<IReadOnlyList<TaskListDto>> GetTasksForProjectAsync(int projectId, FilterTaskDto filter);

        /// <summary>
        /// gets detailed info about a specific task
        /// </summary>
        /// <param name="taskId">task id</param>
        /// <returns>task details DTO</returns>
        Task<TaskDetailsDto> GetTaskDetailsAsync(int taskId);

        /// <summary>
        /// creates a new task
        /// </summary>
        /// <param name="dto">task creation data</param>
        /// <returns>details of the created task</returns>
        Task<TaskDetailsDto> CreateTaskAsync(CreateTaskDto dto);

        /// <summary>
        /// updates an existing task
        /// </summary>
        /// <param name="taskId">task id</param>
        /// <param name="dto">updated task data</param>
        Task UpdateTaskAsync(int taskId, UpdateTaskDto dto);

        /// <summary>
        /// deletes a task
        /// </summary>
        /// <param name="taskId">task id</param>
        Task DeleteTaskAsync(int taskId);
    }
}