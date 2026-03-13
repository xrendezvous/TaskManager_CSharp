using TaskManager.Services.DTO;
namespace TaskManager.Services.Interface
{
    public interface ITaskService
    {
        List<TaskListDto> GetTasksForProject(int projectId);
        TaskDetailsDto GetTaskDetails(int taskId);
    }
}
