using TaskManager.Repositories.Interface;
using TaskManager.Services.DTO;
using TaskManager.Services.Interface;

namespace TaskManager.Services.Services
{
    public sealed class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public List<TaskListDto> GetTasksForProject(int projectId)
        {
            return _taskRepository.GetByProjectId(projectId)
                .Select(task => new TaskListDto
                {
                    Id = task.Id,
                    ProjectId = task.ProjectId,
                    Name = task.Title,
                    Priority = task.Priority,
                    IsFinished = task.IsFinished,
                    IsOverdue = !task.IsFinished && DateTime.Today > task.DueDate.Date
                })
                .ToList();
        }

        public TaskDetailsDto GetTaskDetails(int taskId)
        {
            var task = _taskRepository.GetById(taskId);

            return new TaskDetailsDto
            {
                Id = task.Id,
                ProjectId = task.ProjectId,
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority,
                DueDate = task.DueDate,
                IsFinished = task.IsFinished,
                IsOverdue = !task.IsFinished && DateTime.Today > task.DueDate.Date
            };
        }
    }
}