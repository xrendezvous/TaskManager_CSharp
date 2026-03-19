using TaskManager.Repositories.Entities;

namespace TaskManager.Repositories.Storage
{
    public interface IStorageContext
    {
        IEnumerable<ProjectRecord> GetProjects();
        ProjectRecord? GetProject(int projectId);

        IEnumerable<TaskRecord> GetTasks();
        IEnumerable<TaskRecord> GetTasksByProject(int projectId);
        TaskRecord? GetTask(int taskId);
    }
}