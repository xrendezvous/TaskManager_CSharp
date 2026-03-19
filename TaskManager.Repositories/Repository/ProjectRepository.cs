using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Interface;
using TaskManager.Repositories.Storage;

namespace TaskManager.Repositories.Repositories
{
    public sealed class ProjectRepository : IProjectRepository
    {
        private readonly IStorageContext _storageContext;

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