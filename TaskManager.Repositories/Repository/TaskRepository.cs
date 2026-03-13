using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Interface;
using TaskManager.Repositories.Storage;

namespace TaskManager.Repositories.Repositories
{
    public sealed class TaskRepository : ITaskRepository
    {
        public List<TaskRecord> GetAllTasks()
        {
            return DataStorage.Tasks.ToList();
        }

        public List<TaskRecord> GetByProjectId(int projectId)
        {
            return DataStorage.Tasks
                .Where(t => t.ProjectId == projectId)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .ToList();
        }

        public TaskRecord GetById(int taskId)
        {
            var task = DataStorage.Tasks.FirstOrDefault(t => t.Id == taskId);

            if (task is null)
                throw new KeyNotFoundException($"Task with ID {taskId} was not found.");

            return task;
        }
    }
}