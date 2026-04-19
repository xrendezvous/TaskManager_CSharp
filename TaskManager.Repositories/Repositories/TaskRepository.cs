using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Enums;
using TaskManager.Repositories.Interfaces;
using TaskManager.Repositories.Storage;

namespace TaskManager.Repositories.Repositories
{
    /// <summary>
    /// provides access to task data through the StorageContext
    /// </summary>
    public sealed class TaskRepository : ITaskRepository
    {
        private readonly IStorageContext _storageContext;
        public TaskRepository(IStorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        public async Task<IReadOnlyList<TaskRecord>> GetAllTasksAsync()
        {
            var tasks = await _storageContext.GetTasksAsync();

            return tasks
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .ThenBy(t => t.Id)
                .ToList();
        }

        public async Task<IReadOnlyList<TaskRecord>> GetByProjectIdAsync(int projectId)
        {
            var tasks = await _storageContext.GetTasksByProjectAsync(projectId);

            return tasks
                .Where(t => t.ProjectId == projectId)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .ThenBy(t => t.Id)
                .ToList();
        }

        public async Task<TaskRecord> GetByIdAsync(int taskId)
        {
            var task = await _storageContext.GetTaskAsync(taskId);

            if (task is null)
                throw new KeyNotFoundException($"Task with ID {taskId} was not found.");

            return task;
        }

        public Task<TaskRecord> AddAsync(
            int projectId,
            string name,
            string description,
            Priority priority,
            DateTime dueDate,
            bool isFinished)
        {
            return _storageContext.AddTaskAsync(
                projectId,
                name,
                description,
                priority,
                dueDate,
                isFinished);
        }

        public Task UpdateAsync(TaskRecord task)
        {
            return _storageContext.UpdateTaskAsync(task);
        }

        public Task DeleteAsync(int taskId)
        {
            return _storageContext.DeleteTaskAsync(taskId);
        }
    }
}