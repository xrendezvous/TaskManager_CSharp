using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Enums;

namespace TaskManager.Repositories.Storage
{
    public interface IStorageContext
    {
        Task<IReadOnlyList<ProjectRecord>> GetProjectsAsync();
        Task<ProjectRecord?> GetProjectAsync(int projectId);

        Task<ProjectRecord> AddProjectAsync(string name, string description, TypeOfProject type);
        Task UpdateProjectAsync(ProjectRecord project);
        Task DeleteProjectAsync(int projectId);

        Task<IReadOnlyList<TaskRecord>> GetTasksAsync();
        Task<IReadOnlyList<TaskRecord>> GetTasksByProjectAsync(int projectId);
        Task<TaskRecord?> GetTaskAsync(int taskId);

        Task<TaskRecord> AddTaskAsync(
            int projectId,
            string name,
            string description,
            Priority priority,
            DateTime dueDate,
            bool isFinished);

        Task UpdateTaskAsync(TaskRecord task);
        Task DeleteTaskAsync(int taskId);
    }
}