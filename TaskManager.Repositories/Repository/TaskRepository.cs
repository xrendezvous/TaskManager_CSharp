using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Interface;
using TaskManager.Repositories.Storage;

namespace TaskManager.Repositories.Repositories
{
    public sealed class TaskRepository : ITaskRepository
    {
        private readonly IStorageContext _storageContext;

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