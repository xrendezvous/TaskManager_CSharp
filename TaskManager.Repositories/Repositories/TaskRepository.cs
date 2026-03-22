using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Interfaces;
using TaskManager.Repositories.Storage;

namespace TaskManager.Repositories.Repositories
{
    /// <summary>
    /// provides access to task data stored in the storage context
    /// </summary>
    public sealed class TaskRepository : ITaskRepository
    {
        private readonly IStorageContext _storageContext;

        /// <summary>
        /// initializes a new instance of the <see cref="TaskRepository"/> class
        /// </summary>
        /// <param name="storageContext">the storage context used to access task data</param>
        public TaskRepository(IStorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        public IEnumerable<TaskRecord> GetAllTasks()
        {
            return _storageContext.GetTasks().ToList();
        }

        public IEnumerable<TaskRecord> GetByProjectId(int projectId)
        {
            return _storageContext.GetTasksByProject(projectId)
                .Where(t => t.ProjectId == projectId)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .ToList();
        }

        public TaskRecord GetById(int taskId)
        {
            var task = _storageContext.GetTask(taskId);

            if (task is null)
                throw new KeyNotFoundException($"Task with ID {taskId} was not found.");

            return task;
        }
    }
}