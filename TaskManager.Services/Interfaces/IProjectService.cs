using TaskManager.Services.DTO;

namespace TaskManager.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IReadOnlyList<ProjectListDto>> GetProjectsForListAsync(FilterProjectDto filter);
        Task<ProjectDetailsDto> GetProjectDetailsAsync(int projectId);
        Task<ProjectDetailsDto> CreateProjectAsync(CreateProjectDto dto);
        Task UpdateProjectAsync(int projectId, UpdateProjectDto dto);
        Task DeleteProjectAsync(int projectId);
    }
}