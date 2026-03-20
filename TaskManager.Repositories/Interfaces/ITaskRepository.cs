using TaskManager.Repositories.Entities;

namespace TaskManager.Repositories.Interfaces
{
    /// <summary>
    /// Defines methods for working with task data in the repository layer.
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// Gets all tasks.
        /// </summary>
        /// <returns>A collection of all task records.</returns>
        IEnumerable<TaskRecord> GetAllTasks();

        /// <summary>
        /// Gets tasks that belong to the specified project.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>A collection of task records for the specified project.</returns>
        IEnumerable<TaskRecord> GetByProjectId(int projectId);

        /// <summary>
        /// Gets a task by its identifier.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns>The matching task record.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the task with the specified identifier is not found.
        /// </exception>
        TaskRecord GetById(int taskId);
    }
}
