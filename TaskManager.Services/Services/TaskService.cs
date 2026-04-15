using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Enums;
using TaskManager.Repositories.Interfaces;
using TaskManager.Services.DTO;
using TaskManager.Services.Interfaces;

namespace TaskManager.Services.Services
{
    public sealed class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IReadOnlyList<TaskListDto>> GetTasksForProjectAsync(int projectId, FilterTaskDto filter)
        {
            var tasks = await _taskRepository.GetByProjectIdAsync(projectId);

            var projectedTasks = tasks
                .Select(task => new TaskListDto
                {
                    Id = task.Id,
                    ProjectId = task.ProjectId,
                    Name = task.Name,
                    Priority = task.Priority,
                    IsFinished = task.IsFinished,
                    IsOverdue = !task.IsFinished && DateTime.Today > task.DueDate.Date
                });

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLowerInvariant();

                projectedTasks = projectedTasks.Where(task =>
                    task.Name.ToLowerInvariant().Contains(search));
            }

            if (filter.Priority.HasValue)
            {
                projectedTasks = projectedTasks.Where(task =>
                    task.Priority == filter.Priority.Value);
            }

            if (filter.IsFinished.HasValue)
            {
                projectedTasks = projectedTasks.Where(task =>
                    task.IsFinished == filter.IsFinished.Value);
            }

            if (filter.OnlyOverdue == true)
            {
                projectedTasks = projectedTasks.Where(task => task.IsOverdue);
            }

            projectedTasks = filter.SortBy switch
            {
                SortTask.ByName => projectedTasks
                    .OrderBy(task => task.Name)
                    .ThenBy(task => task.Id),

                SortTask.ByDueDate => tasks
                    .Select(task => new
                    {
                        Task = task,
                        Dto = new TaskListDto
                        {
                            Id = task.Id,
                            ProjectId = task.ProjectId,
                            Name = task.Name,
                            Priority = task.Priority,
                            IsFinished = task.IsFinished,
                            IsOverdue = !task.IsFinished && DateTime.Today > task.DueDate.Date
                        }
                    })
                    .Where(x =>
                        (string.IsNullOrWhiteSpace(filter.SearchText) ||
                         x.Dto.Name.ToLowerInvariant().Contains(filter.SearchText.Trim().ToLowerInvariant())) &&
                        (!filter.Priority.HasValue || x.Dto.Priority == filter.Priority.Value) &&
                        (!filter.IsFinished.HasValue || x.Dto.IsFinished == filter.IsFinished.Value) &&
                        (filter.OnlyOverdue != true || x.Dto.IsOverdue))
                    .OrderBy(x => x.Task.DueDate)
                    .ThenByDescending(x => x.Task.Priority)
                    .ThenBy(x => x.Task.Id)
                    .Select(x => x.Dto),

                _ => projectedTasks
                    .OrderByDescending(task => task.Priority)
                    .ThenBy(task => task.IsOverdue ? 0 : 1)
                    .ThenBy(task => task.Id)
            };

            return projectedTasks.ToList();
        }

        public async Task<TaskDetailsDto> GetTaskDetailsAsync(int taskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            return MapToDetailsDto(task);
        }

        public async Task<TaskDetailsDto> CreateTaskAsync(CreateTaskDto dto)
        {
            ValidateTask(dto.Name, dto.Description, dto.DueDate);

            var createdTask = await _taskRepository.AddAsync(
                dto.ProjectId,
                dto.Name.Trim(),
                dto.Description.Trim(),
                dto.Priority,
                dto.DueDate,
                dto.IsFinished);

            return MapToDetailsDto(createdTask);
        }

        public async Task UpdateTaskAsync(int taskId, UpdateTaskDto dto)
        {
            ValidateTask(dto.Name, dto.Description, dto.DueDate);

            var task = await _taskRepository.GetByIdAsync(taskId);

            task.UpdateRecord(
                dto.Name.Trim(),
                dto.Description.Trim(),
                dto.Priority,
                dto.DueDate,
                dto.IsFinished);

            await _taskRepository.UpdateAsync(task);
        }

        public Task DeleteTaskAsync(int taskId)
        {
            return _taskRepository.DeleteAsync(taskId);
        }

        private static TaskDetailsDto MapToDetailsDto(TaskRecord task)
        {
            return new TaskDetailsDto
            {
                Id = task.Id,
                ProjectId = task.ProjectId,
                Name = task.Name,
                Description = task.Description,
                Priority = task.Priority,
                DueDate = task.DueDate,
                IsFinished = task.IsFinished,
                IsOverdue = !task.IsFinished && DateTime.Today > task.DueDate.Date
            };
        }

        private static void ValidateTask(string name, string description, DateTime dueDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Task name cannot be empty.");

            if (name.Trim().Length > 100)
                throw new ArgumentException("Task name cannot be longer than 100 characters.");

            if (description.Trim().Length > 500)
                throw new ArgumentException("Task description cannot be longer than 500 characters.");

            if (dueDate.Year < 2000)
                throw new ArgumentException("Task due date is invalid.");
        }
    }
}