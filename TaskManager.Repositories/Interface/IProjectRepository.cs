using TaskManager.Repositories.Entities;

namespace TaskManager.Repositories.Interface
{
    public interface IProjectRepository
    {
        IEnumerable<ProjectRecord> GetAllProjects();
        ProjectRecord GetById(int projectId);
    }
}
