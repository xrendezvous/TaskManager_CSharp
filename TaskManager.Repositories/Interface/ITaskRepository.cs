using TaskManager.Repositories.Entities;

namespace TaskManager.Repositories.Interface
{
    public interface ITaskRepository
    {
        IEnumerable<TaskRecord> GetAllTasks();
        IEnumerable<TaskRecord> GetByProjectId(int projectId);
        TaskRecord GetById(int taskId);
    }
}
