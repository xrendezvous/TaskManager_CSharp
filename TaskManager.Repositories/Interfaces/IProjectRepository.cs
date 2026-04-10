using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Enums;

namespace TaskManager.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<IReadOnlyList<ProjectRecord>> GetAllProjectsAsync();
        Task<ProjectRecord> GetByIdAsync(int projectId);
        Task<ProjectRecord> AddAsync(string name, string description, TypeOfProject type);
        Task UpdateAsync(ProjectRecord project);
        Task DeleteAsync(int projectId);
    }
}