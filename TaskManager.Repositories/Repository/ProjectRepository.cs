using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Interface;
using TaskManager.Repositories.Storage;

namespace TaskManager.Repositories.Repositories
{
    public sealed class ProjectRepository : IProjectRepository
    {
        public List<ProjectRecord> GetAllProjects()
        {
            return DataStorage.Projects
                .OrderBy(p => p.Id)
                .ToList();
        }

        public ProjectRecord GetById(int projectId)
        {
            var project = DataStorage.Projects.FirstOrDefault(p => p.Id == projectId);

            if (project is null)
                throw new KeyNotFoundException($"Project with ID {projectId} was not found.");

            return project;
        }
    }
}