using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Interfaces;
using TaskManager.Repositories.Storage;

namespace TaskManager.Repositories.Repositories
{
    /// <summary>
    /// access to project data stored in the storage context
    /// </summary>
    public sealed class ProjectRepository : IProjectRepository
    {
        private readonly IStorageContext _storageContext;

        /// <summary>
        /// initializes a new instance of the <see cref="ProjectRepository"/> class
        /// </summary>
        /// <param name="storageContext">the storage context used to access project data</param>
        public ProjectRepository(IStorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        public IEnumerable<ProjectRecord> GetAllProjects()
        {
            return _storageContext.GetProjects()
                .OrderBy(p => p.Id)
                .ToList();
        }

        public ProjectRecord GetById(int projectId)
        {
            var project = _storageContext.GetProject(projectId);

            if (project is null)
                throw new KeyNotFoundException($"Project with ID {projectId} was not found.");

            return project;
        }
    }
}