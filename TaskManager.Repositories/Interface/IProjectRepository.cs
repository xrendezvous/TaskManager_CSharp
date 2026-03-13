using TaskManager.Repositories.Entities;

namespace TaskManager.Repositories.Interface
{
    public interface IProjectRepository
    {
        List<ProjectRecord> GetAllProjects();
        ProjectRecord GetById(int projectId);
    }
}
