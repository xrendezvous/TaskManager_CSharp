using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Enums;

namespace TaskManager.Repositories.Interfaces
{
    /// <summary>
    /// defines methods for working with task data in the TaskRepository
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// gets all tasks from the repository
        /// </summary>
        /// <returns>read-only collection of task records</returns>
        Task<IReadOnlyList<TaskRecord>> GetAllTasksAsync();

        /// <summary>
        /// gets tasks that belong to the specified project
        /// </summary>
        /// <param name="projectId">owner project identifier</param>
        /// <returns>read-only collection of task records for the project</returns>
        Task<IReadOnlyList<TaskRecord>> GetByProjectIdAsync(int projectId);

        /// <summary>
        /// gets a task by its id
        /// </summary>
        /// <param name="taskId">task id</param>
        /// <returns>matching task record</returns>
        Task<TaskRecord> GetByIdAsync(int taskId);

        /// <summary>
        /// creates a new task in the repository
        /// </summary>
        /// <param name="projectId">owner project id</param>
        /// <param name="name">task name</param>
        /// <param name="description">task description</param>
        /// <param name="priority">task priority</param>
        /// <param name="dueDate">task due date</param>
        /// <param name="isFinished">task completion flag</param>
        /// <returns>created task record</returns>
        Task<TaskRecord> AddAsync(
            int projectId,
            string name,
            string description,
            Priority priority,
            DateTime dueDate,
            bool isFinished);

        /// <summary>
        /// updates an existing task in the repository
        /// </summary>
        /// <param name="task">task record with updated vals</param>
        Task UpdateAsync(TaskRecord task);

        /// <summary>
        /// deletes a task by its id
        /// </summary>
        /// <param name="taskId">task id</param>
        Task DeleteAsync(int taskId);
    }
}