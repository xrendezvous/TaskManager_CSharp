using TaskManager.Repositories.Entities;

namespace TaskManager.Repositories.Interfaces
{
    /// <summary>
    /// defines methods for working with task data in the repository layer
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// gets all tasks
        /// </summary>
        /// <returns>collection of all task records</returns>
        IEnumerable<TaskRecord> GetAllTasks();

        /// <summary>
        /// gets tasks that belong to the specified project
        /// </summary>
        /// <param name="projectId"/>
        /// <returns>collection of task records for the specified project</returns>
        IEnumerable<TaskRecord> GetByProjectId(int projectId);

        /// <summary>
        /// gets a task by its id
        /// </summary>
        /// <param name="taskId"/>
        /// <returns>the matching task record</returns>
        /// <exception cref="KeyNotFoundException">
        /// thrown when the task with the specified id is not found
        /// </exception>
        TaskRecord GetById(int taskId);
    }
}
