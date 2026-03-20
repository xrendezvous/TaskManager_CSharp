using TaskManager.Repositories.Interfaces;
using TaskManager.Services.DTO;
using TaskManager.Services.Interfaces;

namespace TaskManager.Services.Services
{
    /// <summary>
    /// Provides project-related business logic and prepares DTO models for the UI layer.
    /// </summary>
    public sealed class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectService"/> class.
        /// </summary>
        /// <param name="projectRepository">The repository used to access project data.</param>
        /// <param name="taskRepository">The repository used to access task data.</param>
        public ProjectService(
            IProjectRepository projectRepository,
            ITaskRepository taskRepository)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
        }

        /// <summary>
        /// Gets project data for displaying in the projects list.
        /// </summary>
        /// <returns>A list of project DTO objects for the list view.</returns>
        public List<ProjectListDto> GetProjectsForList()
        {
            var projects = _projectRepository.GetAllProjects();
            var tasks = _taskRepository.GetAllTasks();

            return projects
                .Select(project =>
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
                })
                .OrderBy(p => p.Id)
                .ToList();
        }

        /// <summary>
        /// Gets detailed information about a project.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>A DTO object containing detailed project information.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the project with the specified identifier is not found.
        /// </exception>
        public ProjectDetailsDto GetProjectDetails(int projectId)
        {
            var project = _projectRepository.GetById(projectId);
            var tasks = _taskRepository.GetByProjectId(projectId).ToList();

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
    }
}