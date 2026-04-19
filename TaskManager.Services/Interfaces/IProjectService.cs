using TaskManager.Services.DTO;

namespace TaskManager.Services.Interfaces
{
    /// <summary>
    /// defines business operations related to projects
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        /// gets project list items using the 
        /// provided filter options
        /// </summary>
        /// <param name="filter">project search, filter and sort options</param>
        /// <returns>read-only collection of project list DTO objects</returns>
        Task<IReadOnlyList<ProjectListDto>> GetProjectsForListAsync(FilterProjectDto filter);

        /// <summary>
        /// gets detailed info about a specific project
        /// </summary>
        /// <param name="projectId">project id</param>
        /// <returns>project details DTO</returns>
        Task<ProjectDetailsDto> GetProjectDetailsAsync(int projectId);

        /// <summary>
        /// creates a new project
        /// </summary>
        /// <param name="dto">project creation data</param>
        /// <returns>details of the created project</returns>
        Task<ProjectDetailsDto> CreateProjectAsync(CreateProjectDto dto);

        /// <summary>
        /// updates an existing project
        /// </summary>
        /// <param name="projectId">project id</param>
        /// <param name="dto">updated project data</param>
        Task UpdateProjectAsync(int projectId, UpdateProjectDto dto);

        /// <summary>
        /// deletes a project
        /// </summary>
        /// <param name="projectId">project id</param>
        Task DeleteProjectAsync(int projectId);
    }
}