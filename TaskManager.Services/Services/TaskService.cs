using TaskManager.Repositories.Interfaces;
using TaskManager.Services.DTO;
using TaskManager.Services.Interfaces;

namespace TaskManager.Services.Services
{
    /// <summary>
    /// provides task-related business logic and prepares DTO models for the UI layer
    /// </summary>
    public sealed class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        /// <summary>
        /// initializes a new instance of the <see cref="TaskService"/> class
        /// </summary>
        /// <param name="taskRepository">repository used to access task data</param>
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