using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Enums;

namespace TaskManager.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<IReadOnlyList<TaskRecord>> GetAllTasksAsync();
        Task<IReadOnlyList<TaskRecord>> GetByProjectIdAsync(int projectId);
        Task<TaskRecord> GetByIdAsync(int taskId);
        Task<TaskRecord> AddAsync(
            int projectId,
            string name,
            string description,
            Priority priority,
            DateTime dueDate,
            bool isFinished);
        Task UpdateAsync(TaskRecord task);
        Task DeleteAsync(int taskId);
    }
}