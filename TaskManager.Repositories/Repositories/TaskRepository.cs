using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Interfaces;
using TaskManager.Repositories.Storage;

namespace TaskManager.Repositories.Repositories
{
    /// <summary>
    /// Provides access to task data stored in the storage context.
    /// </summary>
    public sealed class TaskRepository : ITaskRepository
    {
        private readonly IStorageContext _storageContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRepository"/> class.
        /// </summary>
        /// <param name="storageContext">The storage context used to access task data.</param>
        public TaskRepository(IStorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        /// <summary>
        /// Gets all tasks.
        /// </summary>
        /// <returns>A collection of all task records.</returns>
        public IEnumerable<TaskRecord> GetAllTasks()
        {
            return _storageContext.GetTasks().ToList();
        }

        /// <summary>
        /// Gets tasks for the specified project ordered by priority and due date.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>A collection of task records for the specified project.</returns>
        public IEnumerable<TaskRecord> GetByProjectId(int projectId)
        {
            return _storageContext.GetTasksByProject(projectId)
                .Where(t => t.ProjectId == projectId)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .ToList();
        }

        /// <summary>
        /// Gets a task by its identifier.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns>The matching task record.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the task with the specified identifier is not found.
        /// </exception>
        public TaskRecord GetById(int taskId)
        {
            var task = _storageContext.GetTask(taskId);

            if (task is null)
                throw new KeyNotFoundException($"Task with ID {taskId} was not found.");

            return task;
        }
    }
}