using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Enums;
using TaskManager.Repositories.Interfaces;
using TaskManager.Services.DTO;
using TaskManager.Services.Interfaces;

namespace TaskManager.Services.Services
{
    /// <summary>
    /// project-related business logic and transforms 
    /// repository models into DTOs for the UI layer
    /// </summary>
    public sealed class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;

        /// <summary>
        /// initializes a new instance of the <see cref="ProjectService"/> class
        /// </summary>
        /// <param name="projectRepository">repository used to access project data</param>
        /// <param name="taskRepository">repository used to access task data</param>
        public ProjectService(IProjectRepository projectRepository, ITaskRepository taskRepository)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
        }

        public async Task<IReadOnlyList<ProjectListDto>> GetProjectsForListAsync(FilterProjectDto filter)
        {
            var projects = await _projectRepository.GetAllProjectsAsync();
            var tasks = await _taskRepository.GetAllTasksAsync();

            var result = projects.Select(project =>
            {
                var projectTasks = tasks.Where(t => t.ProjectId == project.Id).ToList();
                var finishedTasks = projectTasks.Count(t => t.IsFinished);
                var progress = projectTasks.Count == 0
                    ? 0
                    : (int)Math.Round(finishedTasks * 100.0 / projectTasks.Count);

                return new ProjectListDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    Type = project.Type,
                    Progress = progress
                };
            });

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var search = filter.SearchText.Trim().ToLowerInvariant();
                result = result.Where(p =>
                    p.Name.ToLowerInvariant().Contains(search) ||
                    p.Description.ToLowerInvariant().Contains(search));
            }

            if (filter.Type.HasValue)
                result = result.Where(p => p.Type == filter.Type.Value);

            result = filter.SortBy switch
            {
                ProjectSortOption.ByName => result.OrderBy(p => p.Name),
                ProjectSortOption.ByProgressAsc => result.OrderBy(p => p.Progress),
                ProjectSortOption.ByProgressDesc => result.OrderByDescending(p => p.Progress),
                _ => result.OrderBy(p => p.Id)
            };

            return result.ToList();
        }

        public async Task<ProjectDetailsDto> GetProjectDetailsAsync(int projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            var tasks = await _taskRepository.GetByProjectIdAsync(projectId);

            var finishedTasks = tasks.Count(t => t.IsFinished);
            var progress = tasks.Count == 0
                ? 0
                : (int)Math.Round(finishedTasks * 100.0 / tasks.Count);

            return new ProjectDetailsDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Type = project.Type,
                TasksAmount = tasks.Count,
                FinishedTasks = finishedTasks,
                Progress = progress
            };
        }

        public async Task<ProjectDetailsDto> CreateProjectAsync(CreateProjectDto dto)
        {
            var created = await _projectRepository.AddAsync(dto.Name, dto.Description, dto.Type);
            return await GetProjectDetailsAsync(created.Id);
        }

        public async Task UpdateProjectAsync(int projectId, UpdateProjectDto dto)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            project.UpdateRecord(dto.Name, dto.Description, dto.Type);
            await _projectRepository.UpdateAsync(project);
        }

        public Task DeleteProjectAsync(int projectId)
        {
            return _projectRepository.DeleteAsync(projectId);
        }
    }
}