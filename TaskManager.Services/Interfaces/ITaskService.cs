using TaskManager.Services.DTO;

namespace TaskManager.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IReadOnlyList<TaskListDto>> GetTasksForProjectAsync(int projectId, FilterTaskDto filter);
        Task<TaskDetailsDto> GetTaskDetailsAsync(int taskId);
        Task<TaskDetailsDto> CreateTaskAsync(CreateTaskDto dto);
        Task UpdateTaskAsync(int taskId, UpdateTaskDto dto);
        Task DeleteTaskAsync(int taskId);
    }
}