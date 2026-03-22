using TaskManager.Services.DTO;

namespace TaskManager.Services.Interfaces
{
    /// <summary>
    /// defines methods for preparing project data for the UI layer
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        /// gets project data for displaying in the projects list
        /// </summary>
        /// <returns>list of project DTO objects for the list view</returns>
        List<ProjectListDto> GetProjectsForList();
        /// <summary>
        /// gets detailed info about a project
        /// </summary>
        /// <param name="projectId"/>
        /// <returns>DTO object containing detailed project info</returns>
        ProjectDetailsDto GetProjectDetails(int projectId);
    }
}
