using TaskManager.Repositories.Entities;

namespace TaskManager.Repositories.Interface
{
    public interface ITaskRepository
    {
        List<TaskRecord> GetAllTasks();
        List<TaskRecord> GetByProjectId(int projectId);
        TaskRecord GetById(int taskId);
    }
}
