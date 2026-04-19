using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Enums;

namespace TaskManager.Repositories.Interfaces
{
    /// <summary>
    /// defines methods for working with project data in the ProjectRepository
    /// </summary>
    public interface IProjectRepository
    {
        /// <summary>
        /// gets all projects from the repository
        /// </summary>
        /// <returns>read-only collection of project records</returns>
        Task<IReadOnlyList<ProjectRecord>> GetAllProjectsAsync();

        /// <summary>
        /// gets a project by its id
        /// </summary>
        /// <param name="projectId">project id</param>
        /// <returns>matching project record</returns>
        Task<ProjectRecord> GetByIdAsync(int projectId);

        /// <summary>
        /// creates a new project in the repository
        /// </summary>
        /// <param name="name">project name</param>
        /// <param name="description">project description</param>
        /// <param name="type">project type</param>
        /// <returns>created project record</returns>
        Task<ProjectRecord> AddAsync(string name, string description, TypeOfProject type);

        /// <summary>
        /// updates an existing project in the repository
        /// </summary>
        /// <param name="project">project record with updated values</param>
        Task UpdateAsync(ProjectRecord project);

        /// <summary>
        /// deletes a project by its id
        /// </summary>
        /// <param name="projectId">project id</param>
        Task DeleteAsync(int projectId);
    }
}