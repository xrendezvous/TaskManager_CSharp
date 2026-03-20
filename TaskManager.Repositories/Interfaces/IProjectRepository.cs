using TaskManager.Repositories.Entities;

namespace TaskManager.Repositories.Interfaces
{
    /// <summary>
    /// Defines methods for working with project data in the repository layer.
    /// </summary>
    public interface IProjectRepository
    {
        /// <summary>
        /// Gets all projects.
        /// </summary>
        /// <returns>A collection of all project records.</returns>
        IEnumerable<ProjectRecord> GetAllProjects();
        /// <summary>
        /// Gets a project by its identifier.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>The matching project record.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the project with the specified identifier is not found.
        /// </exception>
        ProjectRecord GetById(int projectId);
    }
}
