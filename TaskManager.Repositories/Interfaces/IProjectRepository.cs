using TaskManager.Repositories.Entities;

namespace TaskManager.Repositories.Interfaces
{
    /// <summary>
    /// defines methods for working with project data in the repository layer
    /// </summary>
    public interface IProjectRepository
    {
        /// <summary>
        /// gets all projects
        /// </summary>
        /// <returns>collection of all project records</returns>
        IEnumerable<ProjectRecord> GetAllProjects();
        /// <summary>
        /// gets a project by its id
        /// </summary>
        /// <param name="projectId"/>
        /// <returns>the matching project record</returns>
        /// <exception cref="KeyNotFoundException">
        /// thrown when the project with the specified id is not found
        /// </exception>
        ProjectRecord GetById(int projectId);
    }
}
