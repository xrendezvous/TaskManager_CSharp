using TaskManager.Services.DTO;

namespace TaskManager.Services.Interfaces
{
    /// <summary>
    /// Defines methods for preparing project data for the UI layer.
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        /// Gets project data for displaying in the projects list.
        /// </summary>
        /// <returns>A list of project DTO objects for the list view.</returns>
        List<ProjectListDto> GetProjectsForList();
        /// <summary>
        /// Gets detailed information about a project.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>A DTO object containing detailed project information.</returns>
        ProjectDetailsDto GetProjectDetails(int projectId);
    }
}
