using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Enums;
using TaskManager.Repositories.Interfaces;
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

        public async Task<IReadOnlyList<ProjectRecord>> GetAllProjectsAsync()
        {
            return await _storageContext.GetProjectsAsync();
        }

        public async Task<ProjectRecord> GetByIdAsync(int projectId)
        {
            var project = await _storageContext.GetProjectAsync(projectId);

            if (project is null)
                throw new KeyNotFoundException($"Project with ID {projectId} was not found.");

            return project;
        }

        public Task<ProjectRecord> AddAsync(string name, string description, TypeOfProject type)
        {
            return _storageContext.AddProjectAsync(name, description, type);
        }

        public Task UpdateAsync(ProjectRecord project)
        {
            return _storageContext.UpdateProjectAsync(project);
        }

        public Task DeleteAsync(int projectId)
        {
            return _storageContext.DeleteProjectAsync(projectId);
        }
    }
}