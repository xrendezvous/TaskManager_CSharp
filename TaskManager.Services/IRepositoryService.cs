using TaskManager.Views;

namespace TaskManager.Services
{
    public interface IRepositoryService
    {
        List<ProjectView> GetProjects();
        List<TaskView> GetTasksByProjects(int projectId);
        TaskView GetTask(int taskId);
    }
}