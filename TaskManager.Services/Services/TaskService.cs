using TaskManager.Repositories.Interfaces;
using TaskManager.Services.DTO;
using TaskManager.Services.Interfaces;

namespace TaskManager.Services.Services
{
    /// <summary>
    /// Provides task-related business logic and prepares DTO models for the UI layer.
    /// </summary>
    public sealed class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService"/> class.
        /// </summary>
        /// <param name="taskRepository">The repository used to access task data.</param>
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        /// <summary>
        /// Gets task data for displaying in the project details list.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>A list of task DTO objects for the list view.</returns>
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

        /// <summary>
        /// Gets detailed information about a task.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns>A DTO object containing detailed task information.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the task with the specified identifier is not found.
        /// </exception>
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