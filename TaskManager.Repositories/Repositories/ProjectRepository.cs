using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Interfaces;
using TaskManager.Repositories.Storage;

namespace TaskManager.Repositories.Repositories
{
    /// <summary>
    /// Provides access to project data stored in the storage context.
    /// </summary>
    public sealed class ProjectRepository : IProjectRepository
    {
        private readonly IStorageContext _storageContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectRepository"/> class.
        /// </summary>
        /// <param name="storageContext">The storage context used to access project data.</param>
        public ProjectRepository(IStorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        /// <summary>
        /// Gets all projects ordered by identifier.
        /// </summary>
        /// <returns>A collection of ordered project records.</returns>
        public IEnumerable<ProjectRecord> GetAllProjects()
        {
            return _storageContext.GetProjects()
                .OrderBy(p => p.Id)
                .ToList();
        }

        /// <summary>
        /// Gets a project by its identifier.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>The matching project record.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the project with the specified identifier is not found.
        /// </exception>
        public ProjectRecord GetById(int projectId)
        {
            var project = _storageContext.GetProject(projectId);

            if (project is null)
                throw new KeyNotFoundException($"Project with ID {projectId} was not found.");

            return project;
        }
    }
}